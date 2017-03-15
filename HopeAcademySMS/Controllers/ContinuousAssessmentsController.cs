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
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ContinuousAssessments
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
            var assignedList = from s in db.ContinuousAssessments select s;
            if (!String.IsNullOrEmpty(search))
            {
                assignedList = assignedList.Where(s => s.StudentId.ToUpper().Contains(search.ToUpper())
                                                     || s.ClassName.ToUpper().Contains(search.ToUpper())
                                                     || s.TermName.ToUpper().Contains(search.ToUpper()));

            }
            switch (sortOrder)
            {
                case "name_desc":
                    assignedList = assignedList.OrderByDescending(s => s.StudentId);
                    break;
                case "Date":
                    assignedList = assignedList.OrderBy(s => s.SessionName);
                    break;
                default:
                    assignedList = assignedList.OrderBy(s => s.ClassName);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.SubjectCode = new SelectList(db.Subjects, "CourseName", "CourseName");
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");
            return View(assignedList.ToPagedList(pageNumber, pageSize));
            //return View(await db.ContinuousAssessments.ToListAsync());
        }



        [HttpPost]
        public ActionResult CAQuery(string SubjectCode, string ClassName, string term,
                                        string SessionName, int? page, string sortOrder, string currentFilter)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (SubjectCode != null && ClassName != null && term != null && SessionName != null)
            {
                page = 1;
            }
            else
            {
                //search = currentFilter;
            }
            //.CurrentFilter = search;
            var caList = from s in db.ContinuousAssessments select s;

            if (!String.IsNullOrEmpty(SubjectCode) && (!String.IsNullOrEmpty(ClassName)
                    && !String.IsNullOrEmpty(SessionName) && !String.IsNullOrEmpty(term)))
            {
                caList = caList.Where(s => s.SubjectCode.ToUpper().Equals(SubjectCode.ToUpper())
                                           && s.ClassName.ToUpper().Equals(ClassName.ToUpper())
                                           && s.TermName.ToUpper().Equals(term.ToUpper())
                                           && s.SessionName.ToUpper().Equals(SessionName))
                                           .OrderBy(c => c.StudentId);

            }
            else
            {
                caList = caList.Where(s => s.SubjectCode.ToUpper().Equals(SubjectCode.ToUpper())
                                          || s.ClassName.ToUpper().Equals(ClassName.ToUpper())
                                          || s.TermName.ToUpper().Equals(term.ToUpper())
                                          || s.SessionName.ToUpper().Equals(SessionName));
            }

            var pageCount = caList.Count();
            switch (sortOrder)
            {
                case "name_desc":
                    caList = caList.OrderByDescending(s => s.Total);
                    break;
                case "Date":
                    caList = caList.OrderBy(s => s.SessionName);
                    break;
                default:
                    caList = caList.OrderBy(s => s.TermName);
                    break;
            }
            int pageSize = pageCount;
            int pageNumber = (page ?? 1);
            ViewBag.SubjectCode = new SelectList(db.Subjects, "CourseCode", "CourseName");
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");
            return View(caList.ToPagedList(pageNumber, pageSize));
        }
        // GET: ContinuousAssessments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContinuousAssessment continuousAssessment = await db.ContinuousAssessments.FindAsync(id);
            if (continuousAssessment == null)
            {
                return HttpNotFound();
            }
            return View(continuousAssessment);
        }

        // GET: ContinuousAssessments/Create
        public ActionResult Create()
        {
            ViewBag.SubjectCode = new SelectList(db.Subjects, "CourseName", "CourseName");
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");
            ViewBag.StaffName = User.Identity.GetUserName();
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
                var CA = db.ContinuousAssessments.Where(x => x.ClassName.Equals(model.ClassName)
                                                                  && x.TermName.Contains(model.TermName.ToString())
                                                                  && x.SessionName.Equals(model.SessionName)
                                                                  && x.StudentId.Equals(model.StudentId)
                                                                  && x.SubjectCode.Equals(model.SubjectCode));
                var countFromDb = CA.Count();
                if (countFromDb >= 1)
                {
                    return View("Error2");
                }
                else
                {
                    ContinuousAssessment myContinuousAssessment = new ContinuousAssessment()
                    {
                        StudentId = model.StudentId,
                        SubjectCode = model.SubjectCode,
                        Assignment1 = model.Assignment1,
                        Assignment2 = model.Assignment2,
                        FirstTest = model.FirstTest,
                        SecondTest = model.SecondTest,
                        ExamScore = model.ExamScore,
                        TermName = model.TermName.ToString(),
                        SessionName = model.SessionName,
                        ClassName = model.ClassName,
                        StaffName = model.StaffName
                        //SubjectCategory = mysubjectCategory
                    };
                    db.ContinuousAssessments.Add(myContinuousAssessment);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SubjectCode = new SelectList(db.Subjects, "CourseName", "CourseName");
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");
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
            ContinuousAssessment continuousAssessment = await db.ContinuousAssessments.FindAsync(id);
            if (continuousAssessment == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectCode = new SelectList(db.Subjects, "CourseName", "CourseName");
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");
            ViewBag.StaffName = User.Identity.GetUserName();
            ContinuousAssesmentViewModel model = new ContinuousAssesmentViewModel()
            {
                ContinuousAssessmentId = continuousAssessment.ContinuousAssessmentId,
                StudentId = continuousAssessment.StudentId,
                Assignment1 = continuousAssessment.Assignment1,
                Assignment2 = continuousAssessment.Assignment2,
                FirstTest = continuousAssessment.FirstTest,
                SecondTest = continuousAssessment.SecondTest,
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
                    Assignment1 = model.Assignment1,
                    Assignment2 = model.Assignment2,
                    FirstTest = model.FirstTest,
                    SecondTest = model.SecondTest,
                    ExamScore = model.ExamScore,
                    TermName = model.TermName.ToString(),
                    SessionName = model.SessionName,
                    ClassName = model.ClassName,
                    StaffName = model.StaffName
                    //SubjectCategory = mysubjectCategory
                };
               // db.ContinuousAssessments.Add(myContinuousAssessment);
                db.Entry(myContinuousAssessment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SubjectCode = new SelectList(db.Subjects, "CourseName", "CourseName");
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FullName");
            ViewBag.SessionName = new SelectList(db.Sessions, "SessionName", "SessionName");
            ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");
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
            ContinuousAssessment continuousAssessment = await db.ContinuousAssessments.FindAsync(id);
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
            ContinuousAssessment continuousAssessment = await db.ContinuousAssessments.FindAsync(id);
            db.ContinuousAssessments.Remove(continuousAssessment);
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
                        foreach (var sheet in currentSheet)
                        {
                            //var workSheet = currentSheet.First();
                            var noOfCol = sheet.Dimension.End.Column;
                            var noOfRow = sheet.Dimension.End.Row;

                            for (int row = 2; row <= noOfRow; row++)
                            {
                                string subjectValue = sheet.Cells[row, 2].Value.ToString();
                                string studentId = sheet.Cells[row, 1].Value.ToString();
                                string termName = sheet.Cells[row, 8].Value.ToString();
                                string className = sheet.Cells[row, 11].Value.ToString();
                                string sessionName = sheet.Cells[row, 9].Value.ToString();

                                //var mysubjectCategory = db.Subjects.Where(x => x.CourseCode.Equals(subjectValue))
                                //    .Select(c => c.CategoriesId).FirstOrDefault();
                                var subjectName = db.Subjects.Where(x => x.CourseCode.Equals(subjectValue))
                                    .Select(c => c.CourseName).FirstOrDefault();

                                var CA = db.ContinuousAssessments.Where(x => x.ClassName.Equals(className)
                                                                             && x.TermName.Contains(termName)
                                                                             && x.SessionName.Equals(sessionName)
                                                                             && x.StudentId.Equals(studentId)
                                                                             && x.SubjectCode.Equals(subjectName));
                                var countFromDb = CA.Count();
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
                                        Assignment1 = double.Parse(sheet.Cells[row, 3].Value.ToString()),
                                        Assignment2 = double.Parse(sheet.Cells[row, 4].Value.ToString()),
                                        FirstTest = double.Parse(sheet.Cells[row, 5].Value.ToString()),
                                        SecondTest = double.Parse(sheet.Cells[row, 6].Value.ToString()),
                                        ExamScore = double.Parse(sheet.Cells[row, 7].Value.ToString()),
                                        TermName = termName,
                                        SessionName = sessionName,
                                        StaffName = sheet.Cells[row, 10].Value.ToString(),
                                        ClassName = className,
                                        //SubjectCategory = mysubjectCategory
                                    };
                                    db.ContinuousAssessments.Add(mycontinuousAssessment);
                                }

                            }
                        }
                    }
                    await db.SaveChangesAsync();
                    return View("Success");
                }
                else
                {
                    ViewBag.Error = "File type is Incorrect <br/>";
                    return View("Index");
                }
            }

        }
    }
}

