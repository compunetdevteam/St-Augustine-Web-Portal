using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftSkool.Models
{
    public class Term
    {
        [Key]
        public int TermId { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(20)]
        public string TermName { get; set; }

        [Display(Name = "Current Term")]
        public bool ActiveTerm { get; set; }
    }
}