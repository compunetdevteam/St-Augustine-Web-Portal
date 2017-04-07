using System.ComponentModel.DataAnnotations;

namespace HopeAcademySMS.ViewModel
{
    public class AssignSubjectTeacherVM
    {
        public int AssignTeacherToSubjectId { get; set; }

        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; }

        [Display(Name = "Class Name")]
        public string[] ClassName { get; set; }

        [Display(Name = "Staff Name")]
        public string StaffName { get; set; }

    }
}