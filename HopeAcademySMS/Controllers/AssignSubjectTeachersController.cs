using HopeAcademySMS.ViewModel;
using Microsoft.AspNet.Identity;
using PagedList;
using SwiftSkool.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HopeAcademySMS.Controllers
{
    public class AssignSubjectTeachersController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: AssignSubjectTeachers
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string search,
                                        string SubjectName, string ClassName, int? page)
        {

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (search != null)
            {

                page = 1;
            }
            else
            {
                search = currentFilter;
            }
            ViewBag.CurrentFilter = search;
            var assignedList = from s in _db.AssignSubjectTeachers select s;
            if (User.IsInRole("Teacher"))
            {
                string name = User.Identity.GetUserName();
                //var user = db.Guardians.Where(c => c.UserName.Equals(name)).Select(s => s.Email).FirstOrDefault();

                assignedList = assignedList.Where(x => x.StaffName.Equals(name));

                //return View(subjectName);
            }
            else
            {
                if (!String.IsNullOrEmpty(search))
                {
                    assignedList = assignedList.Where(s => s.SubjectName.ToUpper().Contains(search.ToUpper())
                                                           || s.ClassName.ToUpper().Contains(search.ToUpper()));


                }
                else if (!String.IsNullOrEmpty(SubjectName) || !String.IsNullOrEmpty(ClassName))
                {
                    assignedList = assignedList.Where(s => s.SubjectName.ToUpper().Equals(SubjectName.ToUpper())
                                                           || s.ClassName.ToUpper().Contains(ClassName.ToUpper()));

                }
            }
            switch (sortOrder)
            {
                case "name_desc":
                    assignedList = assignedList.OrderByDescending(s => s.SubjectName);
                    break;
                case "Date":
                    assignedList = assignedList.OrderBy(s => s.SubjectName);
                    break;
                default:
                    assignedList = assignedList.OrderBy(s => s.ClassName);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.SubjectName = new SelectList(_db.Subjects.AsNoTracking(), "CourseName", "CourseName");
            var count = await assignedList.CountAsync();
            TempData["UserMessage"] = $"You Search result contains {count} Records ";
            TempData["Title"] = "Success.";
            return View(assignedList.ToPagedList(pageNumber, pageSize));
            //return View(await db.AssignSubjectTeachers.ToListAsync());
        }

        // GET: AssignSubjectTeachers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignSubjectTeacher assignSubjectTeacher = await _db.AssignSubjectTeachers.FindAsync(id);
            if (assignSubjectTeacher == null)
            {
                return HttpNotFound();
            }
            return View(assignSubjectTeacher);
        }

        // GET: AssignSubjectTeachers/Create
        public ActionResult Create()
        {
            ViewBag.SubjectName = new SelectList(_db.Subjects.AsNoTracking(), "CourseName", "CourseName");
            ViewBag.ClassName = new MultiSelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.StaffName = new SelectList(_db.Staffs.AsNoTracking(), "Username", "Username");
            return View();
        }

        // POST: AssignSubjectTeachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AssignSubjectTeacherVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.ClassName != null)
                {
                    int counter = 0;
                    string theClass = "";
                    string theName = "";
                    foreach (var item in model.ClassName)
                    {

                        var countFromDb = await _db.AssignSubjectTeachers.CountAsync(x => x.ClassName.Equals(item)
                                                            && x.SubjectName.Equals(model.SubjectName));


                        // var countFromDb = CA.Count();

                        if (countFromDb >= 1)
                        {
                            TempData["UserMessage"] = $"Admin have already assigned Teacher to  {model.SubjectName} in {item} Class";
                            TempData["Title"] = "Error.";
                            ViewBag.SubjectName = new SelectList(_db.Subjects.AsNoTracking(), "CourseName", "CourseName");
                            ViewBag.ClassName = new MultiSelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
                            ViewBag.StaffName = new SelectList(_db.Staffs.AsNoTracking(), "Username", "Username");
                            return View(model);
                        }

                        var assigSubjectTeacher = new AssignSubjectTeacher()
                        {
                            ClassName = item,
                            SubjectName = model.SubjectName,
                            StaffName = model.StaffName

                        };
                        _db.AssignSubjectTeachers.Add(assigSubjectTeacher);
                        counter += 1;
                        theClass = model.SubjectName;
                        theName = model.StaffName;
                    }
                    TempData["UserMessage"] = $" You have Assigned {theClass} Subject  to {theName} in {counter} class Successfully.";
                    TempData["Title"] = "Success.";
                    await _db.SaveChangesAsync();
                }
                return RedirectToAction("Index", "AssignSubjectTeachers");
            }
            return View(model);
        }

        // GET: AssignSubjectTeachers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignSubjectTeacher assignSubjectTeacher = await _db.AssignSubjectTeachers.FindAsync(id);
            if (assignSubjectTeacher == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectName = new SelectList(_db.Subjects.AsNoTracking(), "CourseName", "CourseName");
            ViewBag.ClassName = new MultiSelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.StaffName = new SelectList(_db.Staffs.AsNoTracking(), "Username", "Username");
            return View(assignSubjectTeacher);
        }

        // POST: AssignSubjectTeachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,SubjectName,ClassName,StaffName")] AssignSubjectTeacher assignSubjectTeacher)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(assignSubjectTeacher).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                TempData["UserMessage"] = $"Subject Teacher Updated Successfully Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            ViewBag.SubjectName = new SelectList(_db.Subjects.AsNoTracking(), "CourseName", "CourseName");
            ViewBag.ClassName = new MultiSelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.StaffName = new SelectList(_db.Staffs.AsNoTracking(), "Username", "Username");
            return View(assignSubjectTeacher);
        }

        // GET: AssignSubjectTeachers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignSubjectTeacher assignSubjectTeacher = await _db.AssignSubjectTeachers.FindAsync(id);
            if (assignSubjectTeacher == null)
            {
                return HttpNotFound();
            }
            return View(assignSubjectTeacher);
        }

        // POST: AssignSubjectTeachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AssignSubjectTeacher assignSubjectTeacher = await _db.AssignSubjectTeachers.FindAsync(id);
            if (assignSubjectTeacher != null) _db.AssignSubjectTeachers.Remove(assignSubjectTeacher);
            await _db.SaveChangesAsync();
            TempData["UserMessage"] = $"Subject Teacher Successfully Deleted";
            TempData["Title"] = "Deleted.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
