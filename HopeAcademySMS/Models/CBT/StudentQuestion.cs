namespace SwiftSkool.Models.CBT
{
    public class StudentQuestion
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string Question { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }

        public string Answer { get; set; }
        public string QuestionHint { get; set; }

        public int QuestionNumber { get; set; }
        public bool IsCorrect { get; set; }


    }
}