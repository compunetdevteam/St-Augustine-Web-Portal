using StAugustine.Models;
using System.ComponentModel.DataAnnotations;

namespace StAugustine.ViewModel
{
    public class AffectiveViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Student ID")]
        [Required(ErrorMessage = "Your Student ID Number is required")]
        [StringLength(15, ErrorMessage = "Your Student ID is too long")]
        public string StudentId { get; set; }

        [Display(Name = "Term")]
        [Required(ErrorMessage = "Term is required")]
        public PopUp.Term TermName { get; set; }

        [Display(Name = "Session")]
        [Required(ErrorMessage = "Session is required")]
        public string SessionName { get; set; }

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name is required")]
        public string ClassName { get; set; }

        [Display(Name = "Punctuality")]
        [Range(1, 3)]
        public int Punctuality { get; set; }

        [Display(Name = "Behaviour In Class")]
        [Range(1, 3)]
        public int BehaviourInClass { get; set; }
        [Display(Name = "Attentiveness In Class")]
        [Range(1, 3)]
        public int AttentivenessInClass { get; set; }

        [Display(Name = "Class Assignments/Projects")]
        [Range(1, 3)]
        public int ClassAssignmentsProjects { get; set; }

        [Display(Name = "Neatness")]
        [Range(1, 3)]
        public int Neatness { get; set; }

        [Display(Name = "SelfControl")]
        [Range(1, 3)]
        public int SelfControl { get; set; }

        [Display(Name = "Relationship With Others")]
        [Range(1, 3)]
        public int RelationshipWithOthers { get; set; }

        [Display(Name = "Relationship With Teachers/Staff")]
        [Range(1, 3)]
        public int RelationshipWithTeachersAndStaff { get; set; }

        [Display(Name = "Sense Of Responsibility")]
        [Range(1, 3)]
        public int SenseOfResponsibility { get; set; }
        [Display(Name = "Attendance In Class")]
        [Range(1, 3)]
        public int AttendanceInClass { get; set; }

        [Display(Name = "Politeness")]
        [Range(1, 3)]
        public int Politeness { get; set; }

        [Display(Name = "Honesty And Reliability")]
        [Range(1, 3)]
        public int HonestyAndReliability { get; set; }
    }

    public class ExpressionViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Student ID")]
        [Required(ErrorMessage = "Your Student ID Number is required")]
        [StringLength(15, ErrorMessage = "Your Student ID is too long")]
        public string StudentId { get; set; }

        [Display(Name = "Term")]
        [Required(ErrorMessage = "Term is required")]
        public PopUp.Term TermName { get; set; }

        [Display(Name = "Session")]
        [Required(ErrorMessage = "Session is required")]
        public string SessionName { get; set; }

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name is required")]
        public string ClassName { get; set; }
        [Display(Name = "Quality of Handwriting")]
        [Range(1, 3)]
        public int QualityofHandwriting { get; set; }

        [Display(Name = "Grammatical Skills")]
        [Range(1, 3)]
        public int GrammaticalSkills { get; set; }

        [Display(Name = "Oral Expression")]
        [Range(1, 3)]
        public int OralExpression { get; set; }

        [Display(Name = "Imagination Creativity")]
        [Range(1, 3)]
        public int ImaginationCreativity { get; set; }

        [Display(Name = "Vocabulary and Lexical Skills")]
        [Range(1, 3)]
        public int VocabularyLexicalSkills { get; set; }

        [Display(Name = "Organization Of Ideas")]
        [Range(1, 3)]
        public int OrganizationOfIdeas { get; set; }
    }

    public class OtherSkillsViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Student ID")]
        [Required(ErrorMessage = "Your Student ID Number is required")]
        [StringLength(15, ErrorMessage = "Your Student ID is too long")]
        public string StudentId { get; set; }

        [Display(Name = "Term")]
        [Required(ErrorMessage = "Term is required")]
        public PopUp.Term TermName { get; set; }

        [Display(Name = "Session")]
        [Required(ErrorMessage = "Session is required")]
        public string SessionName { get; set; }

        [Display(Name = "Class Name")]
        [Required(ErrorMessage = "Class Name is required")]
        public string ClassName { get; set; }
        [Display(Name = "Team Work/Team Leading")]
        [Range(1, 3)]
        public int TeamWorkTeamLeading { get; set; }

        [Display(Name = "Physical Dexterity")]
        [Range(1, 3)]
        public int PhysicalDexterity { get; set; }

        [Display(Name = "Club And Societies")]
        [Range(1, 3)]
        public int ClubAndSocieties { get; set; }
        [Display(Name = "Artistic Or Musical Skills")]
        [Range(1, 3)]
        public int ArtisticOrMusicalSkills { get; set; }

        [Display(Name = "Lab And Workshop Skills")]
        [Range(1, 3)]
        public int LabAndWorkshopSkills { get; set; }

        [Display(Name = "Sports")]
        [Range(1, 3)]
        public int Sports { get; set; }
    }
}