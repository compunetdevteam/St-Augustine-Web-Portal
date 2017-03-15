using System;
using System.Linq;

namespace StAugustine.Models
{
    public class ReportSummary
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private GradeRemark myGradeRemark = new GradeRemark();
        private ReportSummary()
        {

        }

        public ReportSummary(string studentId, string className, string sessionName, string subjectCode)
        {
            if (!string.IsNullOrEmpty(studentId) && !string.IsNullOrEmpty(className)
                        && !string.IsNullOrEmpty(sessionName) && !string.IsNullOrEmpty(subjectCode))
            {
                StudentId = studentId;
                ClassName = className;
                SessionName = sessionName;
                SubjectName = subjectCode;
                FirstTermScore = GetFirstTermScore(studentId, sessionName, subjectCode);
                FirstTermSubjectGrade = myGradeRemark.Grading(FirstTermScore);

                SecondTermScore = GetSecondTermScore(studentId, sessionName, subjectCode);
                SecondTermSubjectGrade = myGradeRemark.Grading(SecondTermScore);

                ThirdTermScore = GetThirdTermScore(studentId, sessionName, subjectCode);
                ThirdTermSubjectGrade = myGradeRemark.Grading(ThirdTermScore);

                FindSubjectPositionForFirstTerm(studentId, subjectCode, className, sessionName);
                FindSubjectPositionForSecondTerm(studentId, subjectCode, className, sessionName);
                FindSubjectPositionForThirdTerm(studentId, subjectCode, className, sessionName);

                TotalScorePerStudent = SummaryTotalScorePerStudent(studentId, className, sessionName);
                NoOfStudentPerClass = NumberOfStudentPerClass(className, sessionName);
                NoOfSubjectOffered = SubjectOfferedByStudent(studentId);


                ClassAverage = Math.Round(CalculateAverage(studentId, className, sessionName), 2);
                //Average = Math.Round(CalculateAverage(studentId, className, term, sessionName), 2);
                //AggretateScore = Math.Round(SummaryTotalScorePerStudent(studentId, className, term, sessionName), 2);

                // NoOfSubjectOffered = SubjectOfferedByStudent(studentId);
                //FindSubjectPosition(studentId, subject, className, term, sessionName);
                //FindAggregatePosition(studentId, className, term, sessionName);
            }
            else
            {

            }

        }

        public void UpdateResultSummary(string studentId, string className, string sessionName, string subjectCode)
        {
            if (!string.IsNullOrEmpty(studentId) && !string.IsNullOrEmpty(className)
                        && !string.IsNullOrEmpty(sessionName) && !string.IsNullOrEmpty(subjectCode))
            {
                StudentId = studentId;
                ClassName = className;
                SubjectName = subjectCode;
                SessionName = sessionName;

                FirstTermScore = GetFirstTermScore(studentId, sessionName, subjectCode);
                FirstTermSubjectGrade = myGradeRemark.Grading(FirstTermScore);

                SecondTermScore = GetSecondTermScore(studentId, sessionName, subjectCode);
                SecondTermSubjectGrade = myGradeRemark.Grading(SecondTermScore);

                ThirdTermScore = GetThirdTermScore(studentId, sessionName, subjectCode);
                ThirdTermSubjectGrade = myGradeRemark.Grading(ThirdTermScore);

                FindSubjectPositionForFirstTerm(studentId, subjectCode, className, sessionName);
                FindSubjectPositionForSecondTerm(studentId, subjectCode, className, sessionName);
                FindSubjectPositionForThirdTerm(studentId, subjectCode, className, sessionName);
            }
            else
            {

            }

        }

        private double CalculateAverage(string studentId, string className, string sessionName)
        {
            double scorePerstudent = SummaryTotalScorePerStudent(studentId, className, sessionName);
            int subjectOffered = SubjectOfferedByStudent(studentId);
            return scorePerstudent / subjectOffered;
        }

        //private double CalculateClassAverage(string className, string term, string sessionName, string subject)
        //{
        //    var scorePerSubject = SummaryTotalScorePerSubject(subject, className, term, sessionName);
        //    var studentInClass = NumberOfStudentPerClass(className, term, sessionName, subject);
        //    return scorePerSubject / studentInClass;
        //}

        public int ReportSummaryId { get; set; }

        public string StudentId { get; private set; }
        public string SubjectName { get; private set; }
        public string ClassName { get; private set; }
        public string SessionName { get; private set; }

        public double FirstTermScore { get; private set; }
        public int FirstTermSubjectPosition { get; private set; }
        public string FirstTermSubjectGrade { get; private set; }

        public double SecondTermScore { get; private set; }
        public int SecondTermSubjectPosition { get; private set; }
        public string SecondTermSubjectGrade { get; private set; }

        public double ThirdTermScore { get; private set; }
        public int ThirdTermSubjectPosition { get; private set; }
        public string ThirdTermSubjectGrade { get; private set; }

        public double SummaryTotal
        {
            get
            {
                double firstTerm = 0.3 * FirstTermScore;
                double secondTerm = 0.3 * SecondTermScore;
                double thirdTerm = 0.4 * ThirdTermScore;
                return firstTerm + secondTerm + thirdTerm;
            }
            private set { }
        }

        public int WeightedScores
        {
            get
            {

                return Convert.ToInt32(FirstTermScore + SecondTermScore + ThirdTermScore);
            }
            private set { }

        }

        public string SummaryGrading
        {
            get
            {
                return myGradeRemark.Grading(SummaryTotal);
            }
            private set { }
        }


        public string SummaryRemark
        {
            get
            {
                return myGradeRemark.Remark(SummaryTotal);
            }
            private set { }
        }

        public double TotalScorePerStudent { get; private set; }

        public int SummaryPosition { get; private set; }

        public int NoOfSubjectOffered { get; private set; }

        public int NoOfStudentPerClass { get; private set; }
        public double SummaryAverage { get; private set; }

        public double ClassAverage { get; private set; }


        #region Getting score for each Term
        private double GetFirstTermScore(string studentId, string sessionName, string subjectCode)
        {

            var firstTermScore = db.SessionSubjectTotals.Where(x => x.StudentId.Equals(studentId)
                                                                    && x.SessionName.Equals(sessionName)
                                                                    && x.SubjectName.Equals(subjectCode))
                                                                    .Select(c => c.FirstTermScore).FirstOrDefault();

            return firstTermScore;
        }

        private double GetSecondTermScore(string studentId, string sessionName, string subjectCode)
        {
            var secondTermScore = db.SessionSubjectTotals.Where(x => x.StudentId.Equals(studentId)
                                                                    && x.SessionName.Equals(sessionName)
                                                                    && x.SubjectName.Equals(subjectCode))
                                                                    .Select(c => c.SecondTermScore).FirstOrDefault();

            return secondTermScore;
        }

        private double GetThirdTermScore(string studentId, string sessionName, string subjectCode)
        {

            var thirdTermScore = db.SessionSubjectTotals.Where(x => x.StudentId.Equals(studentId)
                                                                    && x.SessionName.Equals(sessionName)
                                                                    && x.SubjectName.Equals(subjectCode))
                                                                    .Select(c => c.ThirdTermScore).FirstOrDefault();

            return thirdTermScore;

        }
        #endregion


        private double SummaryTotalScorePerStudent(string studentId, string className, string session)
        {
            var summaryTotalSum = db.SessionSubjectTotals.Where(x => x.StudentId.Equals(studentId) && x.ClassName.Equals(className)
                                                               && x.SessionName.Equals(session))
                                                               .Sum(y => y.WeightedScores);
            return summaryTotalSum;
        }

        private int NumberOfStudentPerClass(string className, string session)
        {
            var studentPerClass = db.AssignedClasses.Count(x => x.ClassName.Equals(className) &&
                                                                x.TermName.Equals("Third") &&
                                                                x.SessionName.Equals(session));
            return studentPerClass;
        }

        private int SubjectOfferedByStudent(string studentId)
        {
            var className = db.AssignedClasses.Where(x => x.StudentId.Equals(studentId) && x.TermName.Equals("Third"))
                                .Select(y => y.ClassName)
                                .FirstOrDefault();


            var subjectPerStudent = db.AssignSubjects.Count(x => x.ClassName.Equals(className));
            return subjectPerStudent;
        }

        #region CommentedCode


        //private int SubjectOfferedByStudent(string studentId)
        //{
        //    var className =
        //        db.AssignedClasses.Where(x => x.StudentId.Equals(studentId))
        //            .Select(y => y.ClassName)
        //            .FirstOrDefault();


        //    var subjectPerStudent = db.AssignSubjects.Count(x => x.ClassName.Equals(className));
        //    return subjectPerStudent;
        //}

        //private double SummaryTotalScorePerSubject(string subject, string className, string term, string session)
        //{
        //    var sumPerSubject = db.ContinuousAssessments.Where(x => x.SubjectCode.Equals(subject)
        //                                                            && x.ClassName.Equals(className)
        //                                                            && x.TermName.Equals(term) &&
        //                                                            x.SessionName.Equals(session)).Sum(y => y.SummaryTotal);
        //    return sumPerSubject;
        //}


        //private int NumberOfStudentPerClass(string className, string term, string session, string subject)
        //{
        //    var studentPerClass = db.ContinuousAssessments.Count(x => x.ClassName.Equals(className) &&
        //                                                        x.TermName.Equals(term) &&
        //                                                        x.SessionName.Equals(session) &&
        //                                                        x.SubjectCode.Equals(subject));
        //    return studentPerClass;
        //}
        //private int NumberOfStudentPerClass(string className, string term, string session)
        //{
        //    var studentPerClass = db.AssignedClasses.Count(x => x.ClassName.Equals(className) &&
        //                                                        x.TermName.Equals(term) &&
        //                                                        x.SessionName.Equals(session));
        //    return studentPerClass;
        //}

        //public void FindAggregatePosition(string className, string term, string session)
        //{
        //    var myAggregatePosition = db.Results.Where(x => x.ClassName.Equals(className) &&
        //                                                             x.Term.Equals(term) &&
        //                                                             x.SessionName.Equals(session))
        //                                                        .OrderByDescending(y => y.AggretateScore);
        //} 
        #endregion

        #region Subjects positions
        private void FindSubjectPositionForFirstTerm(string studentId, string subject, string className, string session)
        {

            var mySubjectPosition = db.SessionSubjectTotals.Where(x => x.SubjectName.Equals(subject) &&
                                                                            x.ClassName.Equals(className) &&
                                                                            x.SessionName.Equals(session));
            //.OrderByDescending(y => y.FirstTermScore);
            var q = from s in mySubjectPosition
                    orderby s.FirstTermScore descending
                    select new
                    {
                        Name = s.StudentId,
                        Rank = (from o in mySubjectPosition
                                where o.FirstTermScore > s.FirstTermScore
                                select o).Count() + 1
                    };

            foreach (var item in q.Where(s => s.Name.Equals(studentId)))
            {
                FirstTermSubjectPosition = item.Rank;
            }

        }

        private void FindSubjectPositionForSecondTerm(string studentId, string subject, string className, string session)
        {
            var mySubjectPosition = db.SessionSubjectTotals.Where(x => x.SubjectName.Equals(subject) &&
                                                                        x.ClassName.Equals(className) &&
                                                                        x.SessionName.Equals(session));
            //.OrderByDescending(y => y.SecondTermScore);
            var q = from s in mySubjectPosition
                    orderby s.SecondTermScore descending
                    select new
                    {
                        Name = s.StudentId,
                        Rank = (from o in mySubjectPosition
                                where o.SecondTermScore > s.SecondTermScore
                                select o).Count() + 1
                    };

            foreach (var item in q.Where(s => s.Name.Equals(studentId)))
            {
                SecondTermSubjectPosition = item.Rank;
            }
        }

        private void FindSubjectPositionForThirdTerm(string studentId, string subject, string className, string session)
        {
            var mySubjectPosition = db.SessionSubjectTotals.Where(x => x.SubjectName.Equals(subject) &&
                                                                        x.ClassName.Equals(className) &&
                                                                        x.SessionName.Equals(session));
            // .OrderByDescending(y => y.ThirdTermScore);
            var q = from s in mySubjectPosition
                    orderby s.ThirdTermScore descending
                    select new
                    {
                        Name = s.StudentId,
                        Rank = (from o in mySubjectPosition
                                where o.ThirdTermScore > s.ThirdTermScore
                                select o).Count() + 1
                    };

            foreach (var item in q.Where(s => s.Name.Equals(studentId)))
            {
                ThirdTermSubjectPosition = item.Rank;
            }
        }

        #endregion
        private void FindAggregatePosition(string studentId, string className, string term, string session)
        {
            var resultPosition = db.ReportSummarys.Where(x => x.StudentId.Equals(studentId)
                                                                && x.ClassName.Equals(className)
                                                                && x.SessionName.Equals(session));
            //.OrderByDescending(y => y.WeightedScores);
            var q = from s in resultPosition
                    orderby s.WeightedScores descending
                    select new
                    {
                        Name = s.StudentId,
                        Rank = (from o in resultPosition
                                where o.WeightedScores > s.WeightedScores
                                select o).Count() + 1
                    };

            foreach (var item in q.Where(s => s.Name.Equals(studentId)))
            {
                ThirdTermSubjectPosition = item.Rank;
            }
        }

        //public async Task<ActionResult> SubjectPostion(string CourseName, string level, string term, string session)
        //{
        //    var grades = db.Grades.Include(g => g.Course).Include(g => g.Student)
        //                    .Where(s => s.CourseCode.Equals(CourseName) && s.Student.MyClassName.Equals(level)
        //                        && s.TermName.Equals(term) && s.SessionName.Equals(session))
        //                    .OrderByDescending(s => s.SummaryTotal);
        //    if (grades == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    int myposition = 0;
        //    double? mySummaryTotal = 0;
        //    foreach (var item in grades)
        //    {
        //        Grade grade = db.Grades.Find(item.GradeID);
        //        {
        //            if (mySummaryTotal == item.SummaryTotal)
        //            {
        //                grade.SubjectPosition = myposition;
        //            }
        //            else
        //            {
        //                grade.SubjectPosition = ++myposition;
        //            }
        //            mySummaryTotal = item.SummaryTotal;
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