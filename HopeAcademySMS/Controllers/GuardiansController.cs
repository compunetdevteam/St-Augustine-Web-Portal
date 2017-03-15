using StAugustine.Models;
using StAugustine.ViewModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StAugustine.Controllers
{
    public class GuardiansController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Guardians
        public async Task<ActionResult> Index()
        {
            return View(await db.Guardians.ToListAsync());
        }

        // GET: Guardians/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guardian guardian = db.Guardians.FirstOrDefault(x => x.GuardianId.Equals(id));
            if (guardian == null)
            {
                return HttpNotFound();
            }
            return View(guardian);
        }


        // GET: Guardians/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guardian guardian = db.Guardians.FirstOrDefault(x => x.GuardianId.Equals(id));
            if (guardian == null)
            {
                return HttpNotFound();
            }
            var myGuardian = new GuardianEditViewModel()
            {
                GuardianId = guardian.GuardianId,
                FirstName = guardian.FirstName,
                MiddleName = guardian.MiddleName,
                LastName = guardian.LastName,
                PhoneNumber = guardian.PhoneNumber,
                Address = guardian.Address,
                Occupation = guardian.Occupation,
                Email = guardian.GuardianEmail
            };
            return View(myGuardian);
        }

        // POST: Guardians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(GuardianEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var guardian = new Guardian(model.GuardianId, model.Salutation.ToString(),
                                           model.FirstName, model.MiddleName, model.LastName, model.Gender.ToString(),
                                           model.PhoneNumber, model.Address, model.Email, model.Relationship.ToString(), model.Occupation);
                db.Entry(guardian).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
