using System.ComponentModel.DataAnnotations;

namespace HopeAcademySMS.Models
{
    public class SubjectRegistration
    {
        public int Id { get; set; }

        [Display(Name = "Student Number")]
        [Required(ErrorMessage = "Student Number cannot be empty")]
        public string StudentId { get; set; }

        [Display(Name = "Student Name")]
        [Required(ErrorMessage = "Student Name cannot be empty")]
        public string StudentName { get; set; }

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name cannot be empty")]
        public string ClassName { get; set; }

        [Display(Name = "Term Name")]
        [Required(ErrorMessage = "Term Name cannot be empty")]
        public string TermName { get; set; }

        [Display(Name = "Session Name")]
        [Required(ErrorMessage = "Session Name cannot be empty")]
        public string SessionName { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Subject Name cannot be empty")]
        public string SubjectCode { get; set; }
    }

    public class SubjectRegistrationVm
    {
        [Display(Name = "Student Number")]
        [Required(ErrorMessage = "Student Number cannot be empty")]
        public string StudentId { get; set; }

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name cannot be empty")]
        public string ClassName { get; set; }

        [Display(Name = "Term Name")]
        [Required(ErrorMessage = "Term Name cannot be empty")]
        public string TermName { get; set; }

        [Display(Name = "Session Name")]
        [Required(ErrorMessage = "Session Name cannot be empty")]
        public string SessionName { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Subject Name cannot be empty")]
        public string[] SubjectName { get; set; }

    }
}