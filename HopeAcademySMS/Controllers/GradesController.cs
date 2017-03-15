using StAugustine.Models;
using StAugustine.ViewModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StAugustine.Controllers
{
    public class GradesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Grades
        public async Task<ActionResult> Index()
        {
            return View(await db.Grades.ToListAsync());
        }

        // GET: Grades/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Grades/Create
        public ActionResult Create()
        {
            ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");
            return View();
        }

        // POST: Grades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GradeViewModel model)
        {
            if (ModelState.IsValid)
            {

                var myGrade = db.Grades.Where(x => x.GradeName.Equals(model.GradeName.ToString()));
                var countFromDb = myGrade.Count();
                if (countFromDb >= 1)
                {
                    return View("Error2");
                }
                else
                {
                    var grade = new Grade
                    {
                        GradeName = model.GradeName.ToString(),
                        MinimumValue = model.MinimumValue,
                        MaximumValue = model.MaximumValue,
                        GradePoint = model.GradePoint,
                        Remark = model.Remark,
                        PrincipalRemark = model.PrincipalRemark
                    };
                    db.Grades.Add(grade);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }



        // GET: Grades/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grade grade = await db.Grades.FindAsync(id);
            if (grade == null)
            {
                return HttpNotFound();
            }
            var myGrade = new GradeViewModel
            {
                GradeId = grade.GradeId,
                MinimumValue = grade.MinimumValue,
                MaximumValue = grade.MaximumValue,
                GradePoint = grade.GradePoint,
                Remark = grade.Remark,
                PrincipalRemark = grade.PrincipalRemark
            };

            return View(myGrade);
        }

        // POST: Grades/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(GradeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var grade = new Grade()
                {
                    GradeId = model.GradeId,
                    GradeName = model.GradeName.ToString(),
                    MinimumValue = model.MinimumValue,
                    MaximumValue = model.MaximumValue,
                    GradePoint = model.GradePoint,
                    Remark = model.Remark,
                    PrincipalRemark = model.PrincipalRemark
                };
                db.Entry(grade).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Grades/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Grades/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
