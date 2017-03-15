using StAugustine.Models;
using StAugustine.ViewModel;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StAugustine.Controllers
{
    public class AssignSubjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AssignSubjects
        public async Task<ActionResult> Index()
        {
            return View(await db.AssignSubjects.ToListAsync());
        }

        // GET: AssignSubjects/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignSubject assignSubject = await db.AssignSubjects.FindAsync(id);
            if (assignSubject == null)
            {
                return HttpNotFound();
            }
            return View(assignSubject);
        }

        // GET: AssignSubjects/Create
        public ActionResult Create()
        {
            ViewBag.SubjectName = new MultiSelectList(db.Subjects, "CourseName", "CourseName");
            ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");
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
                if (model.SubjectName != null)
                {
                    foreach (var item in model.SubjectName)
                    {
                        var assigSubject = new AssignSubject()
                        {
                            ClassName = model.ClassName,
                            SubjectName = item,

                        };
                        db.AssignSubjects.Add(assigSubject);
                    }

                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "AssignSubjects");
                }
            }

            ViewBag.SubjectName = new MultiSelectList(db.Subjects, "CourseName", "CourseName");
            ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");
            return View(model);
        }

        // GET: AssignSubjects/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignSubject assignSubject = await db.AssignSubjects.FindAsync(id);
            if (assignSubject == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectName = new SelectList(db.Subjects, "CourseName", "CourseName");
            ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");
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
                db.Entry(assignSubject).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SubjectName = new SelectList(db.Subjects, "CourseName", "CourseName");
            ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");
            return View(assignSubject);
        }

        // GET: AssignSubjects/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignSubject assignSubject = await db.AssignSubjects.FindAsync(id);
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
            AssignSubject assignSubject = await db.AssignSubjects.FindAsync(id);
            db.AssignSubjects.Remove(assignSubject);
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
