using Microsoft.AspNet.Identity;
using SwiftSkool.Models;
using SwiftSkool.ViewModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkool.Controllers
{
    public class ClassesController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Classes
        public async Task<ActionResult> Index()
        {
            return View(await _db.Classes.ToListAsync());
        }

        public async Task<ActionResult> FormTeacher()
        {
            var myForm = new FormDataViewModel();
            string username = User.Identity.GetUserName();
            var className = _db.AssignFormTeacherToClasses.Where(x => x.Username.Equals(username))
                                                .Select(s => s.ClassName)
                                                .FirstOrDefault();
            myForm.AssignedClasses = await _db.AssignedClasses.Where(x => x.ClassName.Equals(className) && x.TermName.Equals("First")
                                                          && x.SessionName.Equals("2016-2017")).ToListAsync();

            ViewBag.SubjectCode = new SelectList(_db.Subjects, "CourseName", "CourseName");
            ViewBag.StudentId = new SelectList(_db.Students, "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(_db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(_db.Classes, "FullClassName", "FullClassName");
            // return View(await db.Classes.ToListAsync());
            return View(myForm);
        }

        // GET: Classes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = await _db.Classes.FindAsync(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // GET: Classes/Create
        public ActionResult Create()
        {
            ViewBag.SchoolName = new SelectList(_db.SchoolClasses, "ClassCode", "ClassCode");
            return View();
        }

        // POST: Classes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Class model)
        {
            if (ModelState.IsValid)
            {
                var myClass = _db.Classes.Where(x => x.FullClassName.Equals(model.FullClassName));
                var countFromDb = myClass.Count();
                if (countFromDb >= 1)
                {
                    ViewBag.SchoolName = new SelectList(_db.SchoolClasses, "ClassCode", "ClassCode");
                    TempData["UserMessage"] = "Class Already Exist in Database";
                    TempData["Title"] = "Deleted.";
                    return View(model);
                }
                var @class = new Class()
                {
                    ClassType = model.ClassType.Trim().ToUpper(),
                    SchoolName = model.SchoolName.Trim(),
                    ClassLevel = model.ClassLevel
                };
                _db.Classes.Add(@class);
                await _db.SaveChangesAsync();
                TempData["UserMessage"] = "Class Added Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
                //db.Classes.Add(@class);
                //await db.SaveChangesAsync();
                //return RedirectToAction("Index");
            }
            ViewBag.SchoolName = new SelectList(_db.SchoolClasses, "ClassCode", "ClassCode");
            return View(model);
        }

        // GET: Classes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = await _db.Classes.FindAsync(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            ViewBag.SchoolName = new SelectList(_db.SchoolClasses, "ClassCode", "ClassCode");
            return View(@class);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ClassID,Name,ClassType")] Class model)
        {
            if (ModelState.IsValid)
            {
                var @class = new Class()
                {
                    ClassType = model.ClassType.Trim().ToUpper(),
                    SchoolName = model.SchoolName.Trim(),
                    ClassLevel = model.ClassLevel
                };
                _db.Entry(@class).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                TempData["UserMessage"] = "Class Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            ViewBag.SchoolName = new SelectList(_db.SchoolClasses, "ClassCode", "ClassCode");
            return View(model);
        }

        // GET: Classes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = await _db.Classes.FindAsync(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Class @class = await _db.Classes.FindAsync(id);
            _db.Classes.Remove(@class);
            await _db.SaveChangesAsync();
            TempData["UserMessage"] = "Class Has Been Deleted";
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
