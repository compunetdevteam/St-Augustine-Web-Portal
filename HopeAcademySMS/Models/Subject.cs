using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftSkool.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }

        [Display(Name = "Subject Code")]
        [Required(ErrorMessage = "Subject Code is required")]
        [Index(IsUnique = true)]
        [MaxLength(20)]
        public string CourseCode { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Subject Name is required")]
        [StringLength(50, ErrorMessage = "Subject Name is too long")]
        public string CourseName { get; set; }

        //[DisplayName("Subject Unit")]
        //[Range(1, 2)]
        //public int SubjectUnit { get; set; }

        public virtual ContinuousAssessment ContinuousAssessment { get; set; }

    }
}