using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StAugustine.Models
{
    public class Student
    {

        private Student()
        {

        }

        public Student(string studentId, string guardianId, string firstname, string middleName, string lastname, string gender, DateTime dob,
            DateTime addmissionDate, byte[] passport)
        {
            if (!string.IsNullOrEmpty(studentId) && !string.IsNullOrEmpty(firstname) && !string.IsNullOrEmpty(lastname) && !string.IsNullOrEmpty(middleName)
                && !string.IsNullOrEmpty(gender) && !string.IsNullOrEmpty(guardianId))
            {
                StudentId = studentId;
                FirstName = firstname;
                MiddleName = middleName;
                LastName = lastname;
                Gender = gender;
                GuardianEmail = guardianId;
                StudentPassport = passport;
                DateOfBirth = dob;
                AdmissionDate = addmissionDate;
                var t = DateTime.Now - DateOfBirth;
                Age = (int)t.Days / 365;
            }
            else
            {
                throw new ArgumentException("Invalid values were provided to create student");
            }

            //GenerateAddmissionNumber();

        }

        public Student(string studentId, string guardianId, string firstname, string middleName, string lastname, string gender, DateTime dob,
           DateTime addmissionDate)
        {
            StudentId = studentId;
            FirstName = firstname;
            MiddleName = middleName;
            LastName = lastname;
            Gender = gender;
            GuardianEmail = guardianId;
            // StudentPassport = passport;
            DateOfBirth = dob;
            AdmissionDate = addmissionDate;
            var t = DateTime.Now - DateOfBirth;
            Age = (int)t.Days / 365;

        }

        //private void GenerateAddmissionNumber()
        //{
        //    throw new NotImplementedException();
        //}
        [Key]
        public string StudentId { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string MiddleName { get; private set; }

        public string GuardianEmail { get; private set; }

        public string FullName => string.Format($"{FirstName} {LastName}");

        public int Age { get; private set; }


        public DateTime DateOfBirth { get; private set; }

        public DateTime AdmissionDate { get; private set; }

        public string Gender { get; private set; }

        public byte[] StudentPassport { get; private set; }

        public bool Active { get; private set; }

        public virtual ICollection<Guardian> Guardians { get; set; }
        public virtual ICollection<FeePayment> FeePayments { get; set; }

        public virtual ICollection<ContinuousAssessment> ContinuousAssessments { get; set; }

        public virtual ICollection<AssignedClass> AssignedClasses { get; set; }
    }
}