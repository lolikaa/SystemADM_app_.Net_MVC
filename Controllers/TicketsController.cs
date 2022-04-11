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
    public class TicketsController : Controller
    {
        private systemADMEntities1 db = new systemADMEntities1();

        // GET: Tickets
        public ActionResult Index(string searchString)
        {
           
            int id = int.Parse(Session["id_uzytkownik"].ToString());
            User user = db.users.Find(id);

            var usersBuildings = user.budynki.ToList();
            int[] id_list = usersBuildings.Select(x => x.id_budynek).ToArray();

            if (String.IsNullOrEmpty(searchString))
            {
                var zgloszenia = db.zgloszenia
                .Include(t => t.budynki)
                .Include(t => t.statusy)
                .Include(t => t.users)
                .Where(t => id_list.Contains(t.id_budynek) && t.id_status !=4);
              
                zgloszenia = zgloszenia.OrderByDescending(z => z.data_zgloszenia);
                return View(zgloszenia.ToList());
            }
            else
            {
                var zgloszenia = db.zgloszenia.Include(t => t.budynki)
                 .Include(t => t.statusy).Include(t => t.users)
                 .Where(t => id_list.Contains(t.id_budynek) && t.temat.Contains(searchString));

                return View(zgloszenia.ToList());

            }
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.zgloszenia.Find(id);
            ViewBag.id_status = new SelectList(db.statusy, "id_status", "nazwa" ,ticket.id_status);

            var currentUser = int.Parse(Session["id_uzytkownik"].ToString());
            User user = db.users.Find(currentUser);

            ViewBag.id_uzytkownik = new SelectList(db.users, "id_uzytkownik", "FullName");

            if (ticket.id_uzytkownik != currentUser)
            {
                ViewBag.Message = "Nie masz uprawnień do zmiany tego zgłoszenia";
                RedirectToAction("Tickets", "Index");
            }

            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            int id = int.Parse(Session["id_uzytkownik"].ToString());
            User user = db.users.Find(id);

            var usersBuildings = user.budynki.ToList();

            ViewBag.id_budynek = new SelectList(usersBuildings, "id_budynek", "nazwa_wspolnoty");
            ViewBag.id_status = new SelectList(db.statusy, "id_status", "nazwa");
            ViewBag.id_uzytkownik = new SelectList(db.users, "id_uzytkownik", "imie");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_zgloszenia,id_status,id_uzytkownik,id_budynek,temat,opis,data_zgloszenia,data_wykonania,planowana_data_wykonania")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.zgloszenia.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_budynek = new SelectList(db.budynki, "id_budynek", "nazwa_wspolnoty", ticket.id_budynek);
            ViewBag.id_status = new SelectList(db.statusy, "id_status", "nazwa", ticket.id_status);
            ViewBag.id_uzytkownik = new SelectList(db.users, "id_uzytkownik", "imie", ticket.id_uzytkownik);
            return View(ticket);
        }


        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.zgloszenia.Find(id);

            ViewBag.id_status = ticket.id_status;

            var currentUser = Session["id_uzytkownik"].ToString();
            if (ticket.id_uzytkownik.ToString() != currentUser)
            {
                ViewBag.Message = "Nie masz uprawnień do zmiany tego zgłoszenia";
                return RedirectToAction("Index", "Tickets");
            }
            else
            {

                if (ticket == null)
                {
                    return HttpNotFound();
                }
                ViewBag.id_budynek = new SelectList(db.budynki, "id_budynek", "nazwa_wspolnoty", ticket.id_budynek);
                ViewBag.id_status = new SelectList(db.statusy, "id_status", "nazwa", ticket.id_status);
                ViewBag.id_uzytkownik = new SelectList(db.users, "id_uzytkownik", "imie", ticket.id_uzytkownik);
                return View(ticket);
            }
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_zgloszenia,id_status,id_uzytkownik,id_budynek,temat,opis,data_wykonania,planowana_data_wykonania")] Ticket ticket)
        {

            if (ModelState.IsValid)
            {
              
                db.Entry(ticket).State = EntityState.Modified;
                db.Entry(ticket).Property(x => x.data_zgloszenia).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_budynek = new SelectList(db.budynki, "id_budynek", "nazwa_wspolnoty", ticket.id_budynek);
            ViewBag.id_status = new SelectList(db.statusy, "id_status", "nazwa", ticket.id_status);
            ViewBag.id_uzytkownik = new SelectList(db.users, "id_uzytkownik", "imie", ticket.id_uzytkownik);
            return View(ticket);
        }


        [HttpPost]
        public ActionResult UpdateStatus([Bind(Include = "id_zgloszenia, id_status")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.Entry(ticket).Property(x => x.id_uzytkownik).IsModified = false;
                db.Entry(ticket).Property(x => x.id_budynek).IsModified = false;
                db.Entry(ticket).Property(x => x.temat).IsModified = false;
                db.Entry(ticket).Property(x => x.opis).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_status = new SelectList(db.statusy, "id_status", "nazwa", ticket.id_status);
            return View(ticket);
        }

        [Authorize(Roles = "super_admin, admin")]
        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.zgloszenia.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        [Authorize(Roles = "super_admin, admin")]
        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.zgloszenia.Find(id);
            db.zgloszenia.Remove(ticket);
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
