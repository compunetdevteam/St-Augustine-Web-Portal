namespace StAugustine.Models
{
    public class Affective
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string TermName { get; set; }
        public string SessionName { get; set; }
        public string ClassName { get; set; }
        public int Honesty { get; set; }
        public int SelfConfidence { get; set; }
        public int Sociability { get; set; }
        public int Punctuality { get; set; }
        public int Neatness { get; set; }

        public int Initiative { get; set; }
        public int Organization { get; set; }
        public int AttendanceInClass { get; set; }

        public int HonestyAndReliability { get; set; }
        //public virtual ICollection<R>
    }

    public class Psychomotor
    {

        public int Id { get; set; }
        public string StudentId { get; set; }
        public string TermName { get; set; }
        public string SessionName { get; set; }
        public string ClassName { get; set; }
        public int Sports { get; set; }
        public int ExtraCurricularActivity { get; set; }
        public int Assignment { get; set; }
        public int HelpingOthers { get; set; }
        public int ManualDuty { get; set; }
        public int LevelOfCommitment { get; set; }
    }



}