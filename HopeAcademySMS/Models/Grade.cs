using System;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkool.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public int MinimumValue { get; set; }
        public int MaximumValue { get; set; }
        //public int GradePoint { get; set; }
        public string Remark { get; set; }
        // public string ClassName { get; set; }

    }

    public class PrincipalComment
    {
        public int Id { get; set; }

        [Display(Name = "Minimum Grade")]
        [Required(ErrorMessage = "Minimum Grade is required")]
        [Range(1, 100, ErrorMessage = "Value must be between 1.0 and 100.0")]
        public double MinimumGrade { get; set; }

        [Display(Name = "Maximum Grade")]
        [Required(ErrorMessage = "Maximum Grade is required")]
        [Range(1, 100, ErrorMessage = "Value must be between 1.0 and 100.0")]
        public double MaximumGrade { get; set; }

        [Display(Name = "Principal Remark")]
        [Required(ErrorMessage = "Principal Remark is required")]
        public string Remark { get; set; }

        public string ClassName { get; set; }

    }

    public class PrincipalCommentVm
    {
        public int Id { get; set; }

        [Display(Name = "Minimum Grade")]
        [Required(ErrorMessage = "Minimum Grade is required")]
        [Range(1, 100, ErrorMessage = "Value must be between 1.0 and 100.0")]
        public double MinimumGrade { get; set; }

        [Display(Name = "Maximum Grade")]
        [Required(ErrorMessage = "Maximum Grade is required")]
        [Range(1, 100, ErrorMessage = "Value must be between 1.0 and 100.0")]
        public double MaximumGrade { get; set; }

        [Display(Name = "Principal Remark")]
        [Required(ErrorMessage = "Principal Remark is required")]
        public string Remark { get; set; }

        public string[] ClassName { get; set; }

    }

    public class TeacherComment
    {
        public int TeacherCommentId { get; set; }

        [Display(Name = "Student Id")]
        [Required(ErrorMessage = "Student Id is required")]
        public string StudentId { get; set; }


        [Display(Name = "Term")]
        [Required(ErrorMessage = "Term is required")]
        public string TermName { get; set; }

        [Display(Name = "Session")]
        [Required(ErrorMessage = "Session is required")]
        public string SessionName { get; set; }


        [Display(Name = "Teacher's Remark")]
        [Required(ErrorMessage = "Teacher's remark is required")]
        public string Remark { get; set; }

        public DateTime Date { get; set; }

    }
}