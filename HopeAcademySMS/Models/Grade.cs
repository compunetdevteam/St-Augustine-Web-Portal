using System.ComponentModel.DataAnnotations;

namespace StAugustine.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public int MinimumValue { get; set; }
        public int MaximumValue { get; set; }
        public int GradePoint { get; set; }
        public string Remark { get; set; }
        public string PrincipalRemark { get; set; }
    }
}