using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HopeAcademySMS.Models
{
    public class Staff
    {
        public string Id { get; set; }

        [Display(Name = "Salutation")]
        public PopUp.Salutation Salutation { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = "Your Surname is required")]
        [StringLength(50, ErrorMessage = "Your Surname is too long")]
        public string Surname { get; set; }

        [Display(Name = "Other Names")]
        [Required(ErrorMessage = "Your Other Name is required")]
        [StringLength(50, ErrorMessage = "Your Other Name is too long")]
        public string OtherName { get; set; }
        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Enter the Correct Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Address")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Your Address is required")]
        [StringLength(50, ErrorMessage = "Your Address name is too long")]
        public string Address { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public string Username
        {
            get
            {
                return string.Format("{0} {1}", this.Surname, this.OtherName);
            }
        }
    }
}