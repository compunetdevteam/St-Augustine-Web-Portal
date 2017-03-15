using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using PagedList;
using StAugustine.BusinessLogic;
using StAugustine.Models;
using StAugustine.ViewModel;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

//using Excel = Microsoft.Office.Interop.Excel;

namespace StAugustine.Controllers
{
    public class StudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ResultCommandManager _resultCommand = new ResultCommandManager();
        // GET: Students
        public ActionResult Index(string sortOrder, string currentFilter, string search, int? page)
        {
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

            var studentList = from s in db.Students select s;
            if (User.IsInRole("Guardian"))
            {
                string name = User.Identity.GetUserName();
                var user = db.Users.Where(c => c.UserName.Equals(name)).Select(s => s.Email).FirstOrDefault();

                var studentName = studentList.Where(x => x.GuardianEmail.Equals(user)).ToList();

                return View(studentName);
            }
            else
            {
                if (!String.IsNullOrEmpty(search))
                {
                    studentList = studentList.Where(s => s.LastName.ToUpper().Contains(search.ToUpper())
                                                         || s.FirstName.ToUpper().Contains(search.ToUpper())
                                                         || s.StudentId.ToUpper().Contains(search.ToUpper()));

                }
                switch (sortOrder)
                {
                    case "name_desc":
                        studentList = studentList.OrderByDescending(s => s.LastName);
                        break;
                    case "Date":
                        studentList = studentList.OrderBy(s => s.FirstName);
                        break;
                    default:
                        studentList = studentList.OrderBy(s => s.LastName);
                        break;
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(studentList.ToPagedList(pageNumber, pageSize));
                //return View(studentList.ToList());
            }
        }

        public ActionResult Dashboard()
        {
            int totalMaleStudent = db.Students.Count(s => s.Gender.Equals("Male"));
            int totalFemaleStudent = db.Students.Count(s => s.Gender.Equals("Female"));
            int totalStudent = db.Students.Count();
            int totalStaff = db.Staffs.Count();

            double val1 = totalMaleStudent * 100;
            double val2 = totalFemaleStudent * 100;

            double boysPercentage = Math.Round(val1 / totalStudent, 2);
            double femalePercentage = Math.Round(val2 / totalStudent, 2);

            ViewBag.MaleStudent = totalMaleStudent;
            ViewBag.Femalestudent = totalFemaleStudent;
            ViewBag.TotalStudent = totalStudent;
            ViewBag.TotalStaff = totalStaff;
            ViewBag.BoysPercentage = boysPercentage;
            ViewBag.FemalePercentage = femalePercentage;

            return View();
        }
        // GET: Students/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Student student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }


        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.GuardianId = new SelectList(db.Guardians, "GuardianEmail", "FullName");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var student = new Student(model.StudentId, model.GuardianId, model.FirstName, model.MiddleName, model.LastName,
                                            model.Gender.ToString(), model.DateOfBirth, model.AdmissionDate,
                                            model.StudentPassport);

                db.Students.Add(student);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            };
            ViewBag.GuardianId = new SelectList(db.Guardians, "GuardianEmail", "FullName");
            return View(model);
        }

        // GET: Students/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            var myStudent = new StudentViewModel()
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                MiddleName = student.MiddleName,
                LastName = student.LastName,
                DateOfBirth = student.DateOfBirth,
                AdmissionDate = student.AdmissionDate
            };
            ViewBag.GuardianId = new SelectList(db.Guardians, "GuardianEmail", "FullName");
            return View(myStudent);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var student = new Student(model.StudentId, model.GuardianId, model.FirstName, model.MiddleName, model.LastName,
                                            model.Gender.ToString(), model.DateOfBirth, model.AdmissionDate,
                                            model.StudentPassport);
                db.Entry(student).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.GuardianId = new SelectList(db.Guardians, "GuardianEmail", "FullName");
            return View(model);
        }

        // GET: Students/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Student student = await db.Students.FindAsync(id);
            db.Students.Remove(student);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult UploadStudent()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> UploadStudent(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please Select a excel file <br/>";
                return View("Index");
            }
            else
            {
                HttpPostedFileBase file = Request.Files["excelfile"];
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                    // Read data from excel file
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;

                        for (int row = 2; row <= noOfRow; row++)
                        {
                            string studentId = workSheet.Cells[row, 1].Value.ToString();
                            string firstName = workSheet.Cells[row, 2].Value.ToString();
                            string middleName = workSheet.Cells[row, 3].Value.ToString();
                            string lastName = workSheet.Cells[row, 4].Value.ToString();
                            string gender = workSheet.Cells[row, 5].Value.ToString();
                            DateTime dateofBirth = DateTime.Parse(workSheet.Cells[row, 6].Value.ToString());
                            DateTime admissionDate = DateTime.Parse(workSheet.Cells[row, 7].Value.ToString());
                            string guardianId = workSheet.Cells[row, 8].Value.ToString();

                            var student = new Student(studentId.Trim(), guardianId.Trim(), firstName.Trim(),
                                middleName.Trim(), lastName.Trim(),
                                gender.Trim(), dateofBirth, admissionDate);

                            db.Students.Add(student);
                        }
                        await db.SaveChangesAsync();
                    }

                    return RedirectToAction("Index", "Students");
                }

                else
                {
                    ViewBag.Error = "File type is Incorrect <br/>";
                    return View("Index");
                }
            }


        }

        public async Task<ActionResult> RenderImage(string studentId)
        {
            Student student = await db.Students.FindAsync(studentId);

            byte[] photoBack = student.StudentPassport;

            return File(photoBack, "image/png");
        }

        public PartialViewResult GuardianInfo(string studentNumber)
        {
            var GuardianInfoes = db.Guardians.Include(p => p.Student).Where(s => s.GuardianEmail.Contains(studentNumber));
            return PartialView(GuardianInfoes);
        }

        public async Task<ActionResult> PrintSecondTerm(string id, string term, string sessionName)
        {
            ReportViewModel reportModel = new ReportViewModel();
            reportModel.Student = await db.Students.FindAsync(id);

            //reportModel.Maths = db.ContinuousAssessments.Include(p => p.Student)
            //                                        .Where(s => s.StudentId.Contains(id)
            //                                        && s.TermName.Contains(term)
            //                                        && s.SessionName.Contains(sessionName)
            //                                        && s.SubjectCategory.Equals("Mathematics")).ToList();

            //reportModel.English = db.ContinuousAssessments.Include(p => p.Student)
            //                                       .Where(s => s.StudentId.Contains(id)
            //                                       && s.TermName.Contains(term)
            //                                       && s.SessionName.Contains(sessionName)
            //                                       && s.SubjectCategory.Equals("English")).ToList();

            reportModel.ContinuousAssessments = db.ContinuousAssessments.Include(p => p.Student)
                                                    .Where(s => s.StudentId.Contains(id)
                                                    && s.TermName.Contains(term)
                                                    && s.SessionName.Contains(sessionName))
                                                    .OrderBy(y => y.SubjectCode).ToList();

            reportModel.Results = db.Results.Where(s => s.StudentId.Contains(id)
                                                                 && s.Term.Contains(term)
                                                                 && s.SessionName.Contains(sessionName)).ToList();

            //var className = db.AssignedClasses.Where(x => x.StudentId.Equals(id))
            //                                    .Select(y => y.ClassName)
            //                                    .FirstOrDefault();

            var className = "JSS1 A";

            reportModel.NoOfStudentPerClass = db.AssignedClasses.Count(x => x.ClassName.Contains(className) &&
                                                               x.TermName.Equals(term) &&
                                                               x.SessionName.Equals(sessionName));
            reportModel.AggregatePosition = _resultCommand.FindAggregatePosition(id, className, term, sessionName);
            reportModel.Average = db.Results.Where(s => s.StudentId.Contains(id)
                                                        && s.Term.Contains(term)
                                                        && s.SessionName.Contains(sessionName))
                                                        .Select(c => c.Average).FirstOrDefault();
            reportModel.TotalScore = db.Results.Where(s => s.StudentId.Contains(id)
                                                        && s.Term.Contains(term)
                                                        && s.SessionName.Contains(sessionName))
                                                        .Select(c => c.AggretateScore).FirstOrDefault();
            var myOtherSkills = db.Psychomotors.Where(s => s.StudentId.Contains(id)
                                                          && s.TermName.Contains(term)
                                                          && s.SessionName.Contains(sessionName)
                                                          && s.ClassName.Equals(className))
                                                         .Select(c => c.Id).FirstOrDefault();

            reportModel.GPA = db.Results.Where(s => s.StudentId.Contains(id)
                                                       && s.Term.Contains(term)
                                                       && s.SessionName.Contains(sessionName))
                                                        .Select(c => c.GPA).FirstOrDefault();
            reportModel.AggregateScore = _resultCommand.TotalScorePerStudent(id, className, term, sessionName);
            reportModel.TotalQualityPoint = _resultCommand.TotalQualityPoint(id, className, term, sessionName);
            reportModel.TotalCreditUnit = _resultCommand.TotalcreditUnit(className);
            reportModel.GradePointAverage = Math.Round((reportModel.TotalQualityPoint / reportModel.TotalCreditUnit), 2);

            // reportModel.OtherSkills = await db.Psychomotors.FindAsync(myOtherSkills);
            ;
            //ViewBag.Class = 
            ViewBag.Term = term.ToUpper();
            ViewBag.Session = sessionName;
            ViewBag.ClassName = className;


            return View(reportModel);

            // return new ViewAsPdf("PrintSecondTerm", reportModel);

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Student student = await db.Students.FindAsync(id);
            //if (student == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(student);
        }

        public ActionResult PrintSummaryReport(string id, string sessionName)
        {
            SummaryReportViewModel summary = new SummaryReportViewModel();
            summary.Results = db.Results.Where(s => s.StudentId.Contains(id)
                                    && s.SessionName.Contains(sessionName)).ToList();
            summary.ReportSummaries = db.ReportSummarys.Where(s => s.StudentId.Equals(id)
                                                && s.SessionName.Equals(sessionName)).ToList();
            //foreach (var item in studentResults.Where(c => c.))
            //{

            //}
            return View(summary);
        }

        public PartialViewResult ResultInfo(string studentNumber)
        {
            var resultInfoes = db.ContinuousAssessments.Include(p => p.Student).Where(s => s.StudentId.Contains(studentNumber));
            return PartialView(resultInfoes);
        }
        //public PartialViewResult ResultRemplate(string studentNumber, string term, string sessionName)
        //{
        //    var GuardianInfoes = db.ContinuousAssessments.Include(p => p.Student).Where(s => s.StudentId.Contains(studentNumber)
        //                                            && s.TermName.Contains(term) && s.SessionName.Contains(sessionName));
        //    return PartialView(GuardianInfoes);
        //}

        //public PartialViewResult RenderRemplate(string studentNumber, string term, string sessionName, string subjectcode)
        //{
        //    var myResult = db.Results.Where(s => s.StudentId.Contains(studentNumber)
        //                                            && s.Term.Contains(term)
        //                                            && s.SessionName.Contains(sessionName)
        //                                            && s.SubjectName.Contains(subjectcode));
        //    return PartialView(myResult);
        //}


        //public ActionResult Pdf()
        //{
        //    var pdf = db.Students.ToList();
        //    return new PdfResult(pdf, "Pdf");
        //}
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
