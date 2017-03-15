using StAugustine.Models;
using StAugustine.ViewModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StAugustine.Controllers
{
    public class ResultSummaryController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: ResultSummary
        public async Task<ActionResult> Index()
        {
            return View(await db.ReportSummarys.ToListAsync());
        }

        // GET: ResultSummary/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ResultSummary/Create
        public ActionResult Create()
        {

            ViewBag.SubjectCode = new SelectList(db.SessionSubjectTotals, "SubjectName", "SubjectName");
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "StudentId");
            ViewBag.SessionName = new SelectList(db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");
            return View();
        }

        // POST: ResultSummary/Create
        [HttpPost]
        public async Task<ActionResult> Create(ResultSummaryViewModel model)
        {

            if (ModelState.IsValid)
            {
                var student = db.AssignedClasses.Where(x => x.ClassName.Equals(model.ClassName) && x.TermName.Contains("Third")
                                                       && x.SessionName.Equals(model.SessionName)).ToList();
                foreach (var listStudent in student)
                {
                    string studentNumber = listStudent.StudentId;
                    var CA = db.ReportSummarys.Where(x => x.ClassName.Equals(model.ClassName)
                                                               && x.SessionName.Equals(model.SessionName)
                                                               && x.SubjectName.Equals(model.SubjectCode)
                                                               && x.StudentId.Equals(studentNumber));
                    var countFromDb = CA.Count();
                    if (countFromDb >= 1)
                    {
                        return View("Error3");
                    }
                    else
                    {
                        var resultSummary = new ReportSummary(studentNumber, model.ClassName, model.SessionName,
                       model.SubjectCode);

                        db.ReportSummarys.Add(resultSummary);
                    }
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            };
            return View(model);
        }

        // GET: ResultSummary/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = await db.Results.FindAsync(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            var myStudent = new ResultViewModel()
            {
                ResultId = result.ResultId
            };
            ViewBag.SubjectCode = new SelectList(db.Subjects, "CourseCode", "CourseName");
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");

            ResultSummaryViewModel model = new ResultSummaryViewModel();
            model.ReportSummaryId = myStudent.ResultId;
            //model.StudentId = myStudent.StudentId;
            model.ClassName = myStudent.ClassName;
            return View(model);
        }

        // POST: ResultSummary/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(ResultSummaryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resultSummary = await db.ReportSummarys.FindAsync(model.ReportSummaryId);

                if (resultSummary == null)
                {
                    return HttpNotFound();
                }
                var student = db.AssignedClasses.Where(x => x.ClassName.Equals(model.ClassName) && x.TermName.Contains("Third")
                                                  && x.SessionName.Equals(model.SessionName));
                foreach (var listStudent in student)
                {
                    string studentNumber = listStudent.StudentId;
                    resultSummary.UpdateResultSummary(studentNumber, model.ClassName,
                        model.SessionName, model.SubjectCode);
                    db.Entry(resultSummary).State = EntityState.Modified;
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }


        // GET: ResultSummary/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ResultSummary/Delete/5
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
