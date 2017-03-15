﻿using StAugustine.Models;
using System.Collections.Generic;

namespace StAugustine.ViewModel
{
    public class ReportViewModel
    {
        public int NoOfStudentPerClass { get; set; }
        public double AggregateScore { get; set; }
        public int AggregatePosition { get; set; }
        public double Average { get; set; }
        public double TotalScore { get; set; }
        public double GPA { get; set; }
        public double TotalQualityPoint { get; set; }

        public double TotalCreditUnit { get; set; }

        public double GradePointAverage { get; set; }
        public Student Student { get; set; }
        public List<ContinuousAssessment> Maths { get; set; }
        public List<ContinuousAssessment> English { get; set; }
        public List<ContinuousAssessment> ContinuousAssessments { get; set; }
        public List<Result> Results { get; set; }
    }
}