using StAugustine.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace StAugustine.ViewModel
{
    public class GuardianViewModel
    {
        public string GuardianId { get; set; }

        //[Display(Name = "Student Id")]
        //[Required(ErrorMessage = "Your StudentId is required")]
        //[StringLength(40, ErrorMessage = "Your StudentId name is too long")]
        //public string StudentId { get; set; }

        public PopUp.Salutation Salutation { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Your Next of kin's First Name is required")]
        [StringLength(40, ErrorMessage = "Your Next of kin's First name is too long")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [Required(ErrorMessage = "Your Next of kin's Middle name is required")]
        [StringLength(40, ErrorMessage = "Your Next of kin's Middle name is too long")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Your Next of kin's last name is required")]
        [StringLength(40, ErrorMessage = "Your Next of kin's last name is too long")]
        public string LastName { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Your Gender is required")]
        public PopUp.Gender Gender { get; set; }

        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        //[Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Address")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Your Next of kin's Address is required")]
        [StringLength(50, ErrorMessage = "Your Next of kin's Address name is too long")]
        public string Address { get; set; }


        [Display(Name = "Occupation")]
        [Required(ErrorMessage = "Your Occupation is required")]
        public string Occupation { get; set; }
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Relationship")]
        [Required(ErrorMessage = "Your Relationship is required")]
        public PopUp.Relationship Relationship { get; set; }

        public string Username => FirstName + " " + LastName;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public virtual Student Student { get; set; }
    }
}