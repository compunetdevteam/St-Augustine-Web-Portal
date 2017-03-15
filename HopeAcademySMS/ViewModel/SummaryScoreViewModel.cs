using System.ComponentModel.DataAnnotations;

namespace StAugustine.ViewModel
{
    public class SummaryScoreViewModel
    {
        public int CombinedSubjectTotalId { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Subject is required")]
        public string SubjectCode { get; set; }

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name is required")]
        public string ClassName { get; set; }

        [Display(Name = "Session")]
        [Required(ErrorMessage = "Session is required")]
        public string SessionName { get; set; }




    }
}