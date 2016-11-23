using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HopeAcademySMS.Models
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
    }
}