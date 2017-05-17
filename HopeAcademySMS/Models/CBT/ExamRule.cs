using System.ComponentModel.DataAnnotations;

namespace SwiftSkool.Models.CBT
{
    public class ExamRule
    {
        public int Id { get; set; }

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name is required")]
        public string ClassName { get; set; }

        [Display(Name = "Score per Question")]
        [Required(ErrorMessage = "Minimum Value is required")]
        public int CorrectMark { get; set; }

        // public string WrongMark { get; set; }

        [Display(Name = "Total Question")]
        [Range(1, 100)]
        [Required(ErrorMessage = "Total Question is required")]
        public int TotalQuestion { get; set; }

        [Display(Name = "Maximum Exam Time")]
        [Required(ErrorMessage = "Maximum Exam Time is required")]
        public int MaximunTime { get; set; }
    }
}