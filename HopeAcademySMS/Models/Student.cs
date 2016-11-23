using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HopeAcademySMS.Models
{
    public class Student
    {
        public string ID { get; set; }

        [Key]
        [Display(Name = "Student ID")]
        [Required(ErrorMessage = "Your Student ID Number is required")]
        [StringLength(10, ErrorMessage = "Your Student ID is too long")]
        public string StudentNumber { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Your First Name is required")]
        [StringLength(50, ErrorMessage = "Your First Name is too long")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [Required(ErrorMessage = "Your Middle Name is required")]
        [StringLength(50, ErrorMessage = "Your Middle Name is too long")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Your Last Name is required")]
        [StringLength(50, ErrorMessage = "Your Last Name is too long")]
        public string LastName { get; set; }     

        public string Username
        {
            get
            {
                return string.Format("{0} {1}", this.LastName, this.FirstName);
            }
        }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        [Display(Name = "Current Address")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Your Current Address is required")]
        [StringLength(150, ErrorMessage = "Your Current Address name is too long")]
        public string Address { get; set; }

        [Display(Name = "Current Class")]
        public string Class { get; set; }

        public virtual List<Course> Courses { get; set; }

        public virtual List<Grade> Grades { get; set; }

        public virtual List<ParentGuardianInfo> ParentGuardianInfos { get; set; }

        //public virtual List<SavingsMaintenance> SavingsMaintenances { get; set; }

        //public virtual List<LoanRequest> LoanRequests { get; set; }
    }
}