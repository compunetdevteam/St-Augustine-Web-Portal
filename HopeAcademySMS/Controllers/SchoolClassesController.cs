using StAugustine.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HopeAcademySMS.Controllers
{
    public class SchoolClassesController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: SchoolClasses
        public async Task<ActionResult> Index()
        {
            return View(await _db.SchoolClasses.ToListAsync());
        }

        // GET: SchoolClasses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchoolClass schoolClass = await _db.SchoolClasses.FindAsync(id);
            if (schoolClass == null)
            {
                return HttpNotFound();
            }
            return View(schoolClass);
        }

        // GET: SchoolClasses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SchoolClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ClassCode,ClassName")] SchoolClass model)
        {
            if (ModelState.IsValid)
            {
                var myClass = await _db.SchoolClasses.AsNoTracking().CountAsync(x => x.ClassCode.Equals(model.ClassCode.Trim()));

                if (myClass >= 1)
                {
                    TempData["UserMessage"] = "School Class Already Exist in Database";
                    TempData["Title"] = "Error.";
                    return View(model);
                }
                var schoolClass = new SchoolClass()
                {
                    ClassName = model.ClassName.Trim(),
                    ClassCode = model.ClassCode.Trim()
                };
                _db.SchoolClasses.Add(schoolClass);

                await _db.SaveChangesAsync();
                TempData["UserMessage"] = "School Class Created Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            TempData["UserMessage"] = "Some Values are not entered Correctly";
            TempData["Title"] = "Error.";
            return View(model);
        }

        // GET: SchoolClasses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchoolClass schoolClass = await _db.SchoolClasses.FindAsync(id);
            if (schoolClass == null)
            {
                return HttpNotFound();
            }
            return View(schoolClass);
        }

        // POST: SchoolClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ClassCode,ClassName")] SchoolClass schoolClass)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(schoolClass).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                TempData["UserMessage"] = "School Class Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            return View(schoolClass);
        }

        // GET: SchoolClasses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchoolClass schoolClass = await _db.SchoolClasses.FindAsync(id);
            if (schoolClass == null)
            {
                return HttpNotFound();
            }
            return View(schoolClass);
        }

        // POST: SchoolClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SchoolClass schoolClass = await _db.SchoolClasses.FindAsync(id);
            if (schoolClass != null) _db.SchoolClasses.Remove(schoolClass);
            await _db.SaveChangesAsync();
            TempData["UserMessage"] = "School Class Deleted Successfully";
            TempData["Title"] = "Error.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
