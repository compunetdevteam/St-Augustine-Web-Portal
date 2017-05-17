using System.ComponentModel.DataAnnotations;

namespace SwiftSkool.Models
{
    public class PopUp
    {
        public enum Salutation
        {
            Dr = 1, Nurse, Mr, Mrs, Miss, Engr, Pastor
        }
        public enum Relationship
        {
            Father = 1, Mother, Sister, Brother, Friend, Others
        }
        public enum Gender
        {
            Male = 1, Female
        }
        public enum ThemeColor
        {
            DeepRed = 1, LightBlue, NavyBlue, ArmyGreen, LightRed
        }
        public enum PMode
        {
            Cash = 1, Cheque, Teller
        }
        public enum Status
        {
            GivenOut = 1, Returned
        }

        public enum Religion
        {
            Christianity = 1, Muslim, Others
        }

        public enum ClassType
        {
            UNITAS = 1, VERITAS, CARRITAS
        }
        public enum Maritalstatus
        {
            Single = 1, Married, Divorced
        }

        public enum Qualifications
        {
            SSCE,
            NCE,
            OND,
            HND,
            Degree,
            Masters
        }


        public enum State
        {
            Abia, Adamawa, AkwaIbom, Anambra, Bauchi, Bayelsa, Benue, Borno, CrossRiver, Delta, Ebonyi, Edo, Ekiti,
            Abuja, Gombe, Imo, Jigawa, Kaduna, Kano, Katsina, Kebbi, Kogi, Kwara, Lagos, Nasarawa, Niger, Ogun, Ondo, Osun,
            Oyo, Plateau, Rivers, Sokoto, Taraba, Yobe, Zamfara
        }

        public enum Grade
        {
            A1 = 1, B2, B3, C4, C5, C6, D7, E8, F9
        }

        public enum SchoolName
        {
            JSS = 1, SS, PRY, KG
        }

        //public enum ContactGroup
        //{
        //    JSS1 = 1, JSS2, JSS3, SS1, SS2, SS3, Staffs, FormTeacher, All_Student, Past_Students
        //}
        public enum TruckStatus : byte
        {
            [Display(Name = "Planned")]
            orange,
            [Display(Name = "Confirmed")]
            green,
            [Display(Name = "Changed")]
            red,
            [Display(Name = "Loaded")]
            darkcyan
        }

        public enum Extra
        {
            A = 1, B, C
        }

        public enum MyClass
        {
            JSS1 = 1, JSS2, JSS3, SS1, SS2, SS3
        }

    }
}