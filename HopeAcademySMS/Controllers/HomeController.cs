using System.Web.Mvc;

namespace StAugustine.Controllers
{

    public class HomeController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        //public ActionResult Index()
        //{
        //    int totalMaleStudent = db.Students.Count(s => s.Gender.Equals("Male"));
        //    int totalFemaleStudent = db.Students.Count(s => s.Gender.Equals("Female"));
        //    int totalStudent = db.Students.Count();
        //    int totalStaff = db.Staffs.Count();

        //    double val1 = totalMaleStudent * 100;
        //    double val2 = totalFemaleStudent * 100;

        //    double boysPercentage = Math.Round(val1 / totalStudent, 2);
        //    double femalePercentage = Math.Round(val2 / totalStudent, 2);

        //    ViewBag.MaleStudent = totalMaleStudent;
        //    ViewBag.Femalestudent = totalFemaleStudent;
        //    ViewBag.TotalStudent = totalStudent;
        //    ViewBag.TotalStaff = totalStaff;
        //    ViewBag.BoysPercentage = boysPercentage;
        //    ViewBag.FemalePercentage = femalePercentage;
        //    return View();
        //}

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return new Rotativa.ViewAsPdf();
            //return View();
        }

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}