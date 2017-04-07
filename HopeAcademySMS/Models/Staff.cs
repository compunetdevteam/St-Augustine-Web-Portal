using System;
using System.Collections.Generic;

namespace StAugustine.Models
{
    public class Staff
    {
        public string Id { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        //[Index(IsUnique = true)]
        //[MaxLength(20)]
        public string PhoneNumber { get; set; }

        //[Index(IsUnique = true)]
        //[MaxLength(20)]
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string StateOfOrigin { get; set; }
        public string Designation { get; set; }
        public byte[] StaffPassport { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string Qualifications { get; set; }
        public string Password { get; set; }
        public string Username => $"{this.FirstName} {this.LastName}";

        public virtual ICollection<AssignedClass> AssignedClasses { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
    }
}