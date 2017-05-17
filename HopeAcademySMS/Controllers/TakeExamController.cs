using SwiftSkool.Models;
using SwiftSkool.Models.CBT;
using SwiftSkool.ViewModel.CBTE;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkool.Controllers
{
    public class TakeExamController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: TakeExam
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult SelectSubject()
        {
            ViewBag.SubjectName = new SelectList(db.Subjects, "CourseCode", "CourseCode");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SelectSubject(SelectSubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var totalQuestion = db.ExamRules.Where(x => x.ClassName.Equals("JSS1")).Select(s => s.TotalQuestion).FirstOrDefault();

                //var r = new Random();
                //var tenRandomUser = listUsr.OrderBy(u => r.Next()).Take(10);

                int count = 1;
                var questions = db.QuestionAnswers.Where(x => x.SubjectName.Equals(model.SubjectName)).Take(totalQuestion);
                foreach (var question in questions)
                {
                    var studentQuestion = new StudentQuestion()
                    {
                        StudentId = "HAS-201",
                        Question = question.Question,
                        Option1 = question.Option1,
                        Option2 = question.Option2,
                        Option3 = question.Option3,
                        Option4 = question.Option4,
                        Answer = question.Answer,
                        QuestionHint = question.QuestionHint,
                        QuestionNumber = count
                    };
                    db.StudentQuestions.Add(studentQuestion);
                    count++;
                }
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            ViewBag.SubjectName = new SelectList(db.Subjects, "CourseCode", "CourseCode");
            return View(model);
        }

        [HttpGet]
        public ActionResult Exam(int? questionNo)
        {
            DisplayQuestionViewModel model = new DisplayQuestionViewModel();
            var totalQuestion = db.ExamRules.Where(x => x.ClassName.Equals("")).Select(s => s.TotalQuestion).FirstOrDefault();

            if (questionNo != null && (int)questionNo <= totalQuestion)
            {
                questionNo = null;
            }
            if (questionNo == null)
            {
                var question = db.StudentQuestions.FirstOrDefault(s => s.StudentId.Equals("HAS-201")
                                                                       && s.QuestionNumber.Equals(1));
                if (question == null) return View();
                model.Question = question.Question;
                model.Option1 = question.Option1;
                model.Option2 = question.Option2;
                model.Option3 = question.Option3;
                model.Option4 = question.Option4;
                model.QuestionNo = 1;
            }
            else
            {
                int myno = (int)questionNo;
                var question = db.StudentQuestions.FirstOrDefault(s => s.StudentId.Equals("HAS-201")
                                                                       && s.QuestionNumber.Equals(myno));
                //if (question == null) return View();
                model.Question = question.Question;
                model.Option1 = question.Option1;
                model.Option2 = question.Option2;
                model.Option3 = question.Option3;
                model.Option4 = question.Option4;
                model.QuestionNo = (int)questionNo;
            }

            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> Exam(DisplayQuestionViewModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    return RedirectToAction("Index");
            //}

            if (!String.IsNullOrEmpty(model.SelectedAnswer))
            {
                var question = db.StudentQuestions.FirstOrDefault(s => s.StudentId.Equals("HAS-201")
                                                                       && s.QuestionNumber.Equals(model.QuestionNo));
                if (question.Answer.ToLower().Equals(model.SelectedAnswer.ToLower()))
                {
                    question.IsCorrect = true;
                    db.Entry(question).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                else
                {
                    question.IsCorrect = false;
                    db.Entry(question).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                return RedirectToAction("Exam", "TakeExam", new { questionNo = ++question.QuestionNumber });
            }
            ViewBag.SubjectName = new SelectList(db.Subjects, "CourseCode", "CourseCode");
            return View();
        }
    }

}
