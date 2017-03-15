namespace HopeAcademySMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrators",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Affectives",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.String(),
                        TermName = c.String(),
                        SessionName = c.String(),
                        ClassName = c.String(),
                        Punctuality = c.Int(nullable: false),
                        BehaviourInClass = c.Int(nullable: false),
                        AttentivenessInClass = c.Int(nullable: false),
                        ClassAssignmentsProjects = c.Int(nullable: false),
                        Neatness = c.Int(nullable: false),
                        SelfControl = c.Int(nullable: false),
                        RelationshipWithOthers = c.Int(nullable: false),
                        RelationshipWithTeachersAndStaff = c.Int(nullable: false),
                        SenseOfResponsibility = c.Int(nullable: false),
                        AttendanceInClass = c.Int(nullable: false),
                        Politeness = c.Int(nullable: false),
                        HonestyAndReliability = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AssignedClasses",
                c => new
                    {
                        AssignedClassId = c.Int(nullable: false, identity: true),
                        StudentId = c.String(nullable: false),
                        ClassName = c.String(nullable: false),
                        TermName = c.String(nullable: false),
                        SessionName = c.String(nullable: false),
                        Result_ResultId = c.Int(),
                        Staff_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AssignedClassId)
                .ForeignKey("dbo.Results", t => t.Result_ResultId)
                .ForeignKey("dbo.Staffs", t => t.Staff_Id)
                .Index(t => t.Result_ResultId)
                .Index(t => t.Staff_Id);
            
            CreateTable(
                "dbo.AssignSubjects",
                c => new
                    {
                        AssignSubjectId = c.Int(nullable: false, identity: true),
                        ClassName = c.String(nullable: false),
                        SubjectName = c.String(nullable: false),
                        Result_ResultId = c.Int(),
                        AssignedClass_AssignedClassId = c.Int(),
                    })
                .PrimaryKey(t => t.AssignSubjectId)
                .ForeignKey("dbo.Results", t => t.Result_ResultId)
                .ForeignKey("dbo.AssignedClasses", t => t.AssignedClass_AssignedClassId)
                .Index(t => t.Result_ResultId)
                .Index(t => t.AssignedClass_AssignedClassId);
            
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        ClassId = c.Int(nullable: false, identity: true),
                        SchoolName = c.Int(nullable: false),
                        ClassLevel = c.Int(nullable: false),
                        ClassType = c.Int(nullable: false),
                        ClassName = c.String(),
                        FullClassName = c.String(),
                        AssignClass_AssignedClassId = c.Int(),
                        AssignSubject_AssignSubjectId = c.Int(),
                        Staff_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ClassId)
                .ForeignKey("dbo.AssignedClasses", t => t.AssignClass_AssignedClassId)
                .ForeignKey("dbo.AssignSubjects", t => t.AssignSubject_AssignSubjectId)
                .ForeignKey("dbo.Staffs", t => t.Staff_Id)
                .Index(t => t.AssignClass_AssignedClassId)
                .Index(t => t.AssignSubject_AssignSubjectId)
                .Index(t => t.Staff_Id);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        CourseCode = c.String(nullable: false, maxLength: 50),
                        CourseName = c.String(nullable: false, maxLength: 50),
                        CategoriesId = c.String(),
                        Result_ResultId = c.Int(),
                        ContinuousAssessment_ContinuousAssessmentId = c.Int(),
                        Student_StudentId = c.String(maxLength: 128),
                        SubjectCategory_CategoryId = c.Int(),
                        Class_ClassId = c.Int(),
                        AssignSubject_AssignSubjectId = c.Int(),
                        Staff_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CourseCode)
                .ForeignKey("dbo.Results", t => t.Result_ResultId)
                .ForeignKey("dbo.ContinuousAssessments", t => t.ContinuousAssessment_ContinuousAssessmentId)
                .ForeignKey("dbo.Students", t => t.Student_StudentId)
                .ForeignKey("dbo.SubjectCategories", t => t.SubjectCategory_CategoryId)
                .ForeignKey("dbo.Classes", t => t.Class_ClassId)
                .ForeignKey("dbo.AssignSubjects", t => t.AssignSubject_AssignSubjectId)
                .ForeignKey("dbo.Staffs", t => t.Staff_Id)
                .Index(t => t.Result_ResultId)
                .Index(t => t.ContinuousAssessment_ContinuousAssessmentId)
                .Index(t => t.Student_StudentId)
                .Index(t => t.SubjectCategory_CategoryId)
                .Index(t => t.Class_ClassId)
                .Index(t => t.AssignSubject_AssignSubjectId)
                .Index(t => t.Staff_Id);
            
            CreateTable(
                "dbo.ContinuousAssessments",
                c => new
                    {
                        ContinuousAssessmentId = c.Int(nullable: false, identity: true),
                        StudentId = c.String(nullable: false, maxLength: 128),
                        TermName = c.String(nullable: false),
                        SessionName = c.String(nullable: false),
                        SubjectCode = c.String(nullable: false),
                        SubjectCategory = c.String(),
                        ClassName = c.String(nullable: false),
                        Assignment1 = c.Double(nullable: false),
                        Assignment2 = c.Double(nullable: false),
                        FirstTest = c.Double(nullable: false),
                        SecondTest = c.Double(nullable: false),
                        ExamScore = c.Double(nullable: false),
                        StaffName = c.String(nullable: false),
                        Total = c.Double(nullable: false),
                        Grading = c.String(),
                        Remark = c.String(),
                        Result_ResultId = c.Int(),
                    })
                .PrimaryKey(t => t.ContinuousAssessmentId)
                .ForeignKey("dbo.Results", t => t.Result_ResultId)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.Result_ResultId);
            
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        ResultId = c.Int(nullable: false, identity: true),
                        StudentId = c.String(),
                        ClassName = c.String(),
                        Term = c.String(),
                        SessionName = c.String(),
                        SubjectName = c.String(),
                        SubjectPosition = c.Int(nullable: false),
                        AggretateScore = c.Double(nullable: false),
                        AggregatePosition = c.Int(nullable: false),
                        NoOfSubjectOffered = c.Int(nullable: false),
                        NoOfStudentPerClass = c.Int(nullable: false),
                        Average = c.Double(nullable: false),
                        ClassAverage = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ResultId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentId = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        MiddleName = c.String(),
                        GuardianEmail = c.String(),
                        Age = c.Int(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
                        AdmissionDate = c.DateTime(nullable: false),
                        Gender = c.String(),
                        StudentPassport = c.Binary(),
                        Active = c.Boolean(nullable: false),
                        Result_ResultId = c.Int(),
                        BookIssue_BookIssueId = c.Int(),
                    })
                .PrimaryKey(t => t.StudentId)
                .ForeignKey("dbo.Results", t => t.Result_ResultId)
                .ForeignKey("dbo.BookIssues", t => t.BookIssue_BookIssueId)
                .Index(t => t.Result_ResultId)
                .Index(t => t.BookIssue_BookIssueId);
            
            CreateTable(
                "dbo.FeePayments",
                c => new
                    {
                        FeePaymentId = c.Int(nullable: false),
                        StudentId = c.String(nullable: false, maxLength: 128),
                        FeeName = c.String(),
                        Term = c.Int(nullable: false),
                        Session = c.String(nullable: false),
                        PaidFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentMode = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Remaining = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FeeType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.FeePaymentId)
                .ForeignKey("dbo.FeeTypes", t => t.FeeType_Id)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.FeeType_Id);
            
            CreateTable(
                "dbo.FeeTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FeeName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Guardians",
                c => new
                    {
                        GuardianEmail = c.String(nullable: false, maxLength: 128),
                        GuardianId = c.String(),
                        Salutation = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        Gender = c.String(),
                        PhoneNumber = c.String(),
                        Address = c.String(),
                        Occupation = c.String(),
                        Relationship = c.String(),
                        Student_StudentId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.GuardianEmail)
                .ForeignKey("dbo.Students", t => t.Student_StudentId)
                .Index(t => t.Student_StudentId);
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        SessionId = c.Int(nullable: false),
                        SessionName = c.String(),
                        ContinuousAssessment_ContinuousAssessmentId = c.Int(),
                    })
                .PrimaryKey(t => t.SessionId)
                .ForeignKey("dbo.ContinuousAssessments", t => t.ContinuousAssessment_ContinuousAssessmentId)
                .Index(t => t.ContinuousAssessment_ContinuousAssessmentId);
            
            CreateTable(
                "dbo.SubjectCategories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.BookIssues",
                c => new
                    {
                        BookIssueId = c.Int(nullable: false, identity: true),
                        StudentId = c.String(nullable: false),
                        AccessionNo = c.String(nullable: false),
                        IssueDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookIssueId);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        AccessionNo = c.String(nullable: false, maxLength: 128),
                        BookId = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                        Author = c.String(nullable: false),
                        JointAuthor = c.String(),
                        Subject = c.String(nullable: false),
                        ISBN = c.String(nullable: false),
                        Edition = c.String(nullable: false),
                        Publisher = c.String(nullable: false),
                        PlaceOfPublish = c.String(),
                        BookIssue_BookIssueId = c.Int(),
                    })
                .PrimaryKey(t => t.AccessionNo)
                .ForeignKey("dbo.BookIssues", t => t.BookIssue_BookIssueId)
                .Index(t => t.BookIssue_BookIssueId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PostID = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Posts", t => t.PostID, cascadeDelete: true)
                .Index(t => t.PostID);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ExamRules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClassName = c.String(nullable: false),
                        CorrectMark = c.Int(nullable: false),
                        TotalQuestion = c.Int(nullable: false),
                        MaximunTime = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Expressions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.String(),
                        TermName = c.String(),
                        SessionName = c.String(),
                        ClassName = c.String(),
                        QualityofHandwriting = c.Int(nullable: false),
                        GrammaticalSkills = c.Int(nullable: false),
                        OralExpression = c.Int(nullable: false),
                        ImaginationCreativity = c.Int(nullable: false),
                        VocabularyLexicalSkills = c.Int(nullable: false),
                        OrganizationOfIdeas = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        GradeId = c.Int(nullable: false, identity: true),
                        GradeName = c.String(),
                        MinimumValue = c.Int(nullable: false),
                        MaximumValue = c.Int(nullable: false),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.GradeId);
            
            CreateTable(
                "dbo.OtherSkills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.String(),
                        TermName = c.String(),
                        SessionName = c.String(),
                        ClassName = c.String(),
                        TeamWorkTeamLeading = c.Int(nullable: false),
                        PhysicalDexterity = c.Int(nullable: false),
                        ClubAndSocieties = c.Int(nullable: false),
                        ArtisticOrMusicalSkills = c.Int(nullable: false),
                        LabAndWorkshopSkills = c.Int(nullable: false),
                        Sports = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuestionAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubjectName = c.String(nullable: false),
                        ClassName = c.String(nullable: false),
                        Question = c.String(nullable: false),
                        Option1 = c.String(nullable: false),
                        Option2 = c.String(nullable: false),
                        Option3 = c.String(nullable: false),
                        Option4 = c.String(nullable: false),
                        Answer = c.String(nullable: false),
                        QuestionHint = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReportSummaries",
                c => new
                    {
                        ReportSummaryId = c.Int(nullable: false, identity: true),
                        StudentId = c.String(),
                        SubjectName = c.String(),
                        ClassName = c.String(),
                        SessionName = c.String(),
                        FirstTermScore = c.Double(nullable: false),
                        FirstTermSubjectPosition = c.Int(nullable: false),
                        FirstTermSubjectGrade = c.String(),
                        SecondTermScore = c.Double(nullable: false),
                        SecondTermSubjectPosition = c.Int(nullable: false),
                        SecondTermSubjectGrade = c.String(),
                        ThirdTermScore = c.Double(nullable: false),
                        ThirdTermSubjectPosition = c.Int(nullable: false),
                        ThirdTermSubjectGrade = c.String(),
                        SummaryTotal = c.Double(nullable: false),
                        WeightedScores = c.Int(nullable: false),
                        SummaryGrading = c.String(),
                        SummaryRemark = c.String(),
                        TotalScorePerStudent = c.Double(nullable: false),
                        SummaryPosition = c.Int(nullable: false),
                        NoOfSubjectOffered = c.Int(nullable: false),
                        NoOfStudentPerClass = c.Int(nullable: false),
                        SummaryAverage = c.Double(nullable: false),
                        ClassAverage = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ReportSummaryId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SessionSubjectTotals",
                c => new
                    {
                        SessionSubjectTotalId = c.Int(nullable: false, identity: true),
                        StudentId = c.String(),
                        ClassName = c.String(),
                        SubjectName = c.String(),
                        SessionName = c.String(),
                        FirstTermScore = c.Double(nullable: false),
                        SecondTermScore = c.Double(nullable: false),
                        ThirdTermScore = c.Double(nullable: false),
                        SummaryTotal = c.Double(nullable: false),
                        WeightedScores = c.Int(nullable: false),
                        SummaryGrading = c.String(),
                        SummaryRemark = c.String(),
                    })
                .PrimaryKey(t => t.SessionSubjectTotalId);
            
            CreateTable(
                "dbo.Staffs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Salutation = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                        Gender = c.String(),
                        Address = c.String(),
                        StateOfOrigin = c.String(),
                        Designation = c.String(),
                        StaffPassport = c.Binary(),
                        DateOfBirth = c.DateTime(nullable: false),
                        MaritalStatus = c.String(),
                        Qualifications = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StudentQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.String(),
                        Question = c.String(),
                        Option1 = c.String(),
                        Option2 = c.String(),
                        Option3 = c.String(),
                        Option4 = c.String(),
                        Answer = c.String(),
                        QuestionHint = c.String(),
                        QuestionNumber = c.Int(nullable: false),
                        IsCorrect = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.StudentAssignedClasses",
                c => new
                    {
                        Student_StudentId = c.String(nullable: false, maxLength: 128),
                        AssignedClass_AssignedClassId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_StudentId, t.AssignedClass_AssignedClassId })
                .ForeignKey("dbo.Students", t => t.Student_StudentId, cascadeDelete: true)
                .ForeignKey("dbo.AssignedClasses", t => t.AssignedClass_AssignedClassId, cascadeDelete: true)
                .Index(t => t.Student_StudentId)
                .Index(t => t.AssignedClass_AssignedClassId);
            
            CreateTable(
                "dbo.TagPosts",
                c => new
                    {
                        Tag_ID = c.Int(nullable: false),
                        Post_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_ID, t.Post_ID })
                .ForeignKey("dbo.Tags", t => t.Tag_ID, cascadeDelete: true)
                .ForeignKey("dbo.Posts", t => t.Post_ID, cascadeDelete: true)
                .Index(t => t.Tag_ID)
                .Index(t => t.Post_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Subjects", "Staff_Id", "dbo.Staffs");
            DropForeignKey("dbo.Classes", "Staff_Id", "dbo.Staffs");
            DropForeignKey("dbo.AssignedClasses", "Staff_Id", "dbo.Staffs");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.TagPosts", "Post_ID", "dbo.Posts");
            DropForeignKey("dbo.TagPosts", "Tag_ID", "dbo.Tags");
            DropForeignKey("dbo.Comments", "PostID", "dbo.Posts");
            DropForeignKey("dbo.Students", "BookIssue_BookIssueId", "dbo.BookIssues");
            DropForeignKey("dbo.Books", "BookIssue_BookIssueId", "dbo.BookIssues");
            DropForeignKey("dbo.AssignSubjects", "AssignedClass_AssignedClassId", "dbo.AssignedClasses");
            DropForeignKey("dbo.Subjects", "AssignSubject_AssignSubjectId", "dbo.AssignSubjects");
            DropForeignKey("dbo.Classes", "AssignSubject_AssignSubjectId", "dbo.AssignSubjects");
            DropForeignKey("dbo.Subjects", "Class_ClassId", "dbo.Classes");
            DropForeignKey("dbo.Subjects", "SubjectCategory_CategoryId", "dbo.SubjectCategories");
            DropForeignKey("dbo.Subjects", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.Subjects", "ContinuousAssessment_ContinuousAssessmentId", "dbo.ContinuousAssessments");
            DropForeignKey("dbo.Sessions", "ContinuousAssessment_ContinuousAssessmentId", "dbo.ContinuousAssessments");
            DropForeignKey("dbo.Subjects", "Result_ResultId", "dbo.Results");
            DropForeignKey("dbo.Students", "Result_ResultId", "dbo.Results");
            DropForeignKey("dbo.Guardians", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.FeePayments", "StudentId", "dbo.Students");
            DropForeignKey("dbo.FeePayments", "FeeType_Id", "dbo.FeeTypes");
            DropForeignKey("dbo.ContinuousAssessments", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentAssignedClasses", "AssignedClass_AssignedClassId", "dbo.AssignedClasses");
            DropForeignKey("dbo.StudentAssignedClasses", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.ContinuousAssessments", "Result_ResultId", "dbo.Results");
            DropForeignKey("dbo.AssignSubjects", "Result_ResultId", "dbo.Results");
            DropForeignKey("dbo.AssignedClasses", "Result_ResultId", "dbo.Results");
            DropForeignKey("dbo.Classes", "AssignClass_AssignedClassId", "dbo.AssignedClasses");
            DropIndex("dbo.TagPosts", new[] { "Post_ID" });
            DropIndex("dbo.TagPosts", new[] { "Tag_ID" });
            DropIndex("dbo.StudentAssignedClasses", new[] { "AssignedClass_AssignedClassId" });
            DropIndex("dbo.StudentAssignedClasses", new[] { "Student_StudentId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Comments", new[] { "PostID" });
            DropIndex("dbo.Books", new[] { "BookIssue_BookIssueId" });
            DropIndex("dbo.Sessions", new[] { "ContinuousAssessment_ContinuousAssessmentId" });
            DropIndex("dbo.Guardians", new[] { "Student_StudentId" });
            DropIndex("dbo.FeePayments", new[] { "FeeType_Id" });
            DropIndex("dbo.FeePayments", new[] { "StudentId" });
            DropIndex("dbo.Students", new[] { "BookIssue_BookIssueId" });
            DropIndex("dbo.Students", new[] { "Result_ResultId" });
            DropIndex("dbo.ContinuousAssessments", new[] { "Result_ResultId" });
            DropIndex("dbo.ContinuousAssessments", new[] { "StudentId" });
            DropIndex("dbo.Subjects", new[] { "Staff_Id" });
            DropIndex("dbo.Subjects", new[] { "AssignSubject_AssignSubjectId" });
            DropIndex("dbo.Subjects", new[] { "Class_ClassId" });
            DropIndex("dbo.Subjects", new[] { "SubjectCategory_CategoryId" });
            DropIndex("dbo.Subjects", new[] { "Student_StudentId" });
            DropIndex("dbo.Subjects", new[] { "ContinuousAssessment_ContinuousAssessmentId" });
            DropIndex("dbo.Subjects", new[] { "Result_ResultId" });
            DropIndex("dbo.Classes", new[] { "Staff_Id" });
            DropIndex("dbo.Classes", new[] { "AssignSubject_AssignSubjectId" });
            DropIndex("dbo.Classes", new[] { "AssignClass_AssignedClassId" });
            DropIndex("dbo.AssignSubjects", new[] { "AssignedClass_AssignedClassId" });
            DropIndex("dbo.AssignSubjects", new[] { "Result_ResultId" });
            DropIndex("dbo.AssignedClasses", new[] { "Staff_Id" });
            DropIndex("dbo.AssignedClasses", new[] { "Result_ResultId" });
            DropTable("dbo.TagPosts");
            DropTable("dbo.StudentAssignedClasses");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.StudentQuestions");
            DropTable("dbo.Staffs");
            DropTable("dbo.SessionSubjectTotals");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ReportSummaries");
            DropTable("dbo.QuestionAnswers");
            DropTable("dbo.OtherSkills");
            DropTable("dbo.Grades");
            DropTable("dbo.Expressions");
            DropTable("dbo.ExamRules");
            DropTable("dbo.Tags");
            DropTable("dbo.Posts");
            DropTable("dbo.Comments");
            DropTable("dbo.Books");
            DropTable("dbo.BookIssues");
            DropTable("dbo.SubjectCategories");
            DropTable("dbo.Sessions");
            DropTable("dbo.Guardians");
            DropTable("dbo.FeeTypes");
            DropTable("dbo.FeePayments");
            DropTable("dbo.Students");
            DropTable("dbo.Results");
            DropTable("dbo.ContinuousAssessments");
            DropTable("dbo.Subjects");
            DropTable("dbo.Classes");
            DropTable("dbo.AssignSubjects");
            DropTable("dbo.AssignedClasses");
            DropTable("dbo.Affectives");
            DropTable("dbo.Administrators");
        }
    }
}
