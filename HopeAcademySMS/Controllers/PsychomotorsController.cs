﻿using SwiftSkool.Models;
using SwiftSkool.ViewModel;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HopeAcademySMS.Controllers
{
    public class PsychomotorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Psychomotors
        public async Task<ActionResult> Index()
        {
            return View(await db.Psychomotors.ToListAsync());
        }

        // GET: Psychomotors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Psychomotor psychomotor = await db.Psychomotors.FindAsync(id);
            if (psychomotor == null)
            {
                return HttpNotFound();
            }
            return View(psychomotor);
        }

        public PartialViewResult AddPsycomotor(string studentId)
        {
            ViewBag.StudentId = studentId;
            ViewBag.SessionName = new SelectList(db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.TermName = new SelectList(db.Terms.AsNoTracking(), "TermName", "TermName");
            ViewBag.ClassName = new SelectList(db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            return PartialView("PartialPsycomotor");
        }

        // GET: Psychomotors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Psychomotors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PsycomotorViewModel model)
        {
            if (ModelState.IsValid)
            {
                Psychomotor psychomotor = new Psychomotor()
                {
                    StudentId = model.StudentId,
                    TermName = model.TermName.ToString(),
                    SessionName = model.SessionName,
                    ClassName = model.ClassName,
                    Sports = model.Sports.ToString(),
                    ExtraCurricularActivity = model.ExtraCurricularActivity.ToString(),
                    Assignment = model.Assignment.ToString(),
                    HelpingOthers = model.HelpingOthers.ToString(),
                    ManualDuty = model.ManualDuty.ToString(),
                    LevelOfCommitment = model.LevelOfCommitment.ToString()
                };
                db.Psychomotors.Add(psychomotor);
                await db.SaveChangesAsync();
                return RedirectToAction("FormTeacher", "Classes");
            }

            return View(model);
        }

        // GET: Psychomotors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Psychomotor psychomotor = await db.Psychomotors.FindAsync(id);
            if (psychomotor == null)
            {
                return HttpNotFound();
            }
            return View(psychomotor);
        }

        // POST: Psychomotors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,StudentId,TermName,SessionName,ClassName,Sports,ExtraCurricularActivity,Assignment,HelpingOthers,ManualDuty,LevelOfCommitment")] Psychomotor psychomotor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(psychomotor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(psychomotor);
        }

        // GET: Psychomotors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Psychomotor psychomotor = await db.Psychomotors.FindAsync(id);
            if (psychomotor == null)
            {
                return HttpNotFound();
            }
            return View(psychomotor);
        }

        // POST: Psychomotors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Psychomotor psychomotor = await db.Psychomotors.FindAsync(id);
            if (psychomotor != null) db.Psychomotors.Remove(psychomotor);
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
