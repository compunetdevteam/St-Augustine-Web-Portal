using StAugustine.Models;
using System;
using System.Linq;

namespace StAugustine.BusinessLogic
{
    public class ResultCommandManager
    {

        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        public double TotalScorePerStudent(string studentId, string className, string term, string session)
        {
            var totalSum = _db.ContinuousAssessments.Where(x => x.StudentId.Equals(studentId) && x.ClassName.Equals(className)
                                                             && x.TermName.Equals(term) && x.SessionName.Equals(session))
                .Sum(y => y.Total);
            return totalSum;
        }

        public int SubjectOfferedByStudent(string className)
        {
            //var className = _db.AssignedClasses.Where(x => x.StudentId.Equals(studentId)
            //                                                && x.TermName.Equals(term)
            //                                                && x.SessionName.Equals(session))
            //                                            .Select(y => y.ClassName)
            //                                            .FirstOrDefault();


            var subjectPerStudent = _db.AssignSubjects.Count(x => x.ClassName.Equals(className));
            return subjectPerStudent;
        }

        public double TotalScorePerSubject(string subject, string className, string term, string session)
        {
            var sumPerSubject = _db.ContinuousAssessments.Where(x => x.SubjectCode.Equals(subject)
                                                                    && x.ClassName.Equals(className)
                                                                    && x.TermName.Equals(term) &&
                                                                    x.SessionName.Equals(session)).Sum(y => y.Total);
            return Math.Round(sumPerSubject, 2);
        }



        public int NumberOfStudentPerClass(string className, string term, string session)
        {
            var studentPerClass = _db.AssignedClasses.Count(x => x.ClassName.Equals(className) &&
                                                                x.TermName.Equals(term) &&
                                                                x.SessionName.Equals(session));
            return studentPerClass;
        }

        //public void FindAggregatePosition(string className, string term, string session)
        //{
        //    var myAggregatePosition = db.Results.Where(x => x.ClassName.Equals(className) &&
        //                                                             x.Term.Equals(term) &&
        //                                                             x.SessionName.Equals(session))
        //                                                        .OrderByDescending(y => y.AggretateScore);
        //}

        public int FindSubjectPosition(string studentId, string subject, string className, string term, string session)
        {
            int subjectPosition = 0;
            var mySubjectPosition = _db.ContinuousAssessments.Where(x => x.SubjectCode.Equals(subject) &&
                                                                        x.ClassName.Equals(className) &&
                                                                        x.TermName.Equals(term) &&
                                                                        x.SessionName.Equals(session));

            // .OrderByDescending(y => y.Total);

            var q = from s in mySubjectPosition
                    orderby s.Total descending
                    select new
                    {
                        Name = s.StudentId,
                        Rank = (from o in mySubjectPosition
                                where o.Total > s.Total
                                select o).Count() + 1
                    };

            foreach (var item in q.Where(s => s.Name.Equals(studentId)))
            {
                subjectPosition = item.Rank;
            }

            return subjectPosition;
        }


        public int FindAggregatePosition(string studentId, string className, string term, string session)
        {
            int subjectPosition = 0;
            var resultPosition = _db.Results.Where(x => x.ClassName.Equals(className) &&
                                                       x.Term.Equals(term) &&
                                                       x.SessionName.Equals(session)
                                                       && x.SubjectName.Contains("Mathematics"));
            //.OrderByDescending(y => y.AggretateScore);

            var q = from s in resultPosition
                    orderby s.AggretateScore descending
                    select new
                    {
                        Name = s.StudentId,
                        Rank = (from o in resultPosition
                                where o.AggretateScore > s.AggretateScore
                                select o).Count() + 1
                    };

            foreach (var item in q.Where(s => s.Name.Equals(studentId)))
            {
                subjectPosition = item.Rank;
            }

            return subjectPosition;
        }

        public double CalculateAverage(string studentId, string className, string term, string sessionName)
        {
            double scorePerstudent = TotalScorePerStudent(studentId, className, term, sessionName);
            int subjectOffered = SubjectOfferedByStudent(className);
            return Math.Round((scorePerstudent / subjectOffered), 2);
        }

        public double CalculateClassAverage(string className, string term, string sessionName, string subject)
        {
            var scorePerSubject = TotalScorePerSubject(subject, className, term, sessionName);
            var studentInClass = NumberOfStudentPerClass(className, term, sessionName);
            return Math.Round((scorePerSubject / studentInClass), 2);
        }

        public int TotalQualityPoint(string studentId, string className, string term, string sessionName)
        {
            var totalPoint = _db.ContinuousAssessments.Where(x => x.ClassName.Equals(className) &&
                                                                x.TermName.Equals(term) &&
                                                                x.SessionName.Equals(sessionName) &&
                                                                x.StudentId.Equals(studentId))
                                                                .Sum(c => c.QualityPoint);
            return totalPoint;
        }

        public int TotalcreditUnit(string className)
        {
            var totalCredit = _db.AssignSubjects.Count(s => s.ClassName.Equals(className));
            return totalCredit * 2;
        }

        //public async Task<ActionResult> SubjectPostion(string CourseName, string level, string term, string session)
        //{
        //    var grades = db.Grades.Include(g => g.Course).Include(g => g.Student)
        //                    .Where(s => s.CourseCode.Equals(CourseName) && s.Student.MyClassName.Equals(level)
        //                        && s.TermName.Equals(term) && s.SessionName.Equals(session))
        //                    .OrderByDescending(s => s.Total);
        //    if (grades == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    int myposition = 0;
        //    double? myTotal = 0;
        //    foreach (var item in grades)
        //    {
        //        Grade grade = db.Grades.Find(item.GradeID);
        //        {
        //            if (myTotal == item.Total)
        //            {
        //                grade.SubjectPosition = myposition;
        //            }
        //            else
        //            {
        //                grade.SubjectPosition = ++myposition;
        //            }
        //            myTotal = item.Total;
        //        };
        //        db.Entry(grade).State = EntityState.Modified;

        //        //db.SubjectPositions.Add(grade);
        //    }
        //    await db.SaveChangesAsync();
        //    ViewBag.CourseName = new SelectList(db.Courses, "CourseCode", "CourseName");
        //    return View(await grades.ToListAsync());
        //    //db.Entry(grade).State = EntityState.Modified;
        //    //await db.SaveChangesAsync();
        //}

    }
}