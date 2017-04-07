namespace StAugustine.Models
{
    public class AssignFormTeacherToClass
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public string Username { get; set; }
    }

    public class AssignSubjectTeacher
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public string ClassName { get; set; }
        public string StaffName { get; set; }
    }
}