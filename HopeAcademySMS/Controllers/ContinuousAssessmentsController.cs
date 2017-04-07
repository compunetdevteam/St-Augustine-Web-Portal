using HopeAcademySMS.Services;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using PagedList;
using StAugustine.Models;
using StAugustine.ViewModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

//using Excel = Microsoft.Office.Interop.Excel;

namespace StAugustine.Controllers
{
    public class ContinuousAssessmentsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: ContinuousAssessments
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string search, int? page,
            string SubjectCode, string ClassName, string TermName, string SessionName)
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
            var assignedList = from s in _db.ContinuousAssessments select s;
            if (User.IsInRole("Teacher"))
            {
                string name = User.Identity.GetUserName();
                //var user = db.Guardians.Where(c => c.UserName.Equals(name)).Select(s => s.Email).FirstOrDefault();

                assignedList = assignedList.AsNoTracking().Where(x => x.StaffName.Equals(name));

                //return View(subjectName);
            }
            else
            {
                if (!String.IsNullOrEmpty(search))
                {
                    assignedList = assignedList.AsNoTracking().Where(s => s.StudentId.ToUpper().Contains(search.ToUpper())
                                                                 || s.ClassName.ToUpper().Contains(search.ToUpper())
                                                                 || s.TermName.ToUpper().Contains(search.ToUpper()));

                }
                else if (!String.IsNullOrEmpty(SubjectCode) && (!String.IsNullOrEmpty(ClassName)
                       && !String.IsNullOrEmpty(SessionName) && !String.IsNullOrEmpty(TermName)))
                {
                    assignedList = assignedList.AsNoTracking().Where(s => s.SubjectCode.ToUpper().Equals(SubjectCode.ToUpper())
                                               && s.ClassName.ToUpper().Equals(ClassName.ToUpper())
                                               && s.TermName.ToUpper().Equals(TermName.ToUpper())
                                               && s.SessionName.ToUpper().Equals(SessionName))
                                               .OrderBy(c => c.StudentId);
                    int myCount = await assignedList.CountAsync();
                    if (myCount != 0)
                    {
                        count = myCount;
                    }
                }
                else if (!String.IsNullOrEmpty(SubjectCode) || (!String.IsNullOrEmpty(ClassName)
                                                                || !String.IsNullOrEmpty(SessionName) ||
                                                                !String.IsNullOrEmpty(TermName)))
                {
                    assignedList = assignedList.AsNoTracking().Where(s => s.SubjectCode.ToUpper().Equals(SubjectCode.ToUpper())
                                                           || s.ClassName.ToUpper().Equals(ClassName.ToUpper())
                                                           || s.TermName.ToUpper().Equals(TermName.ToUpper())
                                                           || s.SessionName.ToUpper().Equals(SessionName));
                }

            }


            switch (sortOrder)
            {
                case "name_desc":
                    assignedList = assignedList.AsNoTracking().OrderByDescending(s => s.StudentId);
                    break;
                case "Date":
                    assignedList = assignedList.AsNoTracking().OrderBy(s => s.SessionName);
                    break;
                default:
                    assignedList = assignedList.AsNoTracking().OrderBy(s => s.ClassName);
                    break;
            }
            // ViewBag.SubjectCode = new SelectList(db.Subjects, "CourseName", "CourseName");
            ViewBag.TermName = new SelectList(_db.Terms.AsNoTracking(), "TermName", "TermName");
            ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");

            if (User.IsInRole("Teacher"))
            {
                string name = User.Identity.GetUserName();
                var subjectList = _db.AssignSubjectTeachers.AsNoTracking().Where(x => x.StaffName.Equals(name));
                ViewBag.SubjectCode = new SelectList(subjectList.AsNoTracking(), "SubjectName", "SubjectName");
                ViewBag.ClassName = new SelectList(subjectList.AsNoTracking(), "ClassName", "ClassName");
            }
            else
            {
                ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
                ViewBag.SubjectCode = new SelectList(_db.Subjects.AsNoTracking(), "CourseName", "CourseName");
            }
            int pageSize = count;
            int pageNumber = (page ?? 1);
            return View(assignedList.ToPagedList(pageNumber, pageSize));
            //return View(await db.ContinuousAssessments.ToListAsync());
        }


        // GET: ContinuousAssessments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContinuousAssessment continuousAssessment = await _db.ContinuousAssessments.FindAsync(id);
            if (continuousAssessment == null)
            {
                return HttpNotFound();
            }
            return View(continuousAssessment);
        }

        // GET: ContinuousAssessments/Create
        public ActionResult Create()
        {
            ViewBag.StudentId = new SelectList(_db.Students.AsNoTracking(), "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.TermName = new SelectList(_db.Terms.AsNoTracking(), "TermName", "TermName");
            ViewBag.StaffName = User.Identity.GetUserName();
            if (User.IsInRole("Teacher"))
            {
                string name = User.Identity.GetUserName();
                var assignedList = _db.AssignSubjectTeachers.Where(x => x.StaffName.Equals(name));
                ViewBag.SubjectCode = new SelectList(assignedList.AsNoTracking(), "SubjectName", "SubjectName");
            }
            else
            {
                ViewBag.SubjectCode = new SelectList(_db.Subjects.AsNoTracking(), "CourseName", "CourseName");
            }
            return View();
        }

        // POST: ContinuousAssessments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ContinuousAssesmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var mysubjectCategory = db.Subjects.Where(x => x.CourseCode.Equals(model.SubjectCode))
                //                        .Select(c => c.CategoriesId).FirstOrDefault();

                //var subjectName = db.Subjects.Where(x => x.CourseCode.Equals(model.SubjectCode))
                //                        .Select(c => c.CourseName).FirstOrDefault();
                //var student = db.AssignedClasses.Where(x => x.ClassName.Equals(model.ClassName)
                //                                               && x.TermName.Contains(model.TermName.ToString())
                //                                               && x.SessionName.Equals(model.SessionName));
                var CA = _db.ContinuousAssessments.AsNoTracking().Where(x => x.ClassName.Equals(model.ClassName)
                                                                  && x.TermName.Contains(model.TermName.ToString())
                                                                  && x.SessionName.Equals(model.SessionName)
                                                                  && x.StudentId.Equals(model.StudentId)
                                                                  && x.SubjectCode.Equals(model.SubjectCode));
                var countFromDb = await CA.CountAsync();
                if (countFromDb >= 1)
                {
                    ViewBag.SubjectCode = new SelectList(_db.Subjects.AsNoTracking(), "CourseName", "CourseName");
                    ViewBag.StudentId = new SelectList(_db.Students.AsNoTracking(), "StudentID", "FullName");
                    ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");
                    ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
                    ViewBag.TermName = new SelectList(_db.Terms.AsNoTracking(), "TermName", "TermName");
                    TempData["UserMessage"] = "Record Already Exist in Database.";
                    TempData["Title"] = "Error.";
                    return View(model);
                }
                else
                {
                    ContinuousAssessment myContinuousAssessment = new ContinuousAssessment()
                    {
                        StudentId = model.StudentId,
                        SubjectCode = model.SubjectCode,
                        ProjectScore = model.ProjectScore,
                        Assignment = model.Assignment,
                        Test = model.Test,
                        ExamScore = model.ExamScore,
                        TermName = model.TermName,
                        SessionName = model.SessionName,
                        ClassName = model.ClassName,
                        StaffName = model.StaffName
                        //SubjectCategory = mysubjectCategory
                    };
                    _db.ContinuousAssessments.Add(myContinuousAssessment);
                }
                await _db.SaveChangesAsync();
                TempData["UserMessage"] = "Continuous Assessment Added Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            ViewBag.SubjectCode = new SelectList(_db.Subjects.AsNoTracking(), "CourseName", "CourseName");
            ViewBag.StudentId = new SelectList(_db.Students.AsNoTracking(), "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.TermName = new SelectList(_db.Terms.AsNoTracking(), "TermName", "TermName");
            ViewBag.StaffName = User.Identity.GetUserName();

            return View(model);
        }

        // GET: ContinuousAssessments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContinuousAssessment continuousAssessment = await _db.ContinuousAssessments.FindAsync(id);
            if (continuousAssessment == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectCode = new SelectList(_db.Subjects.AsNoTracking(), "CourseName", "CourseName");
            ViewBag.StudentId = new SelectList(_db.Students.AsNoTracking(), "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.TermName = new SelectList(_db.Terms.AsNoTracking(), "TermName", "TermName");
            ViewBag.StaffName = User.Identity.GetUserName();
            ContinuousAssesmentViewModel model = new ContinuousAssesmentViewModel()
            {
                ContinuousAssessmentId = continuousAssessment.ContinuousAssessmentId,
                StudentId = continuousAssessment.StudentId,
                ProjectScore = continuousAssessment.ProjectScore,
                Assignment = continuousAssessment.Assignment,
                Test = continuousAssessment.Test,
                ExamScore = continuousAssessment.ExamScore
            };
            return View(model);
        }

        // POST: ContinuousAssessments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ContinuousAssesmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                ContinuousAssessment myContinuousAssessment = new ContinuousAssessment()
                {
                    ContinuousAssessmentId = model.ContinuousAssessmentId,
                    StudentId = model.StudentId,
                    SubjectCode = model.SubjectCode,
                    ProjectScore = model.ProjectScore,
                    Assignment = model.Assignment,
                    Test = model.Test,
                    ExamScore = model.ExamScore,
                    TermName = model.TermName.ToString(),
                    SessionName = model.SessionName,
                    ClassName = model.ClassName,
                    StaffName = model.StaffName
                    //SubjectCategory = mysubjectCategory
                };
                // db.ContinuousAssessments.Add(myContinuousAssessment);
                _db.Entry(myContinuousAssessment).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                TempData["UserMessage"] = "Continuous Assessment Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            ViewBag.SubjectCode = new SelectList(_db.Subjects.AsNoTracking(), "CourseName", "CourseName");
            ViewBag.StudentId = new SelectList(_db.Students.AsNoTracking(), "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
            ViewBag.TermName = new SelectList(_db.Terms.AsNoTracking(), "TermName", "TermName");
            ViewBag.StaffName = User.Identity.GetUserName();
            return View(model);
        }

        // GET: ContinuousAssessments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContinuousAssessment continuousAssessment = await _db.ContinuousAssessments.FindAsync(id);
            if (continuousAssessment == null)
            {
                return HttpNotFound();
            }
            return View(continuousAssessment);
        }

        // POST: ContinuousAssessments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ContinuousAssessment continuousAssessment = await _db.ContinuousAssessments.FindAsync(id);
            if (continuousAssessment != null) _db.ContinuousAssessments.Remove(continuousAssessment);
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

        public ActionResult UploadResult()
        {
            //ViewBag.CourseName = new SelectList(db.Courses, "CourseName", "CourseName");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadResult(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please Select a excel file <br/>";
                return View();
            }
            else
            {
                HttpPostedFileBase file = Request.Files["excelfile"];
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string lastrecord = "";
                    int recordCount = 0;
                    string message = "";
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    // Read data from excel file
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        foreach (var sheet in currentSheet)
                        {
                            ExcelValidation myExcel = new ExcelValidation();
                            //var workSheet = currentSheet.First();
                            var noOfCol = sheet.Dimension.End.Column;
                            var noOfRow = sheet.Dimension.End.Row;
                            int requiredField = 10;

                            string validCheck = myExcel.ValidateExcel(noOfRow, sheet, requiredField);
                            if (!validCheck.Equals("Success"))
                            {
                                //string row = "";
                                //string column = "";
                                string[] ssizes = validCheck.Split(' ');
                                string[] myArray = new string[2];
                                for (int i = 0; i < ssizes.Length; i++)
                                {
                                    myArray[i] = ssizes[i];
                                    // myArray[i] = ssizes[];
                                }
                                string lineError = $"Please Check sheet {sheet}, Line/Row number {myArray[0]}  and column {myArray[1]} is not rightly formatted, Please Check for anomalies ";
                                //ViewBag.LineError = lineError;
                                TempData["UserMessage"] = lineError;
                                TempData["Title"] = "Error.";
                                return View();
                            }

                            for (int row = 2; row <= noOfRow; row++)
                            {
                                string studentId = sheet.Cells[row, 1].Value.ToString().ToUpper().Trim();
                                string subjectValue = sheet.Cells[row, 2].Value.ToString().ToUpper().Trim();
                                string termName = sheet.Cells[row, 7].Value.ToString().Trim().ToUpper();
                                string className = sheet.Cells[row, 10].Value.ToString().Trim().ToUpper();
                                string sessionName = sheet.Cells[row, 8].Value.ToString().Trim();

                                //var mysubjectCategory = db.Subjects.Where(x => x.CourseCode.Equals(subjectValue))
                                //    .Select(c => c.CategoriesId).FirstOrDefault();
                                var subjectName = _db.Subjects.Where(x => x.CourseCode.Equals(subjectValue))
                                    .Select(c => c.CourseName).FirstOrDefault();

                                var CA = _db.ContinuousAssessments.Where(x => x.ClassName.Equals(className)
                                                                             && x.TermName.Contains(termName)
                                                                             && x.SessionName.Equals(sessionName)
                                                                             && x.StudentId.Equals(studentId)
                                                                             && x.SubjectCode.Equals(subjectName));
                                var countFromDb = await CA.CountAsync();
                                if (countFromDb >= 1)
                                {
                                    return View("Error2");
                                }
                                else
                                {
                                    var mycontinuousAssessment = new ContinuousAssessment
                                    {
                                        StudentId = studentId,
                                        SubjectCode = subjectName,
                                        ProjectScore = double.Parse(sheet.Cells[row, 3].Value.ToString().Trim()),
                                        Assignment = double.Parse(sheet.Cells[row, 4].Value.ToString().Trim()),
                                        Test = double.Parse(sheet.Cells[row, 5].Value.ToString().Trim()),
                                        ExamScore = double.Parse(sheet.Cells[row, 6].Value.ToString().Trim()),
                                        TermName = termName,
                                        SessionName = sessionName,
                                        StaffName = sheet.Cells[row, 9].Value.ToString().Trim().ToUpper(),
                                        ClassName = className,
                                        //SubjectCategory = mysubjectCategory
                                    };
                                    _db.ContinuousAssessments.Add(mycontinuousAssessment);

                                    recordCount++;
                                    lastrecord = $"The last Updated record has the Student Id {studentId} and Subject Name is {subjectName}. Please Confirm!!!";
                                }

                            }
                        }
                    }
                    await _db.SaveChangesAsync();
                    message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                    TempData["UserMessage"] = message;
                    TempData["Title"] = "Success.";
                    ViewBag.TermName = new SelectList(_db.Terms.AsNoTracking(), "TermName", "TermName");
                    ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");

                    if (User.IsInRole("Teacher"))
                    {
                        string name = User.Identity.GetUserName();
                        var subjectList = _db.AssignSubjectTeachers.AsNoTracking().Where(x => x.StaffName.Equals(name));
                        ViewBag.SubjectCode = new SelectList(subjectList.AsNoTracking(), "SubjectName", "SubjectName");
                        ViewBag.ClassName = new SelectList(subjectList.AsNoTracking(), "ClassName", "ClassName");
                    }
                    else
                    {
                        ViewBag.ClassName = new SelectList(_db.Classes.AsNoTracking(), "FullClassName", "FullClassName");
                        ViewBag.SubjectCode = new SelectList(_db.Subjects.AsNoTracking(), "CourseName", "CourseName");
                    }
                    return View();
                }
                else
                {
                    ViewBag.Error = "File type is Incorrect <br/>";
                    return View();
                }
            }

        }
    }
}

