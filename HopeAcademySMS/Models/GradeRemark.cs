using System.Linq;

namespace StAugustine.Models
{
    public class GradeRemark
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // This can be private now
        public string Grading(double summaryTotal)
        {
            string gradeValue = "";
            int mySummaryTotal = (int)summaryTotal;

            var myGrade = db.Grades.ToList();
            foreach (var item in myGrade)
            {
                if (mySummaryTotal <= item.MaximumValue && mySummaryTotal >= item.MinimumValue)
                {
                    gradeValue = item.GradeName;
                }

            }
            return !string.IsNullOrEmpty(gradeValue) ? gradeValue : "Enter Value between 1 - 100";
            // return gradeValue;

        }

        // This can be private now
        public string Remark(double summaryTotal)
        {
            string remarkValue = "";

            int mySummaryTotal = (int)summaryTotal;
            var myGrade = db.Grades.ToList();
            foreach (var item in myGrade)
            {
                if (mySummaryTotal <= item.MaximumValue && mySummaryTotal >= item.MinimumValue)
                {
                    remarkValue = item.Remark;
                }
            }

            return !string.IsNullOrEmpty(remarkValue) ? remarkValue : "Enter Value between 1 - 100";
        }

        public int GradingPoint(double summaryTotal)
        {
            int remarkValue = 0;

            int mySummaryTotal = (int)summaryTotal;
            var myGrade = db.Grades.ToList();
            foreach (var item in myGrade)
            {
                if (mySummaryTotal <= item.MaximumValue && mySummaryTotal >= item.MinimumValue)
                {
                    remarkValue = item.GradePoint;
                }
            }
            if (remarkValue == 0)
            {
                return 0;
            }
            else
            {
                return remarkValue;

            }
        }

        public string PrincipalRemark(double summaryTotal)
        {
            string remarkValue = "";

            int mySummaryTotal = (int)summaryTotal;
            var myGrade = db.Grades.ToList();
            foreach (var item in myGrade)
            {
                if (mySummaryTotal <= item.MaximumValue && mySummaryTotal >= item.MinimumValue)
                {
                    remarkValue = item.PrincipalRemark;
                }
            }

            return !string.IsNullOrEmpty(remarkValue) ? remarkValue : "Enter Value between 1 - 100";

        }
    }
}
