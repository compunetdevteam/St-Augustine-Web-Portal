using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StAugustine.Models;

namespace StAugustine.Controllers
{
    public class FeeTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FeeTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.FeeTypes.ToListAsync());
        }

        // GET: FeeTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeeType feeType = await db.FeeTypes.FindAsync(id);
            if (feeType == null)
            {
                return HttpNotFound();
            }
            return View(feeType);
        }

        // GET: FeeTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FeeTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,FeeName,Description")] FeeType feeType)
        {
            if (ModelState.IsValid)
            {
                db.FeeTypes.Add(feeType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(feeType);
        }

        // GET: FeeTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeeType feeType = await db.FeeTypes.FindAsync(id);
            if (feeType == null)
            {
                return HttpNotFound();
            }
            return View(feeType);
        }

        // POST: FeeTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,FeeName,Description")] FeeType feeType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feeType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(feeType);
        }

        // GET: FeeTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeeType feeType = await db.FeeTypes.FindAsync(id);
            if (feeType == null)
            {
                return HttpNotFound();
            }
            return View(feeType);
        }

        // POST: FeeTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FeeType feeType = await db.FeeTypes.FindAsync(id);
            db.FeeTypes.Remove(feeType);
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
