using System.ComponentModel.DataAnnotations;

namespace StAugustine.Models
{
    public class Guardian
    {

        [Key]
        public string GuardianId { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string GuardianEmail { get; set; }
        public string Address { get; set; }
        public string Occupation { get; set; }
        public string Relationship { get; set; }

        public string UserName
        {
            get { return FirstName + " " + LastName; }
            set { }
        }

        public string Password { get; set; }

       // public string StudentId { get; set; }
        public virtual Student Student { get; set; }


    }
}