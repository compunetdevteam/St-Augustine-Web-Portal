using System.ComponentModel.DataAnnotations;

namespace SwiftSkool.ViewModel
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


        [Display(Name = "Score for First Test")]
        //[Required(ErrorMessage = "First Test is required")]
        [Range(0, 10, ErrorMessage = "Enter number between 0 to 10")]
        public double FirstTest { get; set; }

        [Display(Name = "Score for Second Test")]
        //[Required(ErrorMessage = "Second Test is required")]
        [Range(0, 10, ErrorMessage = "Enter number between 0 to 10")]
        public double SecondTest { get; set; }

        [Display(Name = "Score Third Test")]
        //[Required(ErrorMessage = "Third Test score is required")]
        [Range(0, 10, ErrorMessage = "Enter number between 0 to 10")]
        public double ThirdTest { get; set; }


        [Display(Name = "Exam Score")]
        [Range(0, 70, ErrorMessage = "Enter number between 0 to 70")]
        //[Required(ErrorMessage = "Exam Score is required")]
        public double ExamScore { get; set; }

        [Display(Name = "Staff Name")]
        [Required(ErrorMessage = "Staff name is required")]
        public string StaffName { get; set; }
    }
}