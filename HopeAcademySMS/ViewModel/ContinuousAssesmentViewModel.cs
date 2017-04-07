using System.ComponentModel.DataAnnotations;

namespace StAugustine.ViewModel
{
    public class ContinuousAssesmentViewModel
    {
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

        public string SubjectCategory { get; set; }

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name is required")]
        public string ClassName { get; set; }

        [Display(Name = "Score for Project")]
        [Range(1, 10)]
        [Required(ErrorMessage = "Project score is required")]
        public double ProjectScore { get; set; }

        [Display(Name = "Score for Assignment")]
        [Range(1, 10)]
        [Required(ErrorMessage = "Assignment is required")]
        public double Assignment { get; set; }

        [Display(Name = "CA Score")]
        [Range(1, 20)]
        [Required(ErrorMessage = "CA Score is required")]
        public double Test { get; set; }


        [Display(Name = "Exam Score")]
        [Range(1, 60)]
        [Required(ErrorMessage = "Exam Score is required")]
        public double ExamScore { get; set; }
        [Display(Name = "Staff Name")]
        [Required(ErrorMessage = "Staff name is required")]
        public string StaffName { get; set; }
    }
}