﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SwiftSkool.Calender;
using SwiftSkool.Models.CBT;
using SwiftSkool.Models.Objects;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SwiftSkool.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {

        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Staff> Staffs { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<ContinuousAssessment> ContinuousAssessments { get; set; }
        //public DbSet<SubjectPositions> SubjectPositions { get; set; }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<FeePayment> FeePayments { get; set; }
        public DbSet<FeeType> FeeTypes { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<BookIssue> BookIssues { get; set; }

        public DbSet<Guardian> Guardians { get; set; }

        public DbSet<AssignedClass> AssignedClasses { get; set; }

        public DbSet<AssignSubject> AssignSubjects { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<ReportSummary> ReportSummarys { get; set; }

        public DbSet<SessionSubjectTotal> SessionSubjectTotals { get; set; }
        public DbSet<Grade> Grades { get; set; }

        public DbSet<Affective> Affectives { get; set; }

        public DbSet<Psychomotor> Psychomotors { get; set; }



        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<CBT.StudentQuestion> StudentQuestions { get; set; }

        public DbSet<ExamRule> ExamRules { get; set; }

        public DbSet<AssignFormTeacherToClass> AssignFormTeacherToClasses { get; set; }

        public DbSet<AssignSubjectTeacher> AssignSubjectTeachers { get; set; }

        public DbSet<PrincipalComment> PrincipalComments { get; set; }

        public DbSet<TeacherComment> TeacherComments { get; set; }

        public DbSet<Term> Terms { get; set; }

        public DbSet<AppointmentDiary> AppointmentDiary { get; set; }

        public DbSet<SubjectRegistration> SubjectRegistrations { get; set; }

        public DbSet<SchoolClass> SchoolClasses { get; set; }

        public DbSet<BehaviorSkillCategory> BehaviorSkillCategories { get; set; }

        public DbSet<BehaviouralSkill> BehaviouralSkills { get; set; }

        public DbSet<AssignBehavior> AssignBehaviors { get; set; }

        public DbSet<ReportCard> ReportCards { get; set; }

        public System.Data.Entity.DbSet<HopeAcademySMS.Models.HomePageSetUp> HomePageSetUps { get; set; }
    }
}