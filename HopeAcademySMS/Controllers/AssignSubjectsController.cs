using PagedList;
using SwiftSkool.Models;
using SwiftSkool.ViewModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkool.Controllers
{
    public class AssignSubjectsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: AssignSubjects
        public ActionResult Index(string sortOrder, string currentFilter, string search,
                                         string ClassName, int? page)
        {

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (search == null)
            {

                page = 1;
            }
            else
            {
                search = currentFilter;
            }
            ViewBag.CurrentFilter = search;
            var assignedList = from s in _db.AssignSubjects select s;
            if (!String.IsNullOrEmpty(search))
            {
                assignedList = assignedList.Where(s => s.ClassName.ToUpper().Contains(search.ToUpper())
                                                       || s.SubjectName.ToUpper().Contains(search.ToUpper()));


            }
            else if (!String.IsNullOrEmpty(ClassName))
            {
                assignedList = assignedList.Where(s => s.ClassName.ToUpper().Contains(ClassName.ToUpper()));

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
            int pageSize = 17;
            int pageNumber = (page ?? 1);

            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");

            return View(assignedList.ToPagedList(pageNumber, pageSize));
            //return View(await db.AssignSubjects.ToListAsync());
        }

        // GET: AssignSubjects/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignSubject assignSubject = await _db.AssignSubjects.FindAsync(id);
            if (assignSubject == null)
            {
                return HttpNotFound();
            }
            return View(assignSubject);
        }

        // GET: AssignSubjects/Create
        public ActionResult Create()
        {
            ViewBag.SubjectName = new MultiSelectList(_db.Subjects.AsNoTracking(), "CourseName", "CourseName");
            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            return View();
        }

        // POST: AssignSubjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AssignSubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                int counter = 0;
                string theClass = "";
                if (model.SubjectName != null)
                {
                    foreach (var item in model.SubjectName)
                    {
                        var CA = _db.AssignSubjects.Where(x => x.ClassName.Equals(model.ClassName)
                                                              && x.SubjectName.Equals(item));
                        var countFromDb = await CA.CountAsync();
                        if (countFromDb >= 1)
                        {
                            TempData["UserMessage"] = $"Admin have already assigned {item} subject to {model.ClassName} Class";
                            TempData["Title"] = "Error.";
                            ViewBag.SubjectName = new MultiSelectList(_db.Subjects.AsNoTracking(), "CourseName", "CourseName");
                            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
                            return View(model);
                        }
                        var assigSubject = new AssignSubject()
                        {
                            ClassName = model.ClassName,
                            SubjectName = item,
                        };
                        _db.AssignSubjects.Add(assigSubject);
                        counter += 1;
                        theClass = model.ClassName;
                    }

                    await _db.SaveChangesAsync();

                    TempData["UserMessage"] = $" You have Assigned {counter} Subject(s)  to {theClass} Successfully.";
                    TempData["Title"] = "Success.";
                    return RedirectToAction("Index", "AssignSubjects");
                }
            }

            ViewBag.SubjectName = new MultiSelectList(_db.Subjects.AsNoTracking(), "CourseName", "CourseName");
            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            return View(model);
        }

        // GET: AssignSubjects/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignSubject assignSubject = await _db.AssignSubjects.FindAsync(id);
            if (assignSubject == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectName = new SelectList(_db.Subjects.AsNoTracking(), "CourseName", "CourseName");
            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            return View(assignSubject);
        }

        // POST: AssignSubjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AssignSubjectId,ClassName,SubjectName")] AssignSubject assignSubject)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(assignSubject).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                TempData["UserMessage"] = "Assigned Subject Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            ViewBag.SubjectName = new SelectList(_db.Subjects.AsNoTracking(), "CourseName", "CourseName");
            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            return View(assignSubject);
        }

        // GET: AssignSubjects/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignSubject assignSubject = await _db.AssignSubjects.FindAsync(id);
            if (assignSubject == null)
            {
                return HttpNotFound();
            }
            return View(assignSubject);
        }

        // POST: AssignSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AssignSubject assignSubject = await _db.AssignSubjects.FindAsync(id);
            if (assignSubject != null) _db.AssignSubjects.Remove(assignSubject);
            await _db.SaveChangesAsync();
            TempData["UserMessage"] = "Subject removed from Class Successfully.";
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
