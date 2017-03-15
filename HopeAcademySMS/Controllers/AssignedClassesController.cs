using PagedList;
using StAugustine.Models;
using StAugustine.ViewModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StAugustine.Controllers
{
    public class AssignedClassesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AssignedClasses
        public ActionResult Index(string sortOrder, string currentFilter, string search, int? page)
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
            var assignedList = from s in db.AssignedClasses select s;
            if (!String.IsNullOrEmpty(search))
            {
                assignedList = assignedList.Where(s => s.StudentId.ToUpper().Contains(search.ToUpper())
                                                     || s.ClassName.ToUpper().Contains(search.ToUpper())
                                                     || s.TermName.ToUpper().Contains(search.ToUpper()));

            }
            switch (sortOrder)
            {
                case "name_desc":
                    assignedList = assignedList.OrderByDescending(s => s.StudentId);
                    break;
                case "Date":
                    assignedList = assignedList.OrderBy(s => s.SessionName);
                    break;
                default:
                    assignedList = assignedList.OrderBy(s => s.ClassName);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(assignedList.ToPagedList(pageNumber, pageSize));
            //return View(await db.AssignedClasses.ToListAsync());
        }

        // GET: AssignedClasses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignedClass assignedClass = await db.AssignedClasses.FindAsync(id);
            if (assignedClass == null)
            {
                return HttpNotFound();
            }
            return View(assignedClass);
        }

        // GET: AssignedClasses/Create
        public ActionResult Create()
        {
            ViewBag.StudentId = new MultiSelectList(db.Students, "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");
            return View();
        }

        // POST: AssignedClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AssignedClassesViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.StudentId != null)
                {
                    foreach (var item in model.StudentId)
                    {
                        var assigClass = new AssignedClass()
                        {
                            StudentId = item,
                            ClassName = model.ClassName,
                            TermName = model.TermName.ToString(),
                            SessionName = model.SessionName
                        };
                        db.AssignedClasses.Add(assigClass);
                    }

                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "AssignedClasses");
                }

                //var assigClass = new AssignedClass()
                //{
                //    StudentId = assignedClass.StudentId,
                //    ClassName = assignedClass.ClassName,
                //    TermName = assignedClass.TermName.ToString(),
                //    SessionName = assignedClass.SessionName
                //};

                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new MultiSelectList(db.Students, "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");
            return View(model);
        }

        // GET: AssignedClasses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignedClass assignedClass = await db.AssignedClasses.FindAsync(id);
            if (assignedClass == null)
            {
                return HttpNotFound();
            }
            var myModel = new AssignedClassesViewModel();
            myModel.AssignedClassId = assignedClass.AssignedClassId;
            ViewBag.StudentId = new MultiSelectList(db.Students, "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");
            return View(myModel);
        }

        // POST: AssignedClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AssignedClassesViewModel assignedClass)
        {
            if (ModelState.IsValid)
            {
                var assigClass = new AssignedClass()
                {
                    AssignedClassId = assignedClass.AssignedClassId,
                    StudentId = assignedClass.StudentId.ToString(),
                    ClassName = assignedClass.ClassName,
                    TermName = assignedClass.TermName.ToString(),
                    SessionName = assignedClass.SessionName
                };
                db.Entry(assigClass).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new MultiSelectList(db.Students, "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");
            return View(assignedClass);
        }

        // GET: AssignedClasses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignedClass assignedClass = await db.AssignedClasses.FindAsync(id);
            if (assignedClass == null)
            {
                return HttpNotFound();
            }
            return View(assignedClass);
        }

        // POST: AssignedClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AssignedClass assignedClass = await db.AssignedClasses.FindAsync(id);
            db.AssignedClasses.Remove(assignedClass);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
