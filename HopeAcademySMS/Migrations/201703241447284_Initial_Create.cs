namespace HopeAcademySMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_Create : DbMigration
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
                        Honesty = c.String(),
                        SelfConfidence = c.String(),
                        Sociability = c.String(),
                        Punctuality = c.String(),
                        Neatness = c.String(),
                        Initiative = c.String(),
                        Organization = c.String(),
                        AttendanceInClass = c.String(),
                        HonestyAndReliability = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AppointmentDiaries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        SomeImportantKey = c.Int(nullable: false),
                        DateTimeScheduled = c.DateTime(nullable: false),
                        AppointmentLength = c.Int(nullable: false),
                        StatusENUM = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AssignedClasses",
                c => new
                    {
                        AssignedClassId = c.Int(nullable: false, identity: true),
                        StudentId = c.String(nullable: false, maxLength: 128),
                        ClassName = c.String(nullable: false),
                        TermName = c.String(nullable: false),
                        SessionName = c.String(nullable: false),
                        Result_ResultId = c.Int(),
                        Staff_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AssignedClassId)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Results", t => t.Result_ResultId)
                .ForeignKey("dbo.Staffs", t => t.Staff_Id)
                .Index(t => t.StudentId)
                .Index(t => t.Result_ResultId)
                .Index(t => t.Staff_Id);
            
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
                "dbo.ContinuousAssessments",
                c => new
                    {
                        ContinuousAssessmentId = c.Int(nullable: false, identity: true),
                        StudentId = c.String(nullable: false, maxLength: 128),
                        TermName = c.String(nullable: false),
                        SessionName = c.String(nullable: false),
                        SubjectCode = c.String(nullable: false),
                        ClassName = c.String(nullable: false),
                        ProjectScore = c.Double(nullable: false),
                        Assignment = c.Double(nullable: false),
                        Test = c.Double(nullable: false),
                        ExamScore = c.Double(nullable: false),
                        StaffName = c.String(nullable: false),
                        Total = c.Double(nullable: false),
                        Grading = c.String(),
                        Remark = c.String(),
                        GradePoint = c.Int(nullable: false),
                        QualityPoint = c.Int(nullable: false),
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
                        SubjectHighest = c.Double(nullable: false),
                        SubjectLowest = c.Double(nullable: false),
                        SubjectPosition = c.Int(nullable: false),
                        AggretateScore = c.Double(nullable: false),
                        Average = c.Double(nullable: false),
                        ClassAverage = c.Double(nullable: false),
                        TotalQualityPoint = c.Double(nullable: false),
                        TotalCreditUnit = c.Double(nullable: false),
                        GradePointAverage = c.Double(nullable: false),
                        GPA = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ResultId);
            
            CreateTable(
                "dbo.AssignSubjects",
                c => new
                    {
                        AssignSubjectId = c.Int(nullable: false, identity: true),
                        ClassName = c.String(nullable: false),
                        SubjectName = c.String(nullable: false),
                        Class_ClassId = c.Int(),
                        Subject_CourseCode = c.String(maxLength: 50),
                        Result_ResultId = c.Int(),
                    })
                .PrimaryKey(t => t.AssignSubjectId)
                .ForeignKey("dbo.Classes", t => t.Class_ClassId)
                .ForeignKey("dbo.Subjects", t => t.Subject_CourseCode)
                .ForeignKey("dbo.Results", t => t.Result_ResultId)
                .Index(t => t.Class_ClassId)
                .Index(t => t.Subject_CourseCode)
                .Index(t => t.Result_ResultId);
            
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
                        Staff_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ClassId)
                .ForeignKey("dbo.AssignedClasses", t => t.AssignClass_AssignedClassId)
                .ForeignKey("dbo.Staffs", t => t.Staff_Id)
                .Index(t => t.AssignClass_AssignedClassId)
                .Index(t => t.Staff_Id);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        CourseCode = c.String(nullable: false, maxLength: 50),
                        CourseName = c.String(nullable: false, maxLength: 50),
                        SubjectUnit = c.Int(nullable: false),
                        ContinuousAssessment_ContinuousAssessmentId = c.Int(),
                        Class_ClassId = c.Int(),
                        Result_ResultId = c.Int(),
                        Staff_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CourseCode)
                .ForeignKey("dbo.ContinuousAssessments", t => t.ContinuousAssessment_ContinuousAssessmentId)
                .ForeignKey("dbo.Classes", t => t.Class_ClassId)
                .ForeignKey("dbo.Results", t => t.Result_ResultId)
                .ForeignKey("dbo.Staffs", t => t.Staff_Id)
                .Index(t => t.ContinuousAssessment_ContinuousAssessmentId)
                .Index(t => t.Class_ClassId)
                .Index(t => t.Result_ResultId)
                .Index(t => t.Staff_Id);
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        SessionId = c.Int(nullable: false, identity: true),
                        SessionName = c.String(nullable: false, maxLength: 20),
                        ActiveSession = c.Boolean(nullable: false),
                        ContinuousAssessment_ContinuousAssessmentId = c.Int(),
                    })
                .PrimaryKey(t => t.SessionId)
                .ForeignKey("dbo.ContinuousAssessments", t => t.ContinuousAssessment_ContinuousAssessmentId)
                .Index(t => t.SessionName, unique: true)
                .Index(t => t.ContinuousAssessment_ContinuousAssessmentId);
            
            CreateTable(
                "dbo.FeePayments",
                c => new
                    {
                        FeePaymentId = c.Int(nullable: false),
                        StudentId = c.String(nullable: false, maxLength: 128),
                        FeeName = c.String(),
                        Term = c.String(nullable: false),
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
                        GuardianId = c.String(nullable: false, maxLength: 128),
                        Salutation = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        Gender = c.String(),
                        PhoneNumber = c.String(),
                        GuardianEmail = c.String(),
                        Address = c.String(),
                        Occupation = c.String(),
                        Relationship = c.String(),
                        StudentId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.GuardianId)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.AssignFormTeacherToClasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClassName = c.String(),
                        Username = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AssignSubjectTeachers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubjectName = c.String(),
                        ClassName = c.String(),
                        StaffName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.Grades",
                c => new
                    {
                        GradeId = c.Int(nullable: false, identity: true),
                        GradeName = c.String(),
                        MinimumValue = c.Int(nullable: false),
                        MaximumValue = c.Int(nullable: false),
                        GradePoint = c.Int(nullable: false),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.GradeId);
            
            CreateTable(
                "dbo.PrincipalComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MinimumGrade = c.Double(nullable: false),
                        MaximumGrade = c.Double(nullable: false),
                        Remark = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Psychomotors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.String(),
                        TermName = c.String(),
                        SessionName = c.String(),
                        ClassName = c.String(),
                        Sports = c.String(),
                        ExtraCurricularActivity = c.String(),
                        Assignment = c.String(),
                        HelpingOthers = c.String(),
                        ManualDuty = c.String(),
                        LevelOfCommitment = c.String(),
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
                "dbo.SubjectRegistrations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.String(nullable: false),
                        ClassName = c.String(nullable: false),
                        TermName = c.String(nullable: false),
                        SessionName = c.String(nullable: false),
                        SubjectCode = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TeacherComments",
                c => new
                    {
                        TeacherCommentId = c.Int(nullable: false, identity: true),
                        StudentId = c.String(nullable: false),
                        TermName = c.String(nullable: false),
                        SessionName = c.String(nullable: false),
                        Remark = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TeacherCommentId);
            
            CreateTable(
                "dbo.Terms",
                c => new
                    {
                        TermId = c.Int(nullable: false, identity: true),
                        TermName = c.String(maxLength: 20),
                        ActiveTerm = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TermId)
                .Index(t => t.TermName, unique: true);
            
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
            DropForeignKey("dbo.Guardians", "StudentId", "dbo.Students");
            DropForeignKey("dbo.FeePayments", "StudentId", "dbo.Students");
            DropForeignKey("dbo.FeePayments", "FeeType_Id", "dbo.FeeTypes");
            DropForeignKey("dbo.ContinuousAssessments", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Sessions", "ContinuousAssessment_ContinuousAssessmentId", "dbo.ContinuousAssessments");
            DropForeignKey("dbo.Subjects", "Result_ResultId", "dbo.Results");
            DropForeignKey("dbo.Students", "Result_ResultId", "dbo.Results");
            DropForeignKey("dbo.ContinuousAssessments", "Result_ResultId", "dbo.Results");
            DropForeignKey("dbo.AssignSubjects", "Result_ResultId", "dbo.Results");
            DropForeignKey("dbo.AssignSubjects", "Subject_CourseCode", "dbo.Subjects");
            DropForeignKey("dbo.AssignSubjects", "Class_ClassId", "dbo.Classes");
            DropForeignKey("dbo.Subjects", "Class_ClassId", "dbo.Classes");
            DropForeignKey("dbo.Subjects", "ContinuousAssessment_ContinuousAssessmentId", "dbo.ContinuousAssessments");
            DropForeignKey("dbo.Classes", "AssignClass_AssignedClassId", "dbo.AssignedClasses");
            DropForeignKey("dbo.AssignedClasses", "Result_ResultId", "dbo.Results");
            DropForeignKey("dbo.AssignedClasses", "StudentId", "dbo.Students");
            DropIndex("dbo.TagPosts", new[] { "Post_ID" });
            DropIndex("dbo.TagPosts", new[] { "Tag_ID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Terms", new[] { "TermName" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Comments", new[] { "PostID" });
            DropIndex("dbo.Books", new[] { "BookIssue_BookIssueId" });
            DropIndex("dbo.Guardians", new[] { "StudentId" });
            DropIndex("dbo.FeePayments", new[] { "FeeType_Id" });
            DropIndex("dbo.FeePayments", new[] { "StudentId" });
            DropIndex("dbo.Sessions", new[] { "ContinuousAssessment_ContinuousAssessmentId" });
            DropIndex("dbo.Sessions", new[] { "SessionName" });
            DropIndex("dbo.Subjects", new[] { "Staff_Id" });
            DropIndex("dbo.Subjects", new[] { "Result_ResultId" });
            DropIndex("dbo.Subjects", new[] { "Class_ClassId" });
            DropIndex("dbo.Subjects", new[] { "ContinuousAssessment_ContinuousAssessmentId" });
            DropIndex("dbo.Classes", new[] { "Staff_Id" });
            DropIndex("dbo.Classes", new[] { "AssignClass_AssignedClassId" });
            DropIndex("dbo.AssignSubjects", new[] { "Result_ResultId" });
            DropIndex("dbo.AssignSubjects", new[] { "Subject_CourseCode" });
            DropIndex("dbo.AssignSubjects", new[] { "Class_ClassId" });
            DropIndex("dbo.ContinuousAssessments", new[] { "Result_ResultId" });
            DropIndex("dbo.ContinuousAssessments", new[] { "StudentId" });
            DropIndex("dbo.Students", new[] { "BookIssue_BookIssueId" });
            DropIndex("dbo.Students", new[] { "Result_ResultId" });
            DropIndex("dbo.AssignedClasses", new[] { "Staff_Id" });
            DropIndex("dbo.AssignedClasses", new[] { "Result_ResultId" });
            DropIndex("dbo.AssignedClasses", new[] { "StudentId" });
            DropTable("dbo.TagPosts");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Terms");
            DropTable("dbo.TeacherComments");
            DropTable("dbo.SubjectRegistrations");
            DropTable("dbo.StudentQuestions");
            DropTable("dbo.Staffs");
            DropTable("dbo.SessionSubjectTotals");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ReportSummaries");
            DropTable("dbo.QuestionAnswers");
            DropTable("dbo.Psychomotors");
            DropTable("dbo.PrincipalComments");
            DropTable("dbo.Grades");
            DropTable("dbo.ExamRules");
            DropTable("dbo.Tags");
            DropTable("dbo.Posts");
            DropTable("dbo.Comments");
            DropTable("dbo.Books");
            DropTable("dbo.BookIssues");
            DropTable("dbo.AssignSubjectTeachers");
            DropTable("dbo.AssignFormTeacherToClasses");
            DropTable("dbo.Guardians");
            DropTable("dbo.FeeTypes");
            DropTable("dbo.FeePayments");
            DropTable("dbo.Sessions");
            DropTable("dbo.Subjects");
            DropTable("dbo.Classes");
            DropTable("dbo.AssignSubjects");
            DropTable("dbo.Results");
            DropTable("dbo.ContinuousAssessments");
            DropTable("dbo.Students");
            DropTable("dbo.AssignedClasses");
            DropTable("dbo.AppointmentDiaries");
            DropTable("dbo.Affectives");
            DropTable("dbo.Administrators");
        }
    }
}
