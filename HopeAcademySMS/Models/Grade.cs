using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HopeAcademySMS.Models
{
    public class Grade
    {
        public int GradeID { get; set; }

        [Display(Name = "Student's Name")]
        [Required(ErrorMessage = "Student's Name is required")]
        public string StudentNumber { get; set; }

        [Display(Name = "Course Code")]
        [Required(ErrorMessage = "Course Code is required")]
        public string CourseCode { get; set; }

        [Display(Name = "Assignment Score")]
        public double Assignment { get; set; }

        [Display(Name = "First Test Score")]
        public double FirstTest { get; set; }

        [Display(Name = "Second Test Score")]
        public double SecondTest { get; set; }

        [Display(Name = "Examination Score")]
        public double ExamScore { get; set; }

        [Display(Name = "Term")]
        public string Term { get; set; }

        [Display(Name = "Teacher's Name")]
        public string StaffName { get; set; }

        public double Total
        {
            get
            {
                double sum = Assignment + FirstTest + SecondTest + ExamScore;
                return sum;
            }
        }

        public string Grading
        {
            #region Checking grade
            get
            {                
                if (Total <= 100 && Total >= 95)
                {
                    return "A1";
                }
                else if (Total <= 94 && Total >= 90)
                {
                    return "B2";
                }
                else if (Total <= 89 && Total >= 85)
                {
                    return "B3";
                }
                else if (Total <= 84 && Total >= 80)
                {
                    return "C4";
                }
                else if (Total <= 79 && Total >= 70)
                {
                    return "C5";
                }
                else if (Total <= 69 && Total >= 60)
                {
                    return "C6";
                }
                else if (Total <= 59 && Total >= 55)
                {
                    return "D7";
                }
                else if (Total <= 54 && Total >= 50)
                {
                    return "E8";
                }
                else if (Total <= 49 && Total >= 0)
                {
                    return "F9";
                }
                else
                {
                    return "Enter the Value between 0 - 100";
                }
            }
            #endregion
        } 

        public string Remark
        {
                #region Checking grade
                get
            {
                    if (Total <= 100 && Total >= 95)
                    {
                        return "Excellent";
                    }
                    else if (Total <= 94 && Total >= 90)
                    {
                        return "Very Good";
                    }
                    else if (Total <= 89 && Total >= 85)
                    {
                        return "Good";
                    }
                    else if (Total <= 84 && Total >= 80)
                    {
                        return "Upper Credit";
                    }
                    else if (Total <= 79 && Total >= 70)
                    {
                        return "Credit";
                    }
                    else if (Total <= 69 && Total >= 60)
                    {
                        return "Lower Credit";
                    }
                    else if (Total <= 59 && Total >= 55)
                    {
                        return "Pass";
                    }
                    else if (Total <= 54 && Total >= 50)
                    {
                        return "Fair";
                    }
                    else if (Total <= 49 && Total >= 0)
                    {
                        return "Fail";
                    }
                    else
                    {
                        return "Enter the Value between 0 - 100";
                    }
                }
                #endregion
            }

        public virtual Student Student { get; set; }

        public virtual Course Course { get; set; }



    }
}