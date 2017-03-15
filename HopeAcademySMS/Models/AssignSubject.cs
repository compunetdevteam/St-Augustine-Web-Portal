using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StAugustine.Models
{
    public class AssignSubject
    {
        public int AssignSubjectId { get; set; }

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name cannot be empty")]
        public string ClassName { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Subject Name cannot be empty")]
        public string SubjectName { get; set; }

        public virtual ICollection<Class> Classes { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }

    }
}