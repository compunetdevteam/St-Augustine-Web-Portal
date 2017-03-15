using System.ComponentModel.DataAnnotations;

namespace StAugustine.Models.CBT
{
    public class QuestionAnswer
    {
        public int Id { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Subject Name is required")]
        public string SubjectName { get; set; }

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name is required")]
        public string ClassName { get; set; }

        [Display(Name = "Question")]
        [Required(ErrorMessage = "Question is required")]
        [DataType(DataType.MultilineText)]
        public string Question { get; set; }

        [Display(Name = "Option 1")]
        [Required(ErrorMessage = "Option 1 is required")]
        [DataType(DataType.MultilineText)]
        public string Option1 { get; set; }

        [Display(Name = "Option 2")]
        [Required(ErrorMessage = "Option 2 is required")]
        [DataType(DataType.MultilineText)]
        public string Option2 { get; set; }

        [Display(Name = "Option 3")]
        [Required(ErrorMessage = "Option 3 is required")]
        [DataType(DataType.MultilineText)]
        public string Option3 { get; set; }

        [Display(Name = "Option 4")]
        [Required(ErrorMessage = "Option 4 is required")]
        [DataType(DataType.MultilineText)]
        public string Option4 { get; set; }

        [Display(Name = "Answer")]
        [Required(ErrorMessage = "Answer is required")]
        [DataType(DataType.MultilineText)]
        public string Answer { get; set; }

        [Display(Name = "Question Hint")]
        public string QuestionHint { get; set; }
    }
}