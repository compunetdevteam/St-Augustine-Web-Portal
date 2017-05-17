using SwiftSkool.Models;
using SwiftSkool.ViewModel;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkool.Controllers
{
    public class AffectivesController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Affectives
        public async Task<ActionResult> Index()
        {
            return View(await _db.Affectives.AsNoTracking().ToListAsync());
        }

        // GET: Affectives/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Affective affective = await _db.Affectives.FindAsync(id);
            if (affective == null)
            {
                return HttpNotFound();
            }
            return View(affective);
        }

        // GET: Affectives/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Affectives/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "Id,StudentId,TermName,SessionName,ClassName,Honesty,SelfConfidence,Sociability,Punctuality,Neatness,Initiative,Organization,AttendanceInClass,HonestyAndReliability")] Affective affective)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Affectives.Add(affective);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(affective);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AffectiveViewModel model)
        {
            if (ModelState.IsValid)
            {
                Affective affective = new Affective()
                {
                    StudentId = model.StudentId,
                    TermName = model.TermName,
                    SessionName = model.SessionName,
                    ClassName = model.ClassName,
                    Honesty = model.Honesty.ToString(),
                    SelfConfidence = model.SelfConfidence.ToString(),
                    Sociability = model.Sociability.ToString(),
                    Punctuality = model.Punctuality.ToString(),
                    Neatness = model.Neatness.ToString(),
                    Initiative = model.Initiative.ToString(),
                    Organization = model.Organization.ToString(),
                    AttendanceInClass = model.AttendanceInClass.ToString(),
                    HonestyAndReliability = model.HonestyAndReliability.ToString(),

                };
                _db.Affectives.Add(affective);
                await _db.SaveChangesAsync();
                return RedirectToAction("FormTeacher", "Classes");
            }

            return View(model);
        }

        public PartialViewResult AddAffective(string studentId)
        {
            ViewBag.StudentId = studentId;
            ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.TermName = new SelectList(_db.Terms.AsNoTracking(), "TermName", "TermName");
            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            return PartialView("PartialAffective");
        }

        // GET: Affectives/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Affective affective = await _db.Affectives.FindAsync(id);
            if (affective == null)
            {
                return HttpNotFound();
            }
            return View(affective);
        }

        // POST: Affectives/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,StudentId,TermName,SessionName,ClassName,Honesty,SelfConfidence,Sociability,Punctuality,Neatness,Initiative,Organization,AttendanceInClass,HonestyAndReliability")] Affective affective)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(affective).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(affective);
        }

        // GET: Affectives/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Affective affective = await _db.Affectives.FindAsync(id);
            if (affective == null)
            {
                return HttpNotFound();
            }
            return View(affective);
        }

        // POST: Affectives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Affective affective = await _db.Affectives.FindAsync(id);
            if (affective != null) _db.Affectives.Remove(affective);
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
