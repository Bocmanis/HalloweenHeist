using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HaloweenHeist.DAL;
using HaloweenHeist.Models;

namespace HaloweenHeist.Controllers
{
    public class EinteinsPuzzlesController : Controller
    {
        private HaloweenDbContext db = new HaloweenDbContext();

        // GET: EinteinsPuzzles
        public ActionResult Index()
        {
            return View(db.EinteinsPuzzles.ToList());
        }

        // GET: EinteinsPuzzles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EinteinsPuzzle einteinsPuzzle = db.EinteinsPuzzles.Find(id);
            if (einteinsPuzzle == null)
            {
                return HttpNotFound();
            }
            return View(einteinsPuzzle);
        }

        // GET: EinteinsPuzzles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EinteinsPuzzles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PuzzleId,Drink,ShirtColor,Nationality,Name,Hobby,Position")] EinteinsPuzzle einteinsPuzzle)
        {
            if (ModelState.IsValid)
            {
                db.EinteinsPuzzles.Add(einteinsPuzzle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(einteinsPuzzle);
        }

        // GET: EinteinsPuzzles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EinteinsPuzzle einteinsPuzzle = db.EinteinsPuzzles.Find(id);
            if (einteinsPuzzle == null)
            {
                return HttpNotFound();
            }
            return View(einteinsPuzzle);
        }

        // POST: EinteinsPuzzles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PuzzleId,Drink,ShirtColor,Nationality,Name,Hobby,Position")] EinteinsPuzzle einteinsPuzzle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(einteinsPuzzle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(einteinsPuzzle);
        }

        // GET: EinteinsPuzzles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EinteinsPuzzle einteinsPuzzle = db.EinteinsPuzzles.Find(id);
            if (einteinsPuzzle == null)
            {
                return HttpNotFound();
            }
            return View(einteinsPuzzle);
        }

        // POST: EinteinsPuzzles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EinteinsPuzzle einteinsPuzzle = db.EinteinsPuzzles.Find(id);
            db.EinteinsPuzzles.Remove(einteinsPuzzle);
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
