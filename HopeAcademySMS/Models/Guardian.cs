using System;
using System.ComponentModel.DataAnnotations;

namespace StAugustine.Models
{
    public class Guardian
    {
        private Guardian()
        {

        }

        public Guardian(string id, string salutation, string firstname, string middleName, string lastname,
                        string gender, string address, string phonenumber, string email, string relationship, string occupation)
        {
            ValidateGuardian(id, salutation, firstname, middleName,
                        lastname, gender, phonenumber, address, email, relationship, occupation);
        }

       
        public string GuardianId { get; private set; }
        public string Salutation { get; private set; }
        public string FirstName { get; private set; }
        public string MiddleName { get; private set; }
        public string LastName { get; private set; }
        public string Gender { get; private set; }
        public string FullName => FirstName + " " + MiddleName + " " + LastName;
        public string PhoneNumber { get; private set; }
        [Key]
        public string GuardianEmail { get; private set; }
        public string Address { get; private set; }
        public string Occupation { get; private set; }
        public string Relationship { get; private set; }

        public virtual Student Student { get; set; }

        private void ValidateGuardian(string id, string salutation, string firstname, string middleName, string lastname,
                        string gender, string address, string phonenumber, string email, string relationship, string occupation)
        {
            validatestring(id);
            validatestring(salutation);
            validatestring(firstname);
            validatestring(middleName);
            validatestring(lastname);
            validatestring(address);
            validatestring(phonenumber);
            validatestring(email);
            validatestring(relationship);
            validatestring(occupation);

            GuardianId = id;
            Salutation = salutation;
            FirstName = firstname;
            MiddleName = middleName;
            LastName = lastname;
            Address = address;
            PhoneNumber = phonenumber;
            GuardianEmail = email;
            Relationship = relationship;
            Occupation = occupation;

        }

        private void validatestring(string astring)
        {
            if (string.IsNullOrWhiteSpace(astring))
                throw new ArgumentException("Invalid Arguments are not allowed!");
            return;
        }
    }
}