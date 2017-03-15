using StAugustine.Models;
using StAugustine.Models.CBT;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StAugustine.Controllers
{
    public class QuestionAnswersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: QuestionAnswers
        public async Task<ActionResult> Index()
        {
            return View(await db.QuestionAnswers.ToListAsync());
        }

        // GET: QuestionAnswers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionAnswer questionAnswer = await db.QuestionAnswers.FindAsync(id);
            if (questionAnswer == null)
            {
                return HttpNotFound();
            }
            return View(questionAnswer);
        }

        // GET: QuestionAnswers/Create
        public ActionResult Create()
        {
            ViewBag.SubjectName = new SelectList(db.Subjects, "CourseCode", "CourseCode");
            ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");
            return View();
        }

        // POST: QuestionAnswers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,SubjectName,ClassName,Question,Option1,Option2,Option3,Option4,Answer,QuestionHint")] QuestionAnswer questionAnswer)
        {
            if (ModelState.IsValid)
            {
                db.QuestionAnswers.Add(questionAnswer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SubjectName = new SelectList(db.Subjects, "CourseCode", "CourseCode");
            ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");
            return View(questionAnswer);
        }

        // GET: QuestionAnswers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionAnswer questionAnswer = await db.QuestionAnswers.FindAsync(id);
            if (questionAnswer == null)
            {
                return HttpNotFound();
            }
            return View(questionAnswer);
        }

        // POST: QuestionAnswers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,SubjectName,ClassName,Question,Option1,Option2,Option3,Option4,Answer,QuestionHint")] QuestionAnswer questionAnswer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(questionAnswer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(questionAnswer);
        }

        // GET: QuestionAnswers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionAnswer questionAnswer = await db.QuestionAnswers.FindAsync(id);
            if (questionAnswer == null)
            {
                return HttpNotFound();
            }
            return View(questionAnswer);
        }

        // POST: QuestionAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            QuestionAnswer questionAnswer = await db.QuestionAnswers.FindAsync(id);
            db.QuestionAnswers.Remove(questionAnswer);
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
