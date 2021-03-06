﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkool.Models
{
    public class Class
    {
        public int ClassId { get; set; }

        [Display(Name = "Class")]
        [Required(ErrorMessage = "Class Name cannot be empty")]
        public string SchoolName { get; set; }

        [Display(Name = "Class Level")]
        [Range(1, 6)]
        [Required(ErrorMessage = "Class Level cannot be empty")]
        public int ClassLevel { get; set; }

        [Display(Name = "Class Type")]
        [Required(ErrorMessage = "Class Type cannot be empty")]
        public string ClassType { get; set; }

        public string ClassName
        {
            get
            {
                return $"{this.SchoolName.ToString()}{this.ClassLevel}";
            }
            private set { }
        }
        public string FullClassName
        {
            get
            {
                return $"{this.ClassName.ToUpper()} {this.ClassType.ToUpper()}";
            }
            private set { }
        }
        // public string ClassName => $"{this.SchoolName.ToString()}{this.ClassLevel}";
        //public string FullClassName => $"{this.ClassName} {this.ClassType.ToString()}";

        public virtual AssignedClass AssignClass { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}