using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SystemADM.Models;

namespace SystemADM.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private systemADMEntities1 db = new systemADMEntities1();

        // GET: Notes
        public ActionResult Index()
        {
            int id = int.Parse(Session["id_uzytkownik"].ToString());
            User user = db.users.Find(id);

            var usersBuildings = user.budynki.ToList();
            int[] id_list = usersBuildings.Select(x => x.id_budynek).ToArray();

            var notatki = db.notatki
                .Include(n => n.users)
                .Include(n => n.zgloszenia)
                .Where(t => id_list.Contains(t.zgloszenia.id_budynek));
                      

            notatki = notatki.OrderByDescending(z => z.data_wpisania);

            return View(notatki.ToList());
        }

        // GET: Notes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.notatki.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // GET: Notes/Create
        public ActionResult Create(int? id)
        {
            int userId = int.Parse(Session["id_uzytkownik"].ToString());
   
            var zgloszenia = db.zgloszenia
                .Where(t => t.id_uzytkownik == userId);

            var user = db.users
                .Where(t => t.id_uzytkownik == userId);
         
            ViewBag.id_uzytkownik = new SelectList(user, "id_uzytkownik", "FullName");
            ViewBag.id_zgloszenia = new SelectList(zgloszenia, "id_zgloszenia", "temat");
            return View();
        }
     

        // POST: Notes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_uzytkownik,id_zgloszenia,tresc")] Note note)
        {
          
            if (ModelState.IsValid)
            {
                db.notatki.Add(note);
                db.SaveChanges();
                return RedirectToAction("Details/"+note.id_zgloszenia.ToString(),"Tickets");
            }

            ViewBag.id_uzytkownik = new SelectList(db.users, "id_uzytkownik", "imie", note.id_uzytkownik);
            ViewBag.id_zgloszenia = new SelectList(db.zgloszenia, "id_zgloszenia", "temat", note.id_zgloszenia);

            Ticket ticket = db.zgloszenia.Find(note.id_zgloszenia);
            return View(ticket);
        }

        // GET: Notes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.notatki.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_uzytkownik = new SelectList(db.users, "id_uzytkownik", "imie", note.id_uzytkownik);
            ViewBag.id_zgloszenia = new SelectList(db.zgloszenia, "id_zgloszenia", "temat", note.id_zgloszenia);
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_notatki,tresc,data_wpisania")] Note note)
        {
            if (ModelState.IsValid)
            {
                db.Entry(note).State = EntityState.Modified;
                db.Entry(note).Property(x => x.id_zgloszenia).IsModified = false;
                db.Entry(note).Property(x => x.id_uzytkownik).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_uzytkownik = new SelectList(db.users, "id_uzytkownik", "imie", note.id_uzytkownik);
            ViewBag.id_zgloszenia = new SelectList(db.zgloszenia, "id_zgloszenia", "temat", note.id_zgloszenia);
            return View(note);
        }

        // GET: Notes/Delete/5
        [Authorize(Roles = "admin, super_admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.notatki.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = db.notatki.Find(id);
            db.notatki.Remove(note);
            db.SaveChanges();
            return RedirectToAction("Index");
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
