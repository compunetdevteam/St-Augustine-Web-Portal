using StAugustine.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StAugustine.Controllers
{
    public class FeePaymentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FeePayments
        public async Task<ActionResult> Index()
        {
            var feePayments = db.FeePayments.Include(f => f.Students);
            ViewBag.Term = new SelectList(db.Terms.AsNoTracking(), "TermName", "TermName");
            return View(await feePayments.ToListAsync());
        }

        public async Task<ActionResult> DebtorsList()
        {
            decimal value = 1m;
            var feePayments = db.FeePayments.Include(f => f.Students).Where(s => s.Remaining > value);
            ViewBag.Term = new SelectList(db.Terms.AsNoTracking(), "TermName", "TermName");
            return View(await feePayments.ToListAsync());
        }

        // GET: FeePayments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeePayment feePayment = await db.FeePayments.FindAsync(id);
            if (feePayment == null)
            {
                return HttpNotFound();
            }
            return View(feePayment);
        }

        // GET: FeePayments/Create
        public ActionResult Create()
        {
            DateTime datetime = new DateTime();
            datetime = DateTime.Now.Date;
            ViewBag.Date = datetime.ToShortDateString();
            ViewBag.StudentId = new SelectList(db.Students.AsNoTracking(), "StudentID", "FullName");
            ViewBag.FeeName = new SelectList(db.FeeTypes.AsNoTracking(), "FeeName", "FeeName");
            ViewBag.Session = new SelectList(db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.Term = new SelectList(db.Terms.AsNoTracking(), "TermName", "TermName");
            return View();
        }

        // POST: FeePayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,StudentId,FeeName,Term,Session,PaidFee,TotalAmount,PaymentMode,Date")] FeePayment feePayment)
        {
            if (ModelState.IsValid)
            {
                db.FeePayments.Add(feePayment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            DateTime datetime = new DateTime();
            datetime = DateTime.Now.Date;
            ViewBag.Date = datetime.ToShortDateString();
            ViewBag.StudentId = new SelectList(db.Students.AsNoTracking(), "StudentID", "FullName");
            ViewBag.FeeName = new SelectList(db.FeeTypes.AsNoTracking(), "FeeName", "FeeName");
            ViewBag.Session = new SelectList(db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.Term = new SelectList(db.Terms.AsNoTracking(), "TermName", "TermName");
            return View(feePayment);
        }

        // GET: FeePayments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeePayment feePayment = await db.FeePayments.FindAsync(id);
            if (feePayment == null)
            {
                return HttpNotFound();
            }
            DateTime datetime = new DateTime();
            datetime = DateTime.Now.Date;
            ViewBag.Date = datetime.ToShortDateString();
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FullName", feePayment.StudentId);
            ViewBag.FeeName = new SelectList(db.FeeTypes, "FeeName", "FeeName");

            ViewBag.Session = new SelectList(db.Sessions, "SessionName", "SessionName");
            return View(feePayment);
        }

        // POST: FeePayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,StudentId,FeeName,Term,Session,PaidFee,TotalAmount,PaymentMode,Date")] FeePayment feePayment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feePayment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            DateTime datetime = new DateTime();
            datetime = DateTime.Now.Date;
            ViewBag.Date = datetime.ToShortDateString();
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FullName", feePayment.StudentId);
            ViewBag.FeeName = new SelectList(db.FeeTypes, "FeeName", "FeeName");

            ViewBag.Session = new SelectList(db.Sessions, "SessionName", "SessionName");

            return View(feePayment);
        }

        // GET: FeePayments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeePayment feePayment = await db.FeePayments.FindAsync(id);
            if (feePayment == null)
            {
                return HttpNotFound();
            }
            return View(feePayment);
        }

        // POST: FeePayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FeePayment feePayment = await db.FeePayments.FindAsync(id);
            db.FeePayments.Remove(feePayment);
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
