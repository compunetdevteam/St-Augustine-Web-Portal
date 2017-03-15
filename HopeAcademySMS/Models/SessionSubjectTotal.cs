using System;
using System.Linq;

namespace StAugustine.Models
{
    public class SessionSubjectTotal
    {
        private GradeRemark myGradeRemark = new GradeRemark();
        private ApplicationDbContext db = new ApplicationDbContext();
        private SessionSubjectTotal()
        {

        }

        public SessionSubjectTotal(string studentId, string className, string sessionName, string subjectCode)
        {
            if (!string.IsNullOrEmpty(className) && !string.IsNullOrEmpty(sessionName)
                            && !string.IsNullOrEmpty(subjectCode))
            {
                StudentId = studentId;
                ClassName = className;
                SessionName = sessionName;
                FirstTermScore = Math.Round(GetFirstTermScoreForSubject(studentId, sessionName, subjectCode, className), 2);
                SecondTermScore = Math.Round(GetSecondTermScoreForSubject(studentId, sessionName, subjectCode, className), 2);
                ThirdTermScore = Math.Round(GetThirdTermScoreForSubject(studentId, sessionName, subjectCode, className), 2);
            }
            else
            {
                throw new ArgumentException("Invalid parameter supplied, make sure required parameters are not empty!");
            }
        }
        public int SessionSubjectTotalId { get; set; }
        public string StudentId { get; private set; }
        public string ClassName { get; private set; }
        public string SubjectName { get; private set; }
        public string SessionName { get; private set; }
        public double FirstTermScore { get; private set; }
        public double SecondTermScore { get; private set; }
        public double ThirdTermScore { get; private set; }

        public double SummaryTotal
        {
            get
            {
                double firstTerm = 0.3 * FirstTermScore;
                double secondTerm = 0.3 * SecondTermScore;
                double thirdTerm = 0.4 * ThirdTermScore;
                return Math.Round((firstTerm + secondTerm + thirdTerm), 2);
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


        private double GetFirstTermScoreForSubject(string studentId, string sessionName, string subjectCode, string className)
        {
            var firstTermScore = db.ContinuousAssessments.Where(x => x.StudentId.Equals(studentId)
                                                                     && x.TermName.Equals("First")
                                                                     && x.SessionName.Equals(sessionName)
                                                                     && x.ClassName.Equals(className));
            var newSubjectName = db.Subjects.Where(x => x.CourseName.Equals(subjectCode))
                .Select(c => c.CourseName).FirstOrDefault();

            //var mysubjectCategory = db.Subjects.Where(x => x.CourseName.Equals(subjectCode))
            //    .Select(c => c.CategoriesId).FirstOrDefault();

            //if (mysubjectCategory == "Mathematics")
            //{
            //    SubjectName = "Mathematics";
            //}
            //else if (mysubjectCategory == "English")
            //{
            //    SubjectName = "English";
            //}
            //else
            //{
            //    SubjectName = newSubjectName;
            //}
            //var subjectCategory = firstTermScore.Where(x => x.SubjectCategory.Equals(mysubjectCategory));
            //var countSubjectCategory = subjectCategory.Count();
            //double myTotal = 0;
            //if (countSubjectCategory > 1)
            //{
            //    foreach (var item in subjectCategory)
            //    {
            //        myTotal += item.Total;
            //    }
            //    return myTotal / countSubjectCategory;

            //}
            //else
            //{
            var myFirstScore = firstTermScore.Where(x => x.SubjectCode.Equals(subjectCode))
                .Select(y => y.Total).FirstOrDefault();

            return myFirstScore;
            //}
        }


        private double GetSecondTermScoreForSubject(string studentId, string sessionName, string subjectCode,
            string className)
        {
            var secondTermScore = db.ContinuousAssessments.Where(x => x.StudentId.Equals(studentId)
                                                                      && x.TermName.Equals("Second")
                                                                      && x.SessionName.Equals(sessionName)
                                                                      && x.ClassName.Equals(className));
            var newSubjectName = db.Subjects.Where(x => x.CourseName.Equals(subjectCode))
                .Select(c => c.CourseName).FirstOrDefault();

            //var mysubjectCategory = db.Subjects.Where(x => x.CourseName.Equals(subjectCode))
            //    .Select(c => c.CategoriesId).FirstOrDefault();

            //if (mysubjectCategory == "Mathematics")
            //{
            //    SubjectName = "Mathematics";
            //}
            //else if (mysubjectCategory == "English")
            //{
            //    SubjectName = "English";
            //}
            //else
            //{
            //    SubjectName = newSubjectName;
            //}
            //var subjectCategory = secondTermScore.Where(x => x.SubjectCategory.Equals(mysubjectCategory));
            //var countSubjectCategory = subjectCategory.Count();
            //double myTotal = 0;
            //if (countSubjectCategory > 1)
            //{
            //    foreach (var item in subjectCategory)
            //    {
            //        myTotal += item.Total;
            //    }
            //    return myTotal / countSubjectCategory;

            //}
            //else
            //{
            var mySecondScore = secondTermScore.Where(x => x.SubjectCode.Equals(subjectCode))
                .Select(y => y.Total).FirstOrDefault();

            return mySecondScore;
            // }


        }


        private double GetThirdTermScoreForSubject(string studentId, string sessionName, string subjectCode,
            string className)
        {

            var secondTermScore = db.ContinuousAssessments.Where(x => x.StudentId.Equals(studentId)
                                                                      && x.TermName.Equals("Third")
                                                                      && x.SessionName.Equals(sessionName)
                                                                      && x.ClassName.Equals(className));

            var newSubjectName = db.Subjects.Where(x => x.CourseName.Equals(subjectCode))
              .Select(c => c.CourseName).FirstOrDefault();

            //var mysubjectCategory = db.Subjects.Where(x => x.CourseName.Equals(subjectCode))
            //    .Select(c => c.CategoriesId).FirstOrDefault();
            //var subjectCategory = secondTermScore.Where(x => x.SubjectCategory.Equals(mysubjectCategory));

            //if (mysubjectCategory == "Mathematics")
            //{
            //    SubjectName = "Mathematics";
            //}
            //else if (mysubjectCategory == "English")
            //{
            //    SubjectName = "English";
            //}
            //else
            //{
            //    SubjectName = newSubjectName;
            //}
            //var countSubjectCategory = subjectCategory.Count();
            //double myTotal = 0;
            //if (countSubjectCategory > 1)
            //{
            //    foreach (var item in subjectCategory)
            //    {
            //        myTotal += item.Total;
            //    }
            //    return myTotal / countSubjectCategory;

            //}
            //else
            //{
            var mySecondScore = secondTermScore.Where(x => x.SubjectCode.Equals(subjectCode))
                .Select(y => y.Total).FirstOrDefault();

            return mySecondScore;

            // }

        }
    }
}