using PagedList;
using SwiftSkool.Models;
using SwiftSkool.ViewModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkool.Controllers
{
    public class GradesController : Controller
    {
        readonly ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Grades
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string search, int? page,
           string ClassName)
        {
            int count = 10;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (search != null)
            {

                page = 1;
            }
            else
            {
                search = currentFilter;
            }
            ViewBag.CurrentFilter = search;
            var assignedList = from s in _db.Grades select s;
            //if (!String.IsNullOrEmpty(search))
            //{
            //    assignedList = assignedList.Where(s => s.ClassName.ToUpper().Contains(search.ToUpper()));


            //}
            //else if (!String.IsNullOrEmpty(ClassName))
            //{
            //    assignedList = assignedList.Where(s => s.ClassName.ToUpper().Equals(ClassName.ToUpper()));
            //    int myCount = await assignedList.CountAsync();
            //    if (myCount != 0)
            //    {
            //        count = myCount;
            //    }
            //}
            switch (sortOrder)
            {
                case "name_desc":
                    assignedList = assignedList.OrderByDescending(s => s.GradeName);
                    break;
                case "Date":
                    assignedList = assignedList.OrderBy(s => s.Remark);
                    break;
                default:
                    assignedList = assignedList.OrderBy(s => s.GradeName);
                    break;
            }

            ViewBag.ClassName = new SelectList(_db.SchoolClasses.AsNoTracking(), "ClassCode", "ClassCode");
            int pageSize = count;
            int pageNumber = (page ?? 1);
            return View(assignedList.ToPagedList(pageNumber, pageSize));
            //return View(await db.ContinuousAssessments.ToListAsync());
        }



        // GET: Grades/Create
        public ActionResult Create()
        {
            ViewBag.ClassName = new SelectList(_db.SchoolClasses.AsNoTracking(), "ClassCode", "ClassCode");
            return View();
        }

        // POST: Grades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GradeViewModel model)
        {
            if (ModelState.IsValid)
            {

                var myGrade = await _db.Grades.CountAsync(x => x.GradeName.Trim().Equals(model.GradeName.Trim()));

                if (myGrade >= 1)
                {
                    TempData["UserMessage"] = "Grade Already Exist in Database.";
                    TempData["Title"] = "Error.";
                    ViewBag.ClassName = new SelectList(_db.SchoolClasses.AsNoTracking(), "ClassCode", "ClassCode");
                    return View(model);
                }

                var grade = new Grade
                {
                    GradeName = model.GradeName.Trim().ToUpper(),
                    MinimumValue = model.MinimumValue,
                    MaximumValue = model.MaximumValue,
                    //GradePoint = model.GradePoint,
                    Remark = model.Remark,
                    //ClassName = item
                };
                _db.Grades.Add(grade);

                await _db.SaveChangesAsync();
                TempData["UserMessage"] = "Grade Added Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            //ViewBag.ClassName = new SelectList(_db.SchoolClasses.AsNoTracking(), "ClassCode", "ClassCode");
            return View(model);
        }



        // GET: Grades/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grade grade = await _db.Grades.FindAsync(id);
            if (grade == null)
            {
                return HttpNotFound();
            }
            var myGrade = new GradeViewModel
            {
                GradeName = grade.GradeName,
                GradeId = grade.GradeId,
                MinimumValue = grade.MinimumValue,
                MaximumValue = grade.MaximumValue,
                //GradePoint = grade.GradePoint,
                Remark = grade.Remark
            };
            ViewBag.ClassName = new SelectList(_db.SchoolClasses.AsNoTracking(), "ClassCode", "ClassCode");
            return View(myGrade);
        }

        // POST: Grades/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(GradeViewModel model)
        {
            if (ModelState.IsValid)
            {
                //foreach (var item in model.ClassName)
                //{
                var grade = new Grade()
                {

                    GradeId = model.GradeId,
                    GradeName = model.GradeName.ToString(),
                    MinimumValue = model.MinimumValue,
                    MaximumValue = model.MaximumValue,
                    //GradePoint = model.GradePoint,
                    Remark = model.Remark,
                    //ClassName = item
                };
                _db.Entry(grade).State = EntityState.Modified;
                //}
                await _db.SaveChangesAsync();
                TempData["UserMessage"] = "Grade Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            ViewBag.ClassName = new SelectList(_db.SchoolClasses.AsNoTracking(), "ClassCode", "ClassCode");
            return View(model);
        }



        // GET: Classes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grade @class = await _db.Grades.FindAsync(id);
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
            Grade grade = await _db.Grades.FindAsync(id);
            if (grade != null) _db.Grades.Remove(grade);
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
