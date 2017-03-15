using StAugustine.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;

namespace StAugustine.ViewModel
{
    public class StudentViewModel
    {
      
        public string GuardianId { get; set; }

        [Display(Name = "Student ID")]
        [Required(ErrorMessage = "Your Student ID Number is required")]
        [StringLength(15, ErrorMessage = "Your Student ID is too long")]
        public string StudentId { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Your First Name is required")]
        [StringLength(50, ErrorMessage = "Your First Name is too long")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Your Last Name is required")]
        [StringLength(50, ErrorMessage = "Your Last Name is too long")]
        public string LastName { get; set; }

        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Your Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public PopUp.Gender Gender { get; set; }

        [Display(Name = "Admission Date")]
        [Required(ErrorMessage = "Your Admission Date is required")]
        [DataType(DataType.Date)]
        public DateTime AdmissionDate { get; set; }

        public byte[] StudentPassport { get; private set; }

        [Display(Name = "Local file")]
        [NotMapped]
        public HttpPostedFileBase File
        {
            get
            {
                return null;
            }

            set
            {
                try
                {
                    MemoryStream target = new MemoryStream();

                    if (value.InputStream == null)
                        return;

                    value.InputStream.CopyTo(target);
                    StudentPassport = target.ToArray();
                }
                catch (Exception)
                {
                    //logger.Error(ex.Message);
                    //logger.Error(ex.StackTrace);
                }
            }
        }
    }
}