using StAugustine.BusinessLogic;
using StAugustine.Models;
using StAugustine.ViewModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StAugustine.Controllers
{
    public class ResultsController : Controller
    {
        private readonly ResultCommandManager ResultCommand = new ResultCommandManager();
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        public ResultsController()
        {
        }

        //public ResultsController(IResultCommandManager resultCommand, ApplicationDbContext db)
        //{
        //    ResultCommand = resultCommand;
        //    //_db = db;
        //}


        // GET: Results
        public async Task<ActionResult> Index()
        {
            return View(await _db.Results.ToListAsync());
        }

        // GET: Results/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Results/Create
        public ActionResult Create()
        {
            ViewBag.SubjectCode = new SelectList(_db.Subjects, "CourseName", "CourseName");
            //ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FullName");
            ViewBag.SessionName = new SelectList(_db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(_db.Classes, "FullClassName", "FullClassName");
            return View();
        }

        // POST: Results/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ResultViewModel model)
        {
            if (ModelState.IsValid)
            {
                var student = _db.AssignedClasses.Where(x => x.ClassName.Equals(model.ClassName.Trim())
                                                            && x.TermName.Contains(model.TermName.ToString())
                                                            && x.SessionName.Equals(model.SessionName)).ToList();

                foreach (var listStudent in student)
                {
                    string studentNumber = listStudent.StudentId;
                    var CA = _db.Results.Where(x => x.ClassName.Equals(model.ClassName)
                                                                 && x.Term.Contains(model.TermName.ToString())
                                                                 && x.SessionName.Equals(model.SessionName)
                                                                 && x.SubjectName.Equals(model.SubjectCode)
                                                                 && x.StudentId.Equals(studentNumber));
                    var countFromDb = CA.Count();
                    if (countFromDb >= 1)
                    {
                        return View("Error2");
                    }
                    else
                    {
                        //var result = new Result(studentNumber, model.ClassName, model.TermName.ToString(),
                        //    model.SessionName, model.SubjectCode);
                        var result = new Result
                        {
                            StudentId = studentNumber,
                            ClassName = model.ClassName,
                            Term = model.TermName.ToString(),
                            SubjectName = model.SubjectCode,
                            SessionName = model.SessionName,
                            ClassAverage = ResultCommand.CalculateClassAverage(model.ClassName, model.TermName.ToString(), model.SessionName, model.SubjectCode),
                            Average = ResultCommand.CalculateAverage(studentNumber, model.ClassName, model.TermName.ToString(), model.SessionName),
                            SubjectPosition = ResultCommand.FindSubjectPosition(studentNumber, model.SubjectCode, model.ClassName, model.TermName.ToString(), model.SessionName),
                            AggretateScore = ResultCommand.TotalScorePerStudent(studentNumber, model.ClassName, model.TermName.ToString(), model.SessionName),
                            TotalQualityPoint = ResultCommand.TotalQualityPoint(studentNumber, model.ClassName, model.TermName.ToString(), model.SessionName),
                            TotalCreditUnit = ResultCommand.TotalcreditUnit(model.ClassName)

                        };

                        _db.Results.Add(result);
                    }
                }
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Results/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = await _db.Results.FindAsync(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            var myStudent = new ResultViewModel()
            {
                ResultId = result.ResultId
            };
            ViewBag.SubjectCode = new SelectList(_db.Subjects, "CourseCode", "CourseName");
            //ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(_db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(_db.Classes, "FullClassName", "FullClassName");
            return View(myStudent);
        }

        // POST: Results/Edit/5
        [HttpPost]
        public ActionResult Edit(ResultViewModel model)
        {
            if (ModelState.IsValid)
            {

                //var result = new Result
                //{
                //    StudentId = studentNumber,
                //    ClassName = model.ClassName,
                //    Term = model.TermName.ToString(),
                //    SubjectName = model.SubjectCode,
                //    SessionName = model.SessionName,
                //    NoOfStudentPerClass = ResultCommand.NumberOfStudentPerClass(model.ClassName, model.TermName.ToString(), model.SessionName),
                //    NoOfSubjectOffered = ResultCommand.SubjectOfferedByStudent(model.ClassName),
                //    ClassAverage = ResultCommand.CalculateClassAverage(model.ClassName, model.TermName.ToString(), model.SessionName, model.SubjectCode),
                //    Average = ResultCommand.CalculateAverage(studentNumber, model.ClassName, model.TermName.ToString(), model.SessionName),
                //    SubjectPosition = ResultCommand.FindSubjectPosition(studentNumber, model.SubjectCode, model.ClassName, model.TermName.ToString(), model.SessionName),
                //    AggretateScore = ResultCommand.TotalScorePerStudent(studentNumber, model.ClassName, model.TermName.ToString(), model.SessionName),
                //    AggregatePosition = ResultCommand.FindAggregatePosition(studentNumber, model.ClassName, model.TermName.ToString(), model.SessionName),
                //    TotalQualityPoint = ResultCommand.TotalQualityPoint(studentNumber, model.ClassName, model.TermName.ToString(), model.SessionName),
                //    TotalCreditUnit = ResultCommand.TotalcreditUnit(model.ClassName)

                //};

                //db.Entry(result).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
            // return View();
        }

        // GET: Results/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Results/Delete/5
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
