using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SystemADM.Models;
using SystemADM.ViewModels;

namespace SystemADM.Controllers
{
   
    [Authorize]
    [Authorize(Roles = "super_admin")]
    public class UsersController : Controller
    {
        private systemADMEntities1 db = new systemADMEntities1();

     
        // GET: Users
        public ActionResult Index()
        {
            var users = db.users.Include(u => u.role);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.id_rola = new SelectList(db.role, "id_rola", "typ_uzytkownika");

            //moja edycja start tu
            var user = new User();
            user.budynki = new List<Building>();
            PopulateAssignedBuildingData(user);
            // koniec tu

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_uzytkownik,id_rola,imie,nazwisko,email,telefon,data_rejestracji,haslo")] User user, string[] selectedOptions)
        {
            if(selectedOptions != null)
            {
                user.budynki = new List<Building>();
                foreach(var wm in selectedOptions)
                {
                    var wmToAdd = db.budynki.Find(int.Parse(wm));
                    user.budynki.Add(wmToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                db.users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_rola = new SelectList(db.role, "id_rola", "typ_uzytkownika", user.id_rola);
            PopulateAssignedBuildingData(user);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            /*      User user = db.users.Find(id);*/
            User user = db.users
                      .Include(p => p.budynki)
                      .Where(i => i.id_uzytkownik == id)
                      .Single();
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_rola = new SelectList(db.role, "id_rola", "typ_uzytkownika", user.id_rola);

            PopulateAssignedBuildingData(user);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedOptions)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userToUpdate = db.users
                .Include(p => p.budynki)
                .Where(i => i.id_uzytkownik == id)
                .Single();

            if (TryUpdateModel(userToUpdate, "",
                new string[] { "id_uzytkownik", "id_rola", "imie", "nazwisko", "email", "telefon", "data_rejestracji", "haslo" }))
            {
                try
                {
                    UpdateUsersBuildings(selectedOptions, userToUpdate);

                    db.Entry(userToUpdate).State = EntityState.Modified;
                    db.Entry(userToUpdate).Property(x => x.haslo).IsModified = false;
     
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
          
            ViewBag.id_rola = new SelectList(db.role, "id_rola", "typ_uzytkownika", userToUpdate.id_rola);
            PopulateAssignedBuildingData((User)userToUpdate);
            return View(userToUpdate);
        }


        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.users.Find(id);
            db.users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void UpdateUsersBuildings(string [] selectedOptions, User userToUpdate)
        {
            if (selectedOptions == null) 
            {
                userToUpdate.budynki = new List<Building>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var usersBuildings = new HashSet<int>
                (userToUpdate.budynki.Select(b => b.id_budynek));

            foreach(var wm in db.budynki)
            {
                if (selectedOptionsHS.Contains(wm.id_budynek.ToString()))
                {
                    if (!usersBuildings.Contains(wm.id_budynek))
                    {
                        userToUpdate.budynki.Add(wm);
                    }
                }
                else
                {
                    if (usersBuildings.Contains(wm.id_budynek))
                    {
                        userToUpdate.budynki.Remove(wm);
                    }
                }
            }
        }


        private void PopulateAssignedBuildingData(User user)
        {
            var allBuildings = db.budynki;
            var usersWM = new HashSet<int>(user.budynki.Select(b => b.id_budynek));
            var viewModelAvailable = new List<UsersBuildingsVM>();
            var viewModelSelected = new List<UsersBuildingsVM>();
            foreach (var wm in allBuildings)
            {
                if (usersWM.Contains(wm.id_budynek))
                {
                    viewModelSelected.Add(new UsersBuildingsVM
                    {
                        BudID = wm.id_budynek,
                        BudName = wm.nazwa_wspolnoty,
                        //Assigned = true
                    });
                }
                else
                {
                    viewModelAvailable.Add(new UsersBuildingsVM
                    {
                        BudID = wm.id_budynek,
                        BudName = wm.nazwa_wspolnoty,
                        //Assigned = false
                    });
                }
            }

            ViewBag.selOpt = new MultiSelectList(viewModelSelected, "BudID", "BudName");
            ViewBag.availOpts = new MultiSelectList(viewModelAvailable, "BudID", "BudName");
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
