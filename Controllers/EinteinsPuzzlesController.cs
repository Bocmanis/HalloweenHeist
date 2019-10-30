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

        [HttpGet, ActionName("Seed")]
        public ActionResult Seed()
        {
            var drinks = Enum.GetValues(typeof(Drink)).Cast<Drink>();
            var hobbies = Enum.GetValues(typeof(Hobby)).Cast<Hobby>();
            var names = Enum.GetValues(typeof(Name)).Cast<Name>();
            var shirtColours = Enum.GetValues(typeof(ShirtColor)).Cast<ShirtColor>();
            var nationalities = Enum.GetValues(typeof(Nationality)).Cast<Nationality>();

            for (int i = 0; i < 100; i++)
            {
                List<EinteinsPuzzle> puzzleSet = GetPuzzleSet(drinks, hobbies, names, shirtColours, nationalities);

                db.EinteinsPuzzles.AddRange(puzzleSet);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        private static List<EinteinsPuzzle> GetPuzzleSet(IEnumerable<Drink> drinks, IEnumerable<Hobby> hobbies, IEnumerable<Name> names, IEnumerable<ShirtColor> shirtColours, IEnumerable<Nationality> nationalities)
        {
            drinks = drinks.OrderBy(x => Guid.NewGuid()).ToList();
            hobbies = hobbies.OrderBy(x => Guid.NewGuid()).ToList();
            names = names.OrderBy(x => Guid.NewGuid()).ToList();
            shirtColours = shirtColours.OrderBy(x => Guid.NewGuid()).ToList();
            nationalities = nationalities.OrderBy(x => Guid.NewGuid()).ToList();

            var puzzleSet = new List<EinteinsPuzzle>();
            var id = Guid.NewGuid();

            for (int i = 0; i < 5; i++)
            {
                var einsteins = new EinteinsPuzzle()
                {
                    Drink = drinks.ElementAt(i),
                    Hobby = hobbies.ElementAt(i),
                    Nationality = nationalities.ElementAt(i),
                    ShirtColor = shirtColours.ElementAt(i),
                    Name = names.ElementAt(i),
                    PuzzleId = id,
                    Position = i,
                };
                puzzleSet.Add(einsteins);
            }

            return puzzleSet;
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
