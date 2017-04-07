using HopeAcademySMS.Services;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using PagedList;
using StAugustine.BusinessLogic;
using StAugustine.Models;
using StAugustine.Services;
using StAugustine.ViewModel;
using SwiftKampus.Services;
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
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly GradeRemark _gradeRemark = new GradeRemark();
        private readonly ResultCommandManager _resultCommand = new ResultCommandManager();
        // GET: Students
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string search, int? page, string whatever)
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

            var studentList = from s in _db.Students.AsNoTracking() select s;
            if (User.IsInRole("Guardian"))
            {
                string name = User.Identity.GetUserName();
                var user = await _db.Users.AsNoTracking().Where(c => c.UserName.Equals(name)).Select(x => x.PhoneNumber).FirstOrDefaultAsync();
                studentList = studentList.Where(s => s.GuardianEmail.ToUpper().Contains(user.ToUpper()));
            }
            else
            {
                if (!String.IsNullOrEmpty(search))
                {
                    studentList = studentList.AsNoTracking().Where(s => s.LastName.ToUpper().Contains(search.ToUpper())
                                                         || s.FirstName.ToUpper().Contains(search.ToUpper())
                                                         || s.StudentId.ToUpper().Contains(search.ToUpper()));

                }
            }

            switch (sortOrder)
            {
                case "name_desc":
                    studentList = studentList.AsNoTracking().OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    studentList = studentList.AsNoTracking().OrderBy(s => s.FirstName);
                    break;
                default:
                    studentList = studentList.AsNoTracking().OrderBy(s => s.LastName);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.Message = whatever;
            return View(studentList.ToPagedList(pageNumber, pageSize));
            //return View(studentList.ToList());
        }

        public ActionResult GuardianIndex(string sortOrder, string currentFilter, string search, int? page)
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

            var studentList = from s in _db.Students.AsNoTracking() select s;
            if (User.IsInRole("Guardian"))
            {
                string name = User.Identity.GetUserName();
                //var user = db.Guardians.Where(c => c.UserName.Equals(name)).Select(s => s.Email).FirstOrDefault();

                //var studentName = studentList.Where(x => x.GuardianEmail.Equals(user)).ToList();

                //return View(studentName);
            }

            if (!String.IsNullOrEmpty(search))
            {
                studentList = studentList.AsNoTracking().Where(s => s.LastName.ToUpper().Contains(search.ToUpper())
                                                     || s.FirstName.ToUpper().Contains(search.ToUpper())
                                                     || s.StudentId.ToUpper().Contains(search.ToUpper()));

            }
            switch (sortOrder)
            {
                case "name_desc":
                    studentList = studentList.AsNoTracking().OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    studentList = studentList.AsNoTracking().OrderBy(s => s.FirstName);
                    break;
                default:
                    studentList = studentList.AsNoTracking().OrderBy(s => s.LastName);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            //ViewBag.Message = whatever;
            return View(studentList.ToPagedList(pageNumber, pageSize));
            //return View(studentList.ToList());
        }



        public async Task<ActionResult> Dashboard()
        {
            int totalMaleStudent = await _db.Students.AsNoTracking().CountAsync(s => s.Gender.Equals("Male"));
            int totalFemaleStudent = await _db.Students.AsNoTracking().CountAsync(s => s.Gender.Equals("Female"));
            int active = await _db.Students.AsNoTracking().CountAsync(s => s.Active.Equals(true));
            int graduatedStudent = await _db.Students.AsNoTracking().CountAsync(s => s.IsGraduated.Equals(true));
            int totalStudent = await _db.Students.AsNoTracking().CountAsync();
            int totalStaff = await _db.Staffs.AsNoTracking().CountAsync();

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
            ViewBag.ActiveStudent = active;
            ViewBag.GraduatedStudent = graduatedStudent;

            return View();
        }
        // GET: Students/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Student student = await _db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }


        // GET: Students/Create
        public async Task<ActionResult> Create()
        {
            var role = await _db.Roles.AsNoTracking().SingleOrDefaultAsync(m => m.Name == "Guardian");
            var usersInRole = _db.Users.AsNoTracking().Where(m => m.Roles.Any(r => r.RoleId == role.Id));
            ViewBag.GuardianId = new SelectList(usersInRole, "PhoneNumber", "UserName");
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
                                            model.StudentPassport, false);

                _db.Students.Add(student);
                await _db.SaveChangesAsync();

                TempData["UserMessage"] = "Student has been Added Successfully";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            };
            var role = await _db.Roles.AsNoTracking().SingleOrDefaultAsync(m => m.Name == "Guardian");
            var usersInRole = _db.Users.AsNoTracking().Where(m => m.Roles.Any(r => r.RoleId == role.Id));
            ViewBag.GuardianId = new SelectList(usersInRole, "PhoneNumber", "UserName");
            return View(model);
        }

        // GET: Students/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await _db.Students.FindAsync(id);
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
                AdmissionDate = student.AdmissionDate,
                StudentPassport = student.StudentPassport
            };
            //var role = _db.Roles.SingleOrDefault(m => m.Name == "Guardian");
            //var usersInRole = _db.Users.Where(m => m.Roles.Any(r => r.RoleId == role.Id));
            // ViewBag.GuardianId = new SelectList(_db.Guardians, "PhoneNumber", "UserName");
            var role = await _db.Roles.AsNoTracking().SingleOrDefaultAsync(m => m.Name == "Guardian");
            var usersInRole = _db.Users.AsNoTracking().Where(m => m.Roles.Any(r => r.RoleId == role.Id));
            ViewBag.GuardianId = new SelectList(usersInRole, "PhoneNumber", "UserName");
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
                                            model.StudentPassport, false);
                _db.Entry(student).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                TempData["UserMessage"] = "Student has been Updated Successfully";
                TempData["Title"] = "Success.";

                return RedirectToAction("Index");
            }
            //var role = _db.Roles.SingleOrDefault(m => m.Name == "Guardian");
            //var usersInRole = _db.Users.Where(m => m.Roles.Any(r => r.RoleId == role.Id));
            //ViewBag.GuardianId = new SelectList(usersInRole, "PhoneNumber", "UserName");
            //ViewBag.GuardianId = new SelectList(_db.Guardians, "PhoneNumber", "UserName");
            var role = await _db.Roles.AsNoTracking().SingleOrDefaultAsync(m => m.Name == "Guardian");
            var usersInRole = _db.Users.AsNoTracking().Where(m => m.Roles.Any(r => r.RoleId == role.Id));
            ViewBag.GuardianId = new SelectList(usersInRole, "PhoneNumber", "UserName");
            return View(model);
        }

        // GET: Students/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await _db.Students.FindAsync(id);
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
            Student student = await _db.Students.FindAsync(id);
            if (student != null) _db.Students.Remove(student);
            TempData["UserMessage"] = "Student has been Deleted Successfully.";
            TempData["Title"] = "Deleted.";
            await _db.SaveChangesAsync();
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
                    string lastrecord = "";
                    int recordCount = 0;
                    string message = "";
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                    // Read data from excel file
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        ExcelValidation myExcel = new ExcelValidation();
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        int requiredField = 7;

                        string validCheck = myExcel.ValidateExcel(noOfRow, workSheet, requiredField);
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
                            string lineError = $"Line/Row number {myArray[0]}  and column {myArray[1]} is not rightly formatted, Please Check for anomalies ";
                            //ViewBag.LineError = lineError;
                            TempData["UserMessage"] = lineError;
                            TempData["Title"] = "Error.";
                            return View();
                        }
                        for (int row = 2; row <= noOfRow; row++)
                        {
                            try
                            {
                                string studentId = workSheet.Cells[row, 1].Value.ToString().Trim();
                                string firstName = workSheet.Cells[row, 2].Value.ToString().Trim();
                                string middleName = workSheet.Cells[row, 3].Value.ToString().Trim();
                                string lastName = workSheet.Cells[row, 4].Value.ToString().Trim();
                                string gender = workSheet.Cells[row, 5].Value.ToString().Trim();
                                DateTime dateofBirth = DateTime.Parse(workSheet.Cells[row, 6].Value.ToString().Trim());
                                DateTime admissionDate = DateTime.Parse(workSheet.Cells[row, 7].Value.ToString().Trim());
                                //string guardianId = workSheet.Cells[row, 8].Value.ToString().Trim();

                                var student = new Student(studentId, firstName, middleName, lastName,
                               gender, dateofBirth, admissionDate);


                                _db.Students.Add(student);
                                recordCount++;
                                lastrecord = $"The last Updated record has the Last Name {lastName} and First Name {firstName} with Student Id {studentId}";
                            }
                            catch (Exception e)
                            {
                                return View("Error3");
                            }

                        }
                        await _db.SaveChangesAsync();
                        message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                        TempData["UserMessage"] = message;
                        TempData["Title"] = "Success.";

                    }

                    return RedirectToAction("Index", "Students");
                }

                else
                {
                    ViewBag.Error = $"File type is Incorrect <br/>";
                    return View("Index");
                }
            }


        }

        public async Task<ActionResult> RenderImage(string studentId)
        {
            Student student = await _db.Students.FindAsync(studentId);

            byte[] photoBack = student.StudentPassport;

            return File(photoBack, "image/png");
        }

        public async Task<ActionResult> RenderSignature(string studentId)
        {
            ReportCard student = await _db.ReportCards.FindAsync(studentId);

            byte[] photoBack = student.PrincipalSignature;

            return File(photoBack, "image/png");
        }

        public PartialViewResult GuardianInfo(string studentNumber)
        {
            var GuardianInfoes = _db.Guardians.Include(p => p.Student).Where(s => s.GuardianEmail.Contains(studentNumber));
            return PartialView(GuardianInfoes);
        }

        public async Task<ActionResult> PrintSecondTerm(string id, string term, string sessionName)
        {
            ReportViewModel reportModel = new ReportViewModel();
            reportModel.Student = await _db.Students.FindAsync(id);
            term = term.ToUpper();

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

            reportModel.ContinuousAssessments = await _db.ContinuousAssessments.AsNoTracking().Where(s => s.StudentId.Contains(id)
                                                    && s.TermName.Contains(term)
                                                    && s.SessionName.Contains(sessionName))
                                                    .OrderBy(y => y.SubjectCode).ToListAsync();

            reportModel.Results = await _db.Results.AsNoTracking().Where(s => s.StudentId.Contains(id)
                                                                 && s.Term.Contains(term)
                                                                 && s.SessionName.Contains(sessionName)).ToListAsync();

            var className = await _db.AssignedClasses.AsNoTracking().Where(x => x.StudentId.Equals(id) && x.TermName.Equals(term)
                                                     && x.SessionName.Equals(sessionName))
                                                .Select(y => y.ClassName)
                                                .FirstOrDefaultAsync();


            // var className = "JSS1 A";

            reportModel.NoOfStudentPerClass = await _db.AssignedClasses.AsNoTracking().CountAsync(x => x.ClassName.Contains(className) &&
                                                               x.TermName.Equals(term) &&
                                                               x.SessionName.Equals(sessionName));
            reportModel.NoOfSubjectOffered = await _resultCommand.SubjectOfferedByStudent(id, term, sessionName);
            //reportModel = _resultCommand.FindAggregatePosition(id, className, term, sessionName);
            reportModel.Average = await _db.Results.AsNoTracking().Where(s => s.StudentId.Contains(id)
                                                        && s.Term.Contains(term)
                                                        && s.SessionName.Contains(sessionName))
                                                        .Select(c => c.Average).FirstOrDefaultAsync();
            reportModel.TotalScore = await _db.Results.AsNoTracking().Where(s => s.StudentId.Contains(id)
                                                        && s.Term.Contains(term)
                                                        && s.SessionName.Contains(sessionName))
                                                        .Select(c => c.AggretateScore).FirstOrDefaultAsync();
            var myOtherSkills = await _db.Psychomotors.AsNoTracking().Where(s => s.StudentId.Contains(id)
                                                          && s.TermName.Contains(term)
                                                          && s.SessionName.Contains(sessionName)
                                                          && s.ClassName.Equals(className))
                                                         .Select(c => c.Id).FirstOrDefaultAsync();

            reportModel.GPA = await _db.Results.AsNoTracking().Where(s => s.StudentId.Contains(id)
                                                       && s.Term.Contains(term)
                                                       && s.SessionName.Contains(sessionName))
                                                        .Select(c => c.GPA).FirstOrDefaultAsync();
            reportModel.AggregateScore = await _resultCommand.TotalScorePerStudent(id, className, term, sessionName);
            reportModel.TotalQualityPoint = await _resultCommand.TotalQualityPoint(id, className, term, sessionName);
            reportModel.TotalCreditUnit = await _resultCommand.TotalcreditUnit(className);
            reportModel.GradePointAverage = Math.Round((reportModel.TotalQualityPoint / reportModel.TotalCreditUnit), 2);


            reportModel.BehaviorCataegory = await _db.BehaviorSkillCategories.AsNoTracking().Select(x => x.Name).ToListAsync();
            reportModel.Psychomotor = await _db.AssignBehaviors.Where(s => s.StudentId.Contains(id)
                                                                     && s.TermName.Contains(term)
                                                                     && s.SessionName.Contains(sessionName)).ToListAsync();
            //&& s.BehaviouralSkillId.Equals()).ToList();

            //reportModel.Affective = _db.Affectives.FirstOrDefault(s => s.StudentId.Contains(id)
            //                                                    && s.TermName.Contains(term)
            //                                                    && s.SessionName.Contains(sessionName)
            //                                                    && s.ClassName.Equals(className));

            //reportModel.TeacherComment = _db.AssignBehaviors.FirstOrDefault(x => x.StudentId.Equals(id) && x.TermName.Equals(term)
            // && x.SessionName.Equals(sessionName)).Select(c => c.TeacherComment);

            reportModel.TeacherComment = reportModel.Psychomotor.Select(c => c.TeacherComment).FirstOrDefault();
            reportModel.TeacherDate = reportModel.Psychomotor.Select(c => c.Date).FirstOrDefault();
            reportModel.ReportCard = await _db.ReportCards.FirstOrDefaultAsync(x => x.TermName.ToUpper().Equals(term)
                                                && x.SessionName.Equals(sessionName));


            //ViewBag.Class = 
            ViewBag.PrincipalComment = _gradeRemark.PrincipalRemark(reportModel.Average, className);
            ViewBag.Term = term;
            ViewBag.Session = sessionName;
            ViewBag.ClassName = className;
            ViewBag.Absent = reportModel.Psychomotor.Select(x => x.NoOfAbsence).FirstOrDefault();

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

        //public async Task<ActionResult> QueryResult(string StudentId, string TermName, string SessionName)
        //{
        //    ReportViewModel reportModel = new ReportViewModel();
        //    try
        //    {

        //        string id = StudentId;
        //        string sessionName = SessionName;
        //        string term = TermName.ToUpper();
        //        reportModel.Student = await _db.Students.FindAsync(id);


        //        //reportModel.Maths = db.ContinuousAssessments.Include(p => p.Student)
        //        //                                        .Where(s => s.StudentId.Contains(id)
        //        //                                        && s.TermName.Contains(term)
        //        //                                        && s.SessionName.Contains(sessionName)
        //        //                                        && s.SubjectCategory.Equals("Mathematics")).ToList();

        //        //reportModel.English = db.ContinuousAssessments.Include(p => p.Student)
        //        //                                       .Where(s => s.StudentId.Contains(id)
        //        //                                       && s.TermName.Contains(term)
        //        //                                       && s.SessionName.Contains(sessionName)
        //        //                                       && s.SubjectCategory.Equals("English")).ToList();

        //        reportModel.ContinuousAssessments = _db.ContinuousAssessments.Where(s => s.StudentId.Contains(id)
        //                                                && s.TermName.Contains(term)
        //                                                && s.SessionName.Contains(sessionName))
        //                                                .OrderBy(y => y.SubjectCode).ToList();

        //        reportModel.Results = _db.Results.Where(s => s.StudentId.Contains(id)
        //                                                             && s.Term.Contains(term)
        //                                                             && s.SessionName.Contains(sessionName)).ToList();

        //        var className = _db.AssignedClasses.Where(x => x.StudentId.Equals(id) && x.TermName.Equals(term)
        //                                                 && x.SessionName.Equals(sessionName))
        //                                            .Select(y => y.ClassName)
        //                                            .FirstOrDefault();


        //        // var className = "JSS1 A";

        //        reportModel.NoOfStudentPerClass = _db.AssignedClasses.Count(x => x.ClassName.Contains(className) &&
        //                                                           x.TermName.Equals(term) &&
        //                                                           x.SessionName.Equals(sessionName));
        //        reportModel.NoOfSubjectOffered = _resultCommand.SubjectOfferedByStudent(id, term, sessionName);
        //        //reportModel = _resultCommand.FindAggregatePosition(id, className, term, sessionName);
        //        reportModel.Average = _db.Results.Where(s => s.StudentId.Contains(id)
        //                                                    && s.Term.Contains(term)
        //                                                    && s.SessionName.Contains(sessionName))
        //                                                    .Select(c => c.Average).FirstOrDefault();
        //        reportModel.TotalScore = _db.Results.Where(s => s.StudentId.Contains(id)
        //                                                    && s.Term.Contains(term)
        //                                                    && s.SessionName.Contains(sessionName))
        //                                                    .Select(c => c.AggretateScore).FirstOrDefault();
        //        var myOtherSkills = _db.Psychomotors.Where(s => s.StudentId.Contains(id)
        //                                                      && s.TermName.Contains(term)
        //                                                      && s.SessionName.Contains(sessionName)
        //                                                      && s.ClassName.Equals(className))
        //                                                     .Select(c => c.Id).FirstOrDefault();

        //        reportModel.GPA = _db.Results.Where(s => s.StudentId.Contains(id)
        //                                                   && s.Term.Contains(term)
        //                                                   && s.SessionName.Contains(sessionName))
        //                                                    .Select(c => c.GPA).FirstOrDefault();
        //        reportModel.AggregateScore = _resultCommand.TotalScorePerStudent(id, className, term, sessionName);
        //        reportModel.TotalQualityPoint = _resultCommand.TotalQualityPoint(id, className, term, sessionName);
        //        reportModel.TotalCreditUnit = _resultCommand.TotalcreditUnit(className);
        //        reportModel.GradePointAverage = Math.Round((reportModel.TotalQualityPoint / reportModel.TotalCreditUnit), 2);


        //        reportModel.BehaviorCataegory = _db.BehaviorSkillCategories.ToList().Select(x => x.Name).ToList();
        //        reportModel.Psychomotor = _db.AssignBehaviors.Where(s => s.StudentId.Contains(id)
        //                                                                 && s.TermName.Contains(term)
        //                                                                 && s.SessionName.Contains(sessionName)).ToList();
        //        //&& s.BehaviouralSkillId.Equals()).ToList();

        //        //reportModel.Affective = _db.Affectives.FirstOrDefault(s => s.StudentId.Contains(id)
        //        //                                                    && s.TermName.Contains(term)
        //        //                                                    && s.SessionName.Contains(sessionName)
        //        //                                                    && s.ClassName.Equals(className));

        //        //reportModel.TeacherComment = _db.AssignBehaviors.FirstOrDefault(x => x.StudentId.Equals(id) && x.TermName.Equals(term)
        //        // && x.SessionName.Equals(sessionName)).Select(c => c.TeacherComment);

        //        reportModel.TeacherComment = reportModel.Psychomotor.Select(c => c.TeacherComment).FirstOrDefault();
        //        reportModel.TeacherDate = reportModel.Psychomotor.Select(c => c.Date).FirstOrDefault();
        //        reportModel.ReportCard = _db.ReportCards.FirstOrDefault(x => x.TermName.ToUpper().Equals(term)
        //                                            && x.SessionName.Equals(sessionName));


        //        //ViewBag.Class = 
        //        ViewBag.PrincipalComment = _gradeRemark.PrincipalRemark(reportModel.GPA, className);
        //        ViewBag.Term = term;
        //        ViewBag.Session = sessionName;
        //        ViewBag.ClassName = className;
        //        ViewBag.Absent = reportModel.Psychomotor.Select(x => x.NoOfAbsence).FirstOrDefault();
        //        return View(reportModel);
        //    }
        //    catch (Exception)
        //    {
        //        ViewBag.PrincipalComment = null;
        //        ViewBag.Term = null;
        //        ViewBag.Session = null;
        //        ViewBag.ClassName = null;
        //        ViewBag.Absent = reportModel.Psychomotor.Select(x => x.NoOfAbsence).FirstOrDefault();
        //        ViewBag.StudentId = new SelectList(_db.Students.AsNoTracking(), "StudentID", "FullName");
        //        ViewBag.SessionName = new SelectList(_db.Sessions.AsNoTracking(), "SessionName", "SessionName");
        //        // ViewBag.ClassName = new SelectList(db.Classes, "FullClassName", "FullClassName");
        //        ViewBag.TermName = new SelectList(_db.Terms.AsNoTracking(), "TermName", "TermName");
        //        return View(reportModel);
        //    }


        //}

        //public ActionResult PrintTest(string id, string term, string sessionName)
        //{


        //    var className = _db.AssignedClasses.Where(x => x.StudentId.Equals(id) && x.TermName.ToUpper().Trim().Equals(term.ToUpper().Trim())
        //                                             && x.SessionName.ToUpper().Trim().Equals(sessionName.ToUpper().Trim()))
        //                                        .Select(y => y.ClassName)
        //                                        .FirstOrDefault();
        //    string subject = "MATHEMATICS";

        //    // var className = "JSS1 A";

        //    ViewBag.Subject = _resultCommand.SubjectOfferedByStudent(id, term, sessionName);
        //    var sumPerSubject = _db.ContinuousAssessments.Where(x => x.SubjectCode.ToUpper().Trim().Equals(subject.ToUpper().Trim())
        //                                                           && x.ClassName.ToUpper().Trim().Equals(className.ToUpper().Trim())
        //                                                            && x.TermName.ToUpper().Trim().Equals(term.ToUpper().Trim())
        //                                                    && x.SessionName.ToUpper().Trim().Equals(sessionName.ToUpper().Trim()))
        //                                                    .Sum(y => y.Total);
        //    double classAverage = _resultCommand.CalculateClassAverage(className, term, sessionName, subject.ToUpper().Trim());
        //    var studentPerClass = _db.AssignedClasses.Count(x => x.ClassName.ToUpper().Trim().Equals(className.ToUpper().Trim())
        //                                                        && x.TermName.ToUpper().Trim().Equals(term.ToUpper().Trim())
        //                                                     && x.SessionName.ToUpper().Trim().Equals(sessionName.ToUpper().Trim()));

        //    double average = _resultCommand.CalculateAverage(id, className, term, sessionName);
        //    double totalScore = _resultCommand.TotalScorePerStudent(id, className, term, sessionName);
        //    // return Math.Round(sumPerSubject, 2);
        //    ViewBag.Term = term;
        //    ViewBag.Session = sessionName;
        //    ViewBag.ClassName = className;
        //    ViewBag.SubjectTotal = Math.Round(sumPerSubject, 2);

        //    ViewBag.ClassAverage = classAverage;
        //    ViewBag.StudentPerClass = studentPerClass;
        //    ViewBag.Average = average;
        //    ViewBag.TotalScore = totalScore;

        //    return View();

        //}
        public ActionResult PrintSummaryReport(string id, string sessionName)
        {
            SummaryReportViewModel summary = new SummaryReportViewModel();
            summary.Results = _db.Results.Where(s => s.StudentId.Contains(id)
                                    && s.SessionName.Contains(sessionName)).ToList();
            summary.ReportSummaries = _db.ReportSummarys.Where(s => s.StudentId.Equals(id)
                                                && s.SessionName.Equals(sessionName)).ToList();
            //foreach (var item in studentResults.Where(c => c.))
            //{

            //}
            return View(summary);
        }

        public PartialViewResult ResultInfo(string studentNumber)
        {
            var resultInfoes = _db.ContinuousAssessments.Where(s => s.StudentId.Contains(studentNumber));
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

        public ActionResult Calender()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public PartialViewResult NewCalender()
        {
            return PartialView();
        }
        public string Init()
        {
            bool rslt = Utils.InitialiseDiary();
            return rslt.ToString();
        }

        public void UpdateEvent(int id, string NewEventStart, string NewEventEnd)
        {
            DiaryEvent.UpdateDiaryEvent(id, NewEventStart, NewEventEnd);
        }


        public bool SaveEvent(string Title, string NewEventDate, string NewEventTime, string NewEventDuration)
        {
            return DiaryEvent.CreateNewEvent(Title, NewEventDate, NewEventTime, NewEventDuration);
        }

        public JsonResult GetDiarySummary(double start, double end)
        {
            var ApptListForDate = DiaryEvent.LoadAppointmentSummaryInDateRange(start, end);
            var eventList = from e in ApptListForDate
                            select new
                            {
                                id = e.ID,
                                title = e.Title,
                                start = e.StartDateString,
                                end = e.EndDateString,
                                someKey = e.SomeImportantKeyID,
                                allDay = false
                            };
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDiaryEvents(double start, double end)
        {
            var ApptListForDate = DiaryEvent.LoadAllAppointmentsInDateRange(start, end);
            var eventList = from e in ApptListForDate
                            select new
                            {
                                id = e.ID,
                                title = e.Title,
                                start = e.StartDateString,
                                end = e.EndDateString,
                                color = e.StatusColor,
                                className = e.ClassName,
                                someKey = e.SomeImportantKeyID,
                                allDay = false
                            };
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
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
