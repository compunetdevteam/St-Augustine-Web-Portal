using Microsoft.AspNet.Identity;
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
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: AssignedClasses
        public ActionResult Index(string sortOrder, string currentFilter, string search,
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
            int pageSize = 15;
            ViewBag.CurrentFilter = search;
            var assignedList = from s in _db.AssignedClasses select s;
            if (User.IsInRole("Teacher"))
            {
                string name = User.Identity.GetUserName();
                //var user = db.Guardians.Where(c => c.UserName.Equals(name)).Select(s => s.Email).FirstOrDefault();
                assignedList = assignedList.AsNoTracking().Where(x => x.ClassName.Equals(name));

                //return View(subjectName);
            }
            else
            {
                if (!String.IsNullOrEmpty(search))
                {
                    assignedList = assignedList.AsNoTracking().AsNoTracking().Where(s => s.StudentId.ToUpper().Contains(search.ToUpper())
                                                           || s.ClassName.ToUpper().Contains(search.ToUpper())
                                                           || s.TermName.ToUpper().Contains(search.ToUpper()));

                }
                else if (!String.IsNullOrEmpty(SessionName) || !String.IsNullOrEmpty(ClassName))
                {
                    assignedList = assignedList.AsNoTracking().Where(s => s.SessionName.Contains(SessionName)
                                                           && s.ClassName.ToUpper().Contains(ClassName.ToUpper())
                                                           && s.TermName.ToUpper().Contains(TermName.ToUpper()));
                    pageSize = assignedList.Count();
                }
            }
            switch (sortOrder)
            {
                case "name_desc":
                    assignedList = assignedList.AsNoTracking().OrderByDescending(s => s.StudentId);
                    break;
                case "Date":
                    assignedList = assignedList.AsNoTracking().OrderBy(s => s.SessionName);
                    break;
                default:
                    assignedList = assignedList.AsNoTracking().OrderBy(s => s.ClassName);
                    break;
            }

            int pageNumber = (page ?? 1);

            if (User.IsInRole("Teacher"))
            {
                string name = User.Identity.GetUserName();
                var subjectList = _db.AssignSubjectTeachers.AsNoTracking().Where(x => x.StaffName.Equals(name));
                ViewBag.ClassName = new SelectList(subjectList.AsNoTracking(), "ClassName", "ClassName");
            }
            else
            {
                ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            }
            ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.TermName = new SelectList(_db.Terms.AsNoTracking(), "TermName", "TermName");
            var count = assignedList.Count();
            TempData["Index"] = $"You Search result contains {count} Records ";
            TempData["Title"] = "Success.";
            return View(assignedList.AsNoTracking().ToPagedList(pageNumber, pageSize));
            //return View(await db.AssignedClasses.ToListAsync());
        }

        // GET: AssignedClasses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignedClass assignedClass = await _db.AssignedClasses.FindAsync(id);
            if (assignedClass == null)
            {
                return HttpNotFound();
            }
            return View(assignedClass);
        }

        // GET: AssignedClasses/Create
        public ActionResult Create()
        {
            if (User.IsInRole("Teacher"))
            {
                string name = User.Identity.GetUserName();
                var subjectList = _db.AssignSubjectTeachers.AsNoTracking().Where(x => x.StaffName.Equals(name));
                ViewBag.ClassName = new SelectList(subjectList.AsNoTracking(), "ClassName", "ClassName");
            }
            else
            {
                ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            }
            ViewBag.StudentId = new MultiSelectList(_db.Students.AsNoTracking(), "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            // ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");
            ViewBag.TermName = new SelectList(_db.Terms.AsNoTracking(), "TermName", "TermName");
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
                    int counter = 0;
                    string theClass = "";
                    foreach (var item in model.StudentId)
                    {
                        var countFromDb = await _db.AssignedClasses.AsNoTracking().CountAsync(x => x.TermName.Equals(model.TermName.ToString())
                                                              && x.SessionName.Equals(model.SessionName)
                                                              && x.StudentId.Equals(item));

                        if (countFromDb >= 1)
                        {
                            TempData["UserMessage"] = "You have already Assigned Class these student";
                            TempData["Title"] = "Error.";
                            ViewBag.StudentId = new MultiSelectList(_db.Students.AsNoTracking(), "StudentID", "FullName");
                            ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");
                            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
                            ViewBag.TermName = new SelectList(_db.Terms.AsNoTracking(), "TermName", "TermName");
                            return View(model);
                        }
                        else
                        {
                            var studentName = await _db.Students.AsNoTracking().Where(x => x.StudentId.Equals(item))
                                                .Select(s => s.FullName)
                                                .FirstOrDefaultAsync();
                            var assigClass = new AssignedClass()
                            {
                                StudentId = item,
                                ClassName = model.ClassName,
                                TermName = model.TermName,
                                SessionName = model.SessionName,
                                StudentName = studentName
                            };
                            _db.AssignedClasses.Add(assigClass);
                            counter += 1;
                            theClass = model.ClassName;
                        }
                    }

                    await _db.SaveChangesAsync();
                    TempData["UserMessage"] = $"You have Assigned to {counter} Student(s) to {theClass} Successfully.";
                    TempData["Title"] = "Success.";
                    return RedirectToAction("Index", "AssignedClasses");
                }
                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new MultiSelectList(_db.Students.AsNoTracking(), "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.TermName = new SelectList(_db.Terms.AsNoTracking(), "TermName", "TermName");
            return View(model);
        }

        // GET: AssignedClasses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignedClass assignedClass = await _db.AssignedClasses.FindAsync(id);
            if (assignedClass == null)
            {
                return HttpNotFound();
            }
            var myModel = new AssignedClassesViewModel();
            myModel.AssignedClassId = assignedClass.AssignedClassId;
            ViewBag.StudentId = new MultiSelectList(_db.Students.AsNoTracking(), "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
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
                var studentName = await _db.Students.AsNoTracking().Where(x => x.StudentId.Equals(assignedClass.StudentId))
                                   .Select(s => s.FullName)
                                   .FirstOrDefaultAsync();
                var assigClass = new AssignedClass()
                {
                    AssignedClassId = assignedClass.AssignedClassId,
                    StudentId = assignedClass.StudentId.ToString(),
                    ClassName = assignedClass.ClassName,
                    TermName = assignedClass.TermName.ToString(),
                    SessionName = assignedClass.SessionName
                };
                _db.Entry(assigClass).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                TempData["UserMessage"] = "Student Class Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new MultiSelectList(_db.Students.AsNoTracking(), "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.TermName = new SelectList(_db.Terms.AsNoTracking(), "TermName", "TermName");
            return View(assignedClass);
        }

        // GET: AssignedClasses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignedClass assignedClass = await _db.AssignedClasses.FindAsync(id);
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
            AssignedClass assignedClass = await _db.AssignedClasses.FindAsync(id);
            if (assignedClass != null) _db.AssignedClasses.Remove(assignedClass);
            await _db.SaveChangesAsync();
            TempData["UserMessage"] = "You have removed Student from Class";
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
