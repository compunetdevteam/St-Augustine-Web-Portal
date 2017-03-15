using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StAugustine.Models
{
    public class ContinuousAssessment
    {
        GradeRemark myGradeRemark = new GradeRemark();
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

        [Display(Name = "Score for Assignment 1")]
        [Required(ErrorMessage = "Assignment is required")]
        public double Assignment1 { get; set; }

        [Display(Name = "Score for Assignment 2")]
        [Required(ErrorMessage = "Assignment is required")]
        public double Assignment2 { get; set; }

        [Display(Name = "First Test")]
        [Required(ErrorMessage = "First Test is required")]
        public double FirstTest { get; set; }

        [Display(Name = "Second Test")]
        [Required(ErrorMessage = "Second Test is required")]
        public double SecondTest { get; set; }

        [Display(Name = "Exam Score")]
        [Required(ErrorMessage = "Exam Score is required")]
        public double ExamScore { get; set; }
        [Display(Name = "Staff Name")]
        [Required(ErrorMessage = "Staff name is required")]
        public string StaffName { get; set; }

        public virtual Student Student { get; set; }

        public virtual List<Subject> Subjects { get; set; }
        public virtual Result Result { get; set; }
        public virtual List<Session> Sessions { get; set; }

        public double Total
        {
            get
            {
                double sum = Assignment1 + Assignment2 + FirstTest + SecondTest + ExamScore;
                return sum;
            }
            private set { }
        }

        public string Grading
        {
            #region Checking grade

            get
            {
                return myGradeRemark.Grading(Total);
            }
            private set { }

            #endregion

        }

        public string Remark
        {
            #region Checking grade

            get
            {
                return myGradeRemark.Remark(Total);
            }
            private set { }

            #endregion
        }

        public int GradePoint
        {
            #region Checking grade

            get
            {
                return myGradeRemark.GradingPoint(Total);
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