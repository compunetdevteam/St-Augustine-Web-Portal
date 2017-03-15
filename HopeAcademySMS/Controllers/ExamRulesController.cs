using StAugustine.Models;
using StAugustine.Models.CBT;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StAugustine.Controllers
{
    public class ExamRulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ExamRules
        public async Task<ActionResult> Index()
        {
            return View(await db.ExamRules.ToListAsync());
        }

        // GET: ExamRules/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamRule examRule = await db.ExamRules.FindAsync(id);
            if (examRule == null)
            {
                return HttpNotFound();
            }
            return View(examRule);
        }

        // GET: ExamRules/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExamRules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ClassName,CorrectMark,TotalQuestion,MaximunTime")] ExamRule model)
        {
            if (ModelState.IsValid)
            {
                var myRule = db.ExamRules.Where(x => x.ClassName.Equals(model.ClassName));
                var countFromDb = myRule.Count();
                if (countFromDb >= 1)
                {
                    return View("Error2");
                }
                else
                {
                    var examRule = new ExamRule()
                    {
                        ClassName = model.ClassName,
                        CorrectMark = model.CorrectMark,
                        TotalQuestion = model.TotalQuestion,
                        MaximunTime = model.MaximunTime
                    };
                    db.ExamRules.Add(examRule);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
                //db.ExamRules.Add(examRule);
                //await db.SaveChangesAsync();
                //return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: ExamRules/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamRule examRule = await db.ExamRules.FindAsync(id);
            if (examRule == null)
            {
                return HttpNotFound();
            }
            return View(examRule);
        }

        // POST: ExamRules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ClassName,CorrectMark,TotalQuestion,MaximunTime")] ExamRule examRule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(examRule).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(examRule);
        }

        // GET: ExamRules/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamRule examRule = await db.ExamRules.FindAsync(id);
            if (examRule == null)
            {
                return HttpNotFound();
            }
            return View(examRule);
        }

        // POST: ExamRules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ExamRule examRule = await db.ExamRules.FindAsync(id);
            db.ExamRules.Remove(examRule);
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
