﻿using SwiftSkool.Models;
using SwiftSkool.Models.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SwiftSkool.Controllers
{
    public class PostsController : Controller
    {
        //private ApplicationDbContext model = new BlogContext();
        private ApplicationDbContext model = new ApplicationDbContext();
        private const int PostPerPage = 3;
        private const int PostPerFeed = 25;

        // GET: Posts
        public ActionResult Index(int? id, string category)
        {
            int pageNumber = id ?? 0;
            var items = (from post in model.Posts
                        where post.Title.Contains(category.ToUpper())
                        select post);
            
            //if (!String.IsNullOrEmpty(category))
            //{               
            //    items = items.Where(s => s.Title.ToUpper().Contains(category.ToUpper())); 
            //}
            if (String.IsNullOrEmpty(category))
            {
                IEnumerable<Post> posts = (from post in model.Posts
                                           where post.DateTime < DateTime.Now
                                           orderby post.DateTime descending
                                           select post).Skip(pageNumber * PostPerPage)
                                  .Take(PostPerPage + 1);

                ViewBag.IsPreviousLinkVisible = pageNumber > 0;
                ViewBag.IsNextLinkVisible = posts.Count() > PostPerPage;
                ViewBag.PageNumber = pageNumber;
                ViewBag.IsAdmin = IsAdmin;

                return View(posts.Take(PostPerPage));
            }
            return View(items);

        }

        [ValidateInput(false)]
        public ActionResult Update(int? id, string title, string body, DateTime dateTime, string tag)
        {
            //if (!IsAdmin)
            //{
            //    return RedirectToAction("Index");
            //}

            Post post = GetPost(id);
            post.Title = title;
            post.Body = body;
            post.DateTime = dateTime;
            post.Tags.Clear();

            tag = tag ?? string.Empty;
            string[] tagNames = tag.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string tagName in tagNames)
            {
                post.Tags.Add(GetTag(tagName));
            }

            if (!id.HasValue)
            {
                model.Posts.Add(post);
                //model.AddToPost(post);
            }
            model.SaveChanges();
            return RedirectToAction("Details", new { id = post.ID });
        }

        private Tag GetTag(string tagName)
        {
            return model.Tags.Where(x => x.Name == tagName).FirstOrDefault() ?? new Tag() { Name = tagName };
        }

        private Post GetPost(int? id)
        {
            return id.HasValue ? model.Posts.Where(x => x.ID == id).First() : new Post() { ID = -1 };
        }


        public bool IsAdmin { get { return Session["IsAdmin"] != null && (bool)Session["IsAdmin"]; } }
        //public bool IsAdmin { get { return true;/* Session["IsAdmin"] != null && (bool)Session["IsAdmin"];*/ } }

        // GET: Posts/Edit/5       
        public ActionResult Edit(int? id)
        {
            Post post = GetPost(id);
            StringBuilder tagList = new StringBuilder();
            foreach (Tag tag in post.Tags)
            {
                tagList.AppendFormat("{0} ", tag.Name);
            }

            ViewBag.Tags = tagList.ToString();
            return View(post);
        }

        public ActionResult Details(int id, string category)
        {
            if (!String.IsNullOrEmpty(category))
            {
                var items = from i in model.Posts
                             where i.Title.Contains(category.ToUpper())
                             select i;
                return View(items);
            }
           
                Post post = GetPost(id);
                ViewBag.IsAdmin = IsAdmin;
                return View(post);
         
            
        }

        [ValidateInput(false)]
        public ActionResult Comment(int id, string name, string email, string body)
        {
            Post post = GetPost(id);
            Comment comment = new Comment();
            comment.Post = post;
            comment.DateTime = DateTime.Now;
            comment.Name = name;
            comment.Email = email;
            comment.Body = body;

            model.Comments.Add(comment);
            model.SaveChanges();

            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult Delete(int id)
        {
            //if (IsAdmin)
            //{
                Post post = GetPost(id);
                model.Posts.Remove(post);
                model.SaveChanges();
            //}

            return RedirectToAction("Index");
        }

        public ActionResult DeleteComment(int id)
        {
            if (IsAdmin)
            {
                // Comment comment = model.Comments.Where(x => x.ID == id).First();
                Comment comment = model.Comments.Where(x => x.PostID == id).First();
                model.Comments.Remove(comment);
                model.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Tags(string id)
        {
            Tag tag = GetTag(id);
            ViewBag.IsAdmin = IsAdmin;
            return View("index", tag.Posts);
        }

        public ActionResult RSS()
        {
            IEnumerable<SyndicationItem> posts =
                (from post in model.Posts
                 where post.DateTime < DateTime.Now
                 orderby post.DateTime descending
                 select post).Take(PostPerFeed).ToList().Select(x => GetSyndicationItem(x));

            SyndicationFeed feed = new SyndicationFeed("HeritageTv", "HeritageTv blog", new Uri("http://localhost:60210/"), posts);
            Rss20FeedFormatter formattedFeed = new Rss20FeedFormatter(feed);
            return new FeedResult(formattedFeed);
        }

        private SyndicationItem GetSyndicationItem(Post post)
        {
            return new SyndicationItem(post.Title, post.Body, new Uri("http://localhost:60210/posts/details" + post.ID));
        }

        public ActionResult Create(int? id)
        {
            Post post = GetPost(id);
            StringBuilder tagList = new StringBuilder();
            foreach (Tag tag in post.Tags)
            {
                tagList.AppendFormat("{0} ", tag.Name);
            }

            ViewBag.Tags = tagList.ToString();
            return View(post);
        }

        [ValidateInput(false)]
        public ActionResult CreatePost(int? id, string title, string body, DateTime dateTime, string tag)
        {
        //    if (!IsAdmin)
        //    {
        //        return RedirectToAction("Index");
        //    }

            Post post = GetPost(id);
            post.Title = title;
            post.Body = body;
            post.DateTime = dateTime;
            post.Tags.Clear();

            tag = tag ?? string.Empty;
            string[] tagNames = tag.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string tagName in tagNames)
            {
                post.Tags.Add(GetTag(tagName));
            }

            if (!id.HasValue)
            {
                model.Posts.Add(post);
                //model.AddToPost(post);
            }
            model.SaveChanges();
            return RedirectToAction("Index");
        }

        public PartialViewResult Menu()
        {
            IEnumerable<string> categories = model.Posts.Select(s => s.Title)
                                                            .OrderBy(s => s)
                                                            .Take(5);
            return PartialView(categories);
        }
        public PartialViewResult LIstCategories()
        {
            IEnumerable<string> categories = model.Posts.Select(s => s.Title)
                                                            .OrderBy(s => s)
                                                            .Take(5);
            return PartialView(categories);
        }
    }
}
