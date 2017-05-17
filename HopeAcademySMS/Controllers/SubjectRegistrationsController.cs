using PagedList;
using SwiftSkool.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkool.Controllers
{
    public class SubjectRegistrationsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: SubjectRegistrations
        public ActionResult Index(string sortOrder, string currentFilter, string search, string StudentId,
                                         string SessionName, string ClassName, string TermName, int? page)
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
            var assignedList = from s in _db.SubjectRegistrations select s;
            if (!String.IsNullOrEmpty(search))
            {
                assignedList = assignedList.Where(s => s.StudentId.ToUpper().Contains(search.ToUpper())
                                                     || s.ClassName.ToUpper().Contains(search.ToUpper())
                                                     || s.TermName.ToUpper().Contains(search.ToUpper()));

            }
            else if (!String.IsNullOrEmpty(SessionName) || !String.IsNullOrEmpty(ClassName)
                            || !String.IsNullOrEmpty(StudentId) || !String.IsNullOrEmpty(TermName))
            {
                assignedList = assignedList.Where(s => s.SessionName.Contains(SessionName)
                                                    && s.ClassName.ToUpper().Contains(ClassName.ToUpper())
                                                    && s.TermName.ToUpper().Contains(TermName.ToUpper())
                                                    && s.StudentId.ToUpper().Contains(StudentId.ToUpper()));
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
            int pageSize = 15;
            int pageNumber = (page ?? 1);

            ViewBag.SessionName = new SelectList(_db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(_db.Classes, "FullClassName", "FullClassName");
            ViewBag.TermName = new SelectList(_db.Terms, "TermName", "TermName");
            ViewBag.StudentId = new SelectList(_db.Students, "StudentID", "FullName");
            //var count = assignedList.Count();
            //TempData["UserMessage"] = $"You Search result contains {count} Records ";
            //TempData["Title"] = "Success.";
            return View(assignedList.ToPagedList(pageNumber, pageSize));
            //return View(await db.AssignedClasses.ToListAsync());
        }

        // GET: SubjectRegistrations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectRegistration subjectRegistration = await _db.SubjectRegistrations.FindAsync(id);
            if (subjectRegistration == null)
            {
                return HttpNotFound();
            }
            return View(subjectRegistration);
        }

        // GET: SubjectRegistrations/Create
        public ActionResult Create()
        {
            ViewBag.StudentId = new SelectList(_db.Students, "StudentID", "FullName");
            ViewBag.SubjectName = new MultiSelectList(_db.Subjects, "CourseName", "CourseName");
            ViewBag.TermName = new SelectList(_db.Terms, "TermName", "TermName");
            ViewBag.SessionName = new SelectList(_db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(_db.Classes, "FullClassName", "FullClassName");
            return View();
        }

        // POST: SubjectRegistrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,StudentId,ClassName,TermName,SessionName,SubjectName")] SubjectRegistrationVm model)
        {
            if (ModelState.IsValid)
            {
                int counter = 0;
                string theClass = "";
                foreach (var subject in model.SubjectName)
                {
                    var studentName = _db.Students.Where(x => x.StudentId.Equals(model.StudentId))
                                              .Select(s => s.FullName)
                                              .FirstOrDefault();
                    var countFromDb = _db.SubjectRegistrations.Count(x => x.ClassName.Equals(model.ClassName)
                                                             && x.TermName.Equals(model.TermName.ToString())
                                                             && x.SessionName.Equals(model.SessionName)
                                                             && x.StudentId.Equals(model.StudentId));

                    // var countFromDb = CA.Count();

                    if (countFromDb >= 1)
                    {
                        TempData["UserMessage"] = $"Admin have already assigned {subject} subject to this {studentName} Student";
                        TempData["Title"] = "Error.";
                        ViewBag.StudentId = new SelectList(_db.Students, "StudentID", "FullName");
                        ViewBag.SubjectName = new MultiSelectList(_db.Subjects, "CourseName", "CourseName");
                        ViewBag.TermName = new SelectList(_db.Terms, "TermName", "TermName");
                        ViewBag.SessionName = new SelectList(_db.Sessions, "SessionName", "SessionName");
                        ViewBag.ClassName = new SelectList(_db.Classes, "FullClassName", "FullClassName");
                        return View(model);
                    }

                    SubjectRegistration mySubject = new SubjectRegistration()
                    {
                        StudentId = model.StudentId,
                        StudentName = studentName,
                        ClassName = model.ClassName,
                        TermName = model.TermName,
                        SessionName = model.SessionName,
                        SubjectCode = subject
                    };
                    _db.SubjectRegistrations.Add(mySubject);
                    counter += 1;
                    theClass = studentName;
                }

                await _db.SaveChangesAsync();
                TempData["UserMessage"] = $" You have Assigned {counter} Subject(s)  to {theClass} Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(_db.Students, "StudentID", "FullName");
            ViewBag.SubjectName = new MultiSelectList(_db.Subjects, "CourseName", "CourseName");
            ViewBag.TermName = new SelectList(_db.Terms, "TermName", "TermName");
            ViewBag.SessionName = new SelectList(_db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(_db.Classes, "FullClassName", "FullClassName");

            return View(model);
        }

        // GET: SubjectRegistrations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectRegistration subjectRegistration = await _db.SubjectRegistrations.FindAsync(id);
            if (subjectRegistration == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(_db.Students, "StudentID", "FullName");
            ViewBag.SubjectName = new MultiSelectList(_db.Subjects, "CourseName", "CourseName");
            ViewBag.TermName = new SelectList(_db.Terms, "TermName", "TermName");
            ViewBag.SessionName = new SelectList(_db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(_db.Classes, "FullClassName", "FullClassName");
            return View(subjectRegistration);
        }

        // POST: SubjectRegistrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,StudentId,ClassName,TermName,SessionName,SubjectName")] SubjectRegistration model)
        {
            if (ModelState.IsValid)
            {
                var studentName = _db.Students.Where(x => x.StudentId.Equals(model.StudentId))
                                              .Select(s => s.FullName)
                                              .FirstOrDefault();
                var subjectRegistration = new SubjectRegistration()
                {
                    StudentId = model.StudentId,
                    StudentName = studentName,
                    ClassName = model.ClassName,
                    TermName = model.TermName,
                    SessionName = model.SessionName,
                    SubjectCode = model.SubjectCode
                };
                _db.Entry(subjectRegistration).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                TempData["UserMessage"] = "Subject Registration Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(_db.Students, "StudentID", "FullName");
            ViewBag.SubjectName = new MultiSelectList(_db.Subjects, "CourseName", "CourseName");
            ViewBag.TermName = new SelectList(_db.Terms, "TermName", "TermName");
            ViewBag.SessionName = new SelectList(_db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(_db.Classes, "FullClassName", "FullClassName");
            return View(model);
        }

        // GET: SubjectRegistrations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectRegistration subjectRegistration = await _db.SubjectRegistrations.FindAsync(id);
            if (subjectRegistration == null)
            {
                return HttpNotFound();
            }
            return View(subjectRegistration);
        }

        // POST: SubjectRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SubjectRegistration subjectRegistration = await _db.SubjectRegistrations.FindAsync(id);
            _db.SubjectRegistrations.Remove(subjectRegistration);
            await _db.SaveChangesAsync();
            TempData["UserMessage"] = "Subject has been removed Successfully";
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
