using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StAugustine.Models
{
    public class SchoolClass
    {
        public int Id { get; set; }

        [Display(Name = "School Class Code")]
        [Required(ErrorMessage = "School Class code is required")]
        [Index(IsUnique = true)]
        [MaxLength(20)]
        public string ClassCode { get; set; }


        [Display(Name = "School Class Name")]
        [Required(ErrorMessage = "School Class name is required")]
        public string ClassName { get; set; }
    }
}