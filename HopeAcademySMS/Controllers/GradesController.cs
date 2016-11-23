using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HopeAcademySMS.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace HopeAcademySMS.Controllers
{
    public class GradesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Grades
        public async Task<ActionResult> Index()
        {
            var grades = db.Grades.Include(g => g.Course).Include(g => g.Student);
            return View(await grades.ToListAsync());
        }

        // GET: Grades/Details/5
        public async Task<ActionResult> Details(int? id)
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
            return View(grade);
        }

        // GET: Grades/Create
        public ActionResult Create()
        {
            ViewBag.CourseCode = new SelectList(db.Courses, "CourseCode", "CourseName");
            ViewBag.StudentNumber = new SelectList(db.Students, "StudentNumber", "Username");
            return View();
        }

        // POST: Grades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "GradeID,StudentNumber,CourseCode,Assignment,FirstTest,SecondTest,ExamScore,Term,StaffName")] Grade grade)
        {
            if (ModelState.IsValid)
            {
                db.Grades.Add(grade);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseCode = new SelectList(db.Courses, "CourseCode", "CourseName", grade.CourseCode);
            ViewBag.StudentNumber = new SelectList(db.Students, "StudentNumber", "Username", grade.StudentNumber);
            return View(grade);
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
            ViewBag.CourseCode = new SelectList(db.Courses, "CourseCode", "CourseName", grade.CourseCode);
            ViewBag.StudentNumber = new SelectList(db.Students, "StudentNumber", "Username", grade.StudentNumber);
            return View(grade);
        }

        // POST: Grades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "GradeID,StudentNumber,CourseCode,Assignment,FirstTest,SecondTest,ExamScore,Term,StaffName")] Grade grade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grade).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseCode = new SelectList(db.Courses, "CourseCode", "CourseName", grade.CourseCode);
            ViewBag.StudentNumber = new SelectList(db.Students, "StudentNumber", "Username", grade.StudentNumber);
            return View(grade);
        }

        // GET: Grades/Delete/5
        public async Task<ActionResult> Delete(int? id)
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
            return View(grade);
        }

        // POST: Grades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Grade grade = await db.Grades.FindAsync(id);
            db.Grades.Remove(grade);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult UploadResult()
        {
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
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/Content/ExcelUploadedFile/" + excelfile.FileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelfile.SaveAs(path);

                    // Read data from excel file
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;

                    List<Grade> listSavingsMaintenance = new List<Grade>();

                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        var myGrade = new Grade
                        {
                            StudentNumber = ((Excel.Range)range.Cells[row, 1]).Text,
                            CourseCode = ((Excel.Range)range.Cells[row, 2]).Text,
                            Assignment = double.Parse(((Excel.Range)range.Cells[row, 3]).Text),
                            FirstTest = double.Parse(((Excel.Range)range.Cells[row, 4]).Text),
                            SecondTest = double.Parse(((Excel.Range)range.Cells[row, 5]).Text),
                            ExamScore = double.Parse(((Excel.Range)range.Cells[row, 6]).Text),
                            Term = ((Excel.Range)range.Cells[row, 7]).Text,
                            StaffName = ((Excel.Range)range.Cells[row, 8]).Text,
                        };
                        db.Grades.Add(myGrade);
                        //listSavingsMaintenance.Add(mySavingMaintenance);
                    }
                    //db.SavingsMaintenances.Add(listSavingsMaintenance);
                    await db.SaveChangesAsync();

                    //ViewBag.ListSavingsMaintenance = listSavingsMaintenance;

                    return View("Success");
                }
                else
                {
                    ViewBag.Error = "File type is Incorrect <br/>";
                    return View("Index");
                }
            }
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
