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
    public class RicketyBridgesController : Controller
    {
        private HaloweenDbContext db = new HaloweenDbContext();

        [HttpGet, ActionName("Seed")]
        public ActionResult Seed()
        {
            var random = new Random();

            for (int i = 0; i < 100; i++)
            {
                var speedster1 = random.Next(20, 40);
                var speedSter2 = random.Next(3, 9) + speedster1;
                var slowPoke1 = speedster1 + speedSter2;
                var slowPoke2 = slowPoke1 + random.Next(9, 15);

                var correctAnswer = speedster1 + speedSter2 * 3 + slowPoke2;
                var wrongAnser = speedster1 * 2 + speedSter2 + slowPoke1 + slowPoke2;

                var ricketyBridge = new RicketyBridge()
                {
                    CorrectAnswer = correctAnswer,
                    SlowPoke1 = slowPoke1,
                    SlowPoke2 = slowPoke2,
                    Speedster1 = speedster1,
                    Speedster2 = speedSter2,
                    WrongAnswer = wrongAnser
                };

                if (correctAnswer > wrongAnser)
                {
                    throw new Exception($"What the fuck. Correct: {correctAnswer} Wrong: {wrongAnser} Numbers: {speedster1} {speedSter2} {slowPoke1} {slowPoke2}");
                }


                db.RicketyBridges.Add(ricketyBridge);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: RicketyBridges
        public ActionResult Index()
        {
            return View(db.RicketyBridges.ToList());
        }

        // GET: RicketyBridges/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RicketyBridge ricketyBridge = db.RicketyBridges.Find(id);
            if (ricketyBridge == null)
            {
                return HttpNotFound();
            }
            return View(ricketyBridge);
        }

        // GET: RicketyBridges/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RicketyBridges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,WrongAnswer,CorrectAnswer,Speedster1,Speedster2,SlowPoke1,SlowPoke2")] RicketyBridge ricketyBridge)
        {
            if (ModelState.IsValid)
            {
                db.RicketyBridges.Add(ricketyBridge);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ricketyBridge);
        }

        // GET: RicketyBridges/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RicketyBridge ricketyBridge = db.RicketyBridges.Find(id);
            if (ricketyBridge == null)
            {
                return HttpNotFound();
            }
            return View(ricketyBridge);
        }

        // POST: RicketyBridges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,WrongAnswer,CorrectAnswer,Speedster1,Speedster2,SlowPoke1,SlowPoke2")] RicketyBridge ricketyBridge)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ricketyBridge).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ricketyBridge);
        }

        // GET: RicketyBridges/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RicketyBridge ricketyBridge = db.RicketyBridges.Find(id);
            if (ricketyBridge == null)
            {
                return HttpNotFound();
            }
            return View(ricketyBridge);
        }

        // POST: RicketyBridges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RicketyBridge ricketyBridge = db.RicketyBridges.Find(id);
            db.RicketyBridges.Remove(ricketyBridge);
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
