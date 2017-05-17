using SwiftSkool.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SwiftSkool.BusinessLogic
{
    public class ResultCommandManager : IDisposable
    {

        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        public async Task<double> TotalScorePerStudent(string studentId, string className, string term, string session)
        {
            var totalSum = await _db.ContinuousAssessments.AsNoTracking().Where(x => x.StudentId.ToUpper().Trim().Equals(studentId.ToUpper().Trim())
                                                            && x.ClassName.ToUpper().Trim().Equals(className.ToUpper().Trim())
                                                             && x.TermName.ToUpper().Trim().Equals(term.ToUpper().Trim())
                                                             && x.SessionName.ToUpper().Trim().Equals(session.ToUpper().Trim()))
                                                            .SumAsync(y => y.Total);

            return Math.Round(totalSum, 2);
        }

        public async Task<int> SubjectOfferedByStudent(string studentId, string termName, string sessionName)
        {
            int subjectPerStudent = 0;
            var className = await _db.AssignedClasses.AsNoTracking().Where(x => x.StudentId.ToUpper().Trim().Equals(studentId.ToUpper().Trim())
                                                            && x.TermName.ToUpper().Trim().Equals(termName.ToUpper().Trim())
                                                            && x.SessionName.ToUpper().Trim().Equals(sessionName.ToUpper().Trim()))
                                                        .Select(y => y.ClassName).FirstOrDefaultAsync();

            var subjectAssigned = await _db.AssignSubjects.CountAsync(c => c.ClassName.ToUpper().Trim().Equals(className.ToUpper().Trim()));
            var subjectregistration = await _db.SubjectRegistrations.AsNoTracking().CountAsync(x => x.StudentId.ToUpper().Trim().Equals(studentId.ToUpper().Trim())
                                                    && x.TermName.ToUpper().Trim().Equals(termName.ToUpper().Trim())
                                                    && x.SessionName.ToUpper().Trim().Equals(sessionName.ToUpper().Trim())
                                                    && x.ClassName.ToUpper().Trim().Equals(className.ToUpper().Trim()));
            if (subjectAssigned > 1 && subjectregistration > 1)
            {
                subjectPerStudent = subjectregistration;
            }
            else if (subjectAssigned > 1 && subjectregistration < 2)
            {
                subjectPerStudent = subjectAssigned;
            }
            else if (subjectAssigned < 2 && subjectregistration > 1)
            {
                subjectPerStudent = subjectregistration;
            }

            //var noOfSubjectPerStudent = _db.AssignSubjects.Count(x => x.ClassName.Equals(className));
            return subjectPerStudent;
        }

        public async Task<double> TotalScorePerSubject(string subject, string className, string term, string session)
        {

            double sumPerSubject = await _db.ContinuousAssessments.AsNoTracking().Where(x => x.SubjectCode.ToUpper().Trim().Equals(subject.ToUpper().Trim())
                                                                    && x.ClassName.ToUpper().Trim().Equals(className.ToUpper().Trim())
                                                                     && x.TermName.ToUpper().Trim().Equals(term.ToUpper().Trim())
                                                             && x.SessionName.ToUpper().Trim().Equals(session.ToUpper().Trim()))
                                                             .SumAsync(y => y.Total);
            return Math.Round(sumPerSubject, 2);
        }



        public async Task<int> NumberOfStudentPerClass(string className, string term, string session)
        {
            var studentPerClass = await _db.AssignedClasses.AsNoTracking().CountAsync(x => x.ClassName.ToUpper().Trim().Equals(className.ToUpper().Trim())
                                                                && x.TermName.ToUpper().Trim().Equals(term.ToUpper().Trim())
                                                             && x.SessionName.ToUpper().Trim().Equals(session.ToUpper().Trim()));
            return studentPerClass;
        }

        //public void FindAggregatePosition(string className, string term, string session)
        //{
        //    var myAggregatePosition = db.Results.Where(x => x.ClassName.Equals(className) &&
        //                                                             x.Term.Equals(term) &&
        //                                                             x.SessionName.Equals(session))
        //                                                        .OrderByDescending(y => y.AggretateScore);
        //}

        public async Task<double> SubjectHighest(string subject, string className, string term, string session)
        {

            var mySubjectHighest = await _db.ContinuousAssessments.AsNoTracking().Where(x => x.SubjectCode.ToUpper().Trim().Equals(subject.ToUpper().Trim())
                                                                    && x.ClassName.ToUpper().Trim().Equals(className.ToUpper().Trim())
                                                                     && x.TermName.ToUpper().Trim().Equals(term.ToUpper().Trim())
                                                             && x.SessionName.ToUpper().Trim().Equals(session.ToUpper().Trim()))
                                                                   .MaxAsync(i => i.Total);
            return mySubjectHighest;
        }

        public async Task<double> SubjectLowest(string subject, string className, string term, string session)
        {
            var mySubjectLowest = await _db.ContinuousAssessments.AsNoTracking().Where(x => x.SubjectCode.ToUpper().Trim().Equals(subject.ToUpper().Trim())
                                                                    && x.ClassName.ToUpper().Trim().Equals(className.ToUpper().Trim())
                                                                     && x.TermName.ToUpper().Trim().Equals(term.ToUpper().Trim())
                                                             && x.SessionName.ToUpper().Trim().Equals(session.ToUpper().Trim()))
                                                                   .MinAsync(i => i.Total);
            return mySubjectLowest;
        }

        public int FindSubjectPosition(string studentId, string subject, string className, string term, string session)
        {
            int subjectPosition = 0;
            var mySubjectPosition = _db.ContinuousAssessments.AsNoTracking().Where(x => x.SubjectCode.ToUpper().Trim().Equals(subject.ToUpper().Trim())
                                                                    && x.ClassName.ToUpper().Trim().Equals(className.ToUpper().Trim())
                                                                     && x.TermName.ToUpper().Trim().Equals(term.ToUpper().Trim())
                                                             && x.SessionName.ToUpper().Trim().Equals(session.ToUpper().Trim()));

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
            var resultPosition = _db.Results.AsNoTracking().Where(x => x.ClassName.ToUpper().Trim().Equals(className.ToUpper().Trim()) &&
                                                       x.Term.ToUpper().Trim().Equals(term.ToUpper().Trim()) &&
                                                       x.SessionName.ToUpper().Trim().Equals(session.ToUpper().Trim())
                                                       && x.SubjectName.Contains("Math"));
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

        public async Task<double> CalculateAverage(string studentId, string className, string term, string sessionName)
        {
            double scorePerstudent = await TotalScorePerStudent(studentId, className, term, sessionName);
            int subjectOffered = await SubjectOfferedByStudent(studentId, term, sessionName);
            return Math.Round((scorePerstudent / subjectOffered), 2);
        }

        public async Task<double> CalculateClassAverage(string className, string term, string sessionName, string subject)
        {
            var scorePerSubject = await TotalScorePerSubject(subject, className, term, sessionName);
            var studentInClass = await NumberOfStudentPerClass(className, term, sessionName);
            return Math.Round((scorePerSubject / studentInClass), 2);
        }

        //public async Task<double> TotalQualityPoint(string studentId, string className, string term, string sessionName)
        //{

        //    var totalPoint = await _db.ContinuousAssessments.AsNoTracking().Where(x => x.ClassName.ToUpper().Trim().Equals(className.ToUpper().Trim()) &&
        //                                                        x.TermName.ToUpper().Trim().Equals(term.ToUpper().Trim()) &&
        //                                                        x.SessionName.ToUpper().Trim().Equals(sessionName.ToUpper().Trim()) &&
        //                                                        x.StudentId.ToUpper().Trim().Equals(studentId.ToUpper().Trim()))
        //                                                        .SumAsync(c => c.QualityPoint);
        //    return totalPoint;
        //}

        public async Task<double> TotalcreditUnit(string className)
        {
            var totalCredit = await _db.AssignSubjects.AsNoTracking().CountAsync(s => s.ClassName.ToUpper().Trim().Equals(className.ToUpper().Trim()));
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

        //public virtual void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _db.Dispose();
        //    }
        //    Dispose(disposing);
        //}

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}