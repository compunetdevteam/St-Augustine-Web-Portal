using SwiftSkool.Models;
using SwiftSkool.ViewModel;
using System;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SwiftSkool.Controllers
{

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

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
        public async Task<ActionResult> Index()
        {
            ViewBag.PictureList = await _db.HomePageSetUps.AsNoTracking().CountAsync();
            return View(await _db.HomePageSetUps.ToListAsync());
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return new Rotativa.ViewAsPdf();
            //return View();
        }

        public ActionResult SchoolSetUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SchoolSetUp(SetUpVm model)
        {
            string _FileName = String.Empty;
            if (model.File?.ContentLength > 0)
            {
                _FileName = Path.GetFileName(model.File.FileName);
                string _path = HostingEnvironment.MapPath("~/Content/Images/") + _FileName;
                var directory = new DirectoryInfo(HostingEnvironment.MapPath("~/Content/Images/"));
                if (directory.Exists == false)
                {
                    directory.Create();
                }
                model.File.SaveAs(_path);
            }


            //ViewBag.Message = "File upload failed!!";
            //return View(model);

            Configuration objConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");
            //Edit
            if (objAppsettings != null)
            {
                objAppsettings.Settings["SchoolName"].Value = model.SchoolName;
                objAppsettings.Settings["SchoolTheme"].Value = model.SchoolTheme.ToString();
                if (!String.IsNullOrEmpty(_FileName))
                {
                    objAppsettings.Settings["SchoolImage"].Value = _FileName;
                }
                objConfig.Save();
            }
            return View("Index");
        }

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}