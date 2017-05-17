using System.ComponentModel.DataAnnotations;

namespace SwiftSkool.ViewModel
{
    public class AssignSubjectViewModel
    {
        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name cannot be empty")]
        public string ClassName { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Subject Name cannot be empty")]
        public string[] SubjectName { get; set; }


    }
}