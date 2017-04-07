using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StAugustine.Models
{
    public class ContinuousAssessment
    {
        private readonly GradeRemark _myGradeRemark = new GradeRemark();
        public int ContinuousAssessmentId { get; set; }

        [Display(Name = "Student ID")]
        [Required(ErrorMessage = "Your Student ID Number is required")]
        [StringLength(15, ErrorMessage = "Your Student ID is too long")]
        public string StudentId { get; set; }

        [Display(Name = "Term")]
        [Required(ErrorMessage = "Term is required")]
        public string TermName { get; set; }

        [Display(Name = "Session")]
        [Required(ErrorMessage = "Session is required")]
        public string SessionName { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Subject is required")]
        public string SubjectCode { get; set; }

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name is required")]
        public string ClassName { get; set; }

        [Display(Name = "Score for Project")]
        [Required(ErrorMessage = "Assignment is required")]
        public double ProjectScore { get; set; }

        [Display(Name = "Score for Assignment")]
        [Required(ErrorMessage = "Assignment is required")]
        public double Assignment { get; set; }

        [Display(Name = "CA Score")]
        [Required(ErrorMessage = "CA score is required")]
        public double Test { get; set; }


        [Display(Name = "Exam Score")]
        [Required(ErrorMessage = "Exam Score is required")]
        public double ExamScore { get; set; }

        [Display(Name = "Staff Name")]
        [Required(ErrorMessage = "Staff name is required")]
        public string StaffName { get; set; }

        public virtual List<Subject> Subjects { get; set; }
        public virtual Result Result { get; set; }
        public virtual List<Session> Sessions { get; set; }

        public double Total
        {
            get
            {
                double sum = ProjectScore + Assignment + Test + ExamScore;
                return sum;
            }
            private set { }
        }

        public string Grading
        {
            #region Checking grade

            get
            {
                return _myGradeRemark.Grading(Total, ClassName);
            }
            private set { }

            #endregion

        }

        public string Remark
        {
            #region Checking grade

            get
            {
                return _myGradeRemark.Remark(Total, ClassName);
            }
            private set { }

            #endregion
        }

        public int GradePoint
        {
            #region Checking grade

            get
            {
                return _myGradeRemark.GradingPoint(Total, ClassName);
            }
            private set { }

            #endregion
        }

        public int QualityPoint
        {
            get
            {
                return 2 * GradePoint;
            }
            private set { }
        }
    }
}