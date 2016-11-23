using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HopeAcademySMS.Models
{
    public class ParentGuardianInfo
    {
        public int Id { get; set; }        

        [Display(Name = "Student's Name")]
        [Required(ErrorMessage = "Student's Name is required")]
        public string StudentNumber { get; set; }

        public PopUp.Salutation Salutation { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Your Next of kin's First Name is required")]
        [StringLength(40, ErrorMessage = "Your Next of kin's First name is too long")]
        public string NOK_FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [Required(ErrorMessage = "Your Next of kin's Middle name is required")]
        [StringLength(40, ErrorMessage = "Your Next of kin's Middle name is too long")]
        public string NOK_MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Your Next of kin's last name is required")]
        [StringLength(40, ErrorMessage = "Your Next of kin's last name is too long")]
        public string NOK_LastName { get; set; }

        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        public string NOK_PhoneNumber { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Address")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Your Next of kin's Address is required")]
        [StringLength(50, ErrorMessage = "Your Next of kin's Address name is too long")]
        public string NOK_Address { get; set; }

        [Display(Name = "Relationship")]
        [Required(ErrorMessage = "Your Next of kin's Relationship is required")]
        public PopUp.Relationship NOK_Relationship { get; set; }

        public virtual Student Student { get; set; }
    }
}