using SwiftSkool.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HopeAcademySMS.Controllers
{
    public class AssignFormTeacherToClassesController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: AssignFormTeacherToClasses
        public async Task<ActionResult> Index()
        {
            return View(await _db.AssignFormTeacherToClasses.AsNoTracking().ToListAsync());
        }

        public PartialViewResult AddFormTeacher(string studentId)
        {
            //ViewBag.Username = studentId;
            ViewBag.Username = new SelectList(_db.Users.AsNoTracking(), "Username", "Username");
            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            return PartialView("AddFormTeacher");
        }

        // GET: AssignFormTeacherToClasses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignFormTeacherToClass assignFormTeacherToClass = await _db.AssignFormTeacherToClasses.FindAsync(id);
            if (assignFormTeacherToClass == null)
            {
                return HttpNotFound();
            }
            return View(assignFormTeacherToClass);
        }

        // GET: AssignFormTeacherToClasses/Create
        public ActionResult Create()
        {
            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.Username = new SelectList(_db.Users.AsNoTracking(), "Username", "Username");
            return View();
        }

        // POST: AssignFormTeacherToClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ClassName,Username")] AssignFormTeacherToClass assignFormTeacherToClass)
        {
            if (ModelState.IsValid)
            {
                _db.AssignFormTeacherToClasses.Add(assignFormTeacherToClass);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index", "Staffs");
            }

            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.Username = new SelectList(_db.Staffs.AsNoTracking(), "Username", "Username");
            return View(assignFormTeacherToClass);
        }

        // GET: AssignFormTeacherToClasses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignFormTeacherToClass assignFormTeacherToClass = await _db.AssignFormTeacherToClasses.FindAsync(id);
            if (assignFormTeacherToClass == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.Username = new SelectList(_db.Staffs.AsNoTracking(), "Username", "Username");
            return View(assignFormTeacherToClass);
        }

        // POST: AssignFormTeacherToClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ClassName,StaffName")] AssignFormTeacherToClass assignFormTeacherToClass)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(assignFormTeacherToClass).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.Username = new SelectList(_db.Staffs.AsNoTracking(), "Username", "Username");
            return View(assignFormTeacherToClass);
        }

        // GET: AssignFormTeacherToClasses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignFormTeacherToClass assignFormTeacherToClass = await _db.AssignFormTeacherToClasses.FindAsync(id);
            if (assignFormTeacherToClass == null)
            {
                return HttpNotFound();
            }
            return View(assignFormTeacherToClass);
        }

        // POST: AssignFormTeacherToClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AssignFormTeacherToClass assignFormTeacherToClass = await _db.AssignFormTeacherToClasses.FindAsync(id);
            if (assignFormTeacherToClass != null) _db.AssignFormTeacherToClasses.Remove(assignFormTeacherToClass);
            await _db.SaveChangesAsync();
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
