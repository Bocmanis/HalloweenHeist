using HaloweenHeist.DAL;
using HaloweenHeist.DTO;
using HaloweenHeist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaloweenHeist.Controllers
{
    public class HalloweenHeistController : Controller
    {
        private HaloweenDbContext db = new HaloweenDbContext();

        public ActionResult Index()
        {
            var playerId = Request.Cookies.Get("PlayerId").Value;
            if (string.IsNullOrEmpty(playerId))
            {
                return View();
            }

            var playerGuid = new Guid(playerId);
            var player = db.Players.FirstOrDefault(x => x.UniqueId == playerGuid);

            return View(player);
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Name,Surname,Alias")] Player player)
        {
            //This done nothing
            if (player == null)
            {
                return View(player);
            }

            if (player.UniqueId != null && player.UniqueId != Guid.Empty)
            {
                Response.Cookies.Add(new HttpCookie("PlayerId", player.UniqueId.ToString()));
                return RedirectToAction("Stage1");
            }

            if (ModelState.IsValid)
            {
                player.UniqueId = Guid.NewGuid();
                player.EinsteinsPuzzleId = RandomHolder.Random.Next(10, 490);
                player.EinteinsAnswer = GetEinsteinsAnswer(player.EinsteinsPuzzleId);
                player.RicketyBridgeId = RandomHolder.Random.Next(4, 89);
                player.GameStage = GameStage.Start;
                Response.Cookies.Add(new HttpCookie("PlayerId", player.UniqueId.ToString()));
                db.Players.Add(player);
                db.SaveChanges();
                return RedirectToAction("Stage1");
            }

            return View(player);
        }

        public ActionResult Stage1()
        {
            if (PlayerFailed())
            {
                return RedirectToAction("Index");
            }

            var playerId = Request.Cookies.Get("PlayerId").Value;
            var stage = new Stage1Model()
            {
                UserId = new Guid(playerId),
                TaskText = "You have to venture into the darkness with nothing but the torch"
            };
            return View(stage);
        }

        private bool PlayerFailed()
        {
            var playerId = Request.Cookies.Get("PlayerId").Value;
            var playerGuid = new Guid(playerId);
            var player = db.Players.FirstOrDefault(x => x.UniqueId == playerGuid);
            if (player == null || player.GameStage == GameStage.Lost)
            {
                return true;
            }
            return false;
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Stage1([Bind(Include = "Answer")] Stage1Model stage)
        {
            if (PlayerFailed())
            {
                return RedirectToAction("Index");
            }

            //This done nothing
            if (stage == null)
            {
                return View(stage);
            }

            var playerId = Request.Cookies.Get("PlayerId").Value;
            var playerGuid = new Guid(playerId);
            var player = db.Players.First(x => x.UniqueId == playerGuid);
            var correctAnswer = db.RicketyBridges.First(x => x.Id == player.RicketyBridgeId).CorrectAnswer;
            if (correctAnswer == stage.Answer)
            {
                player.GameStage = GameStage.FirstDone;
            }
            else
            {
                player.GameStage = GameStage.Lost;
            }
            db.SaveChanges();

            return RedirectToAction("Stage2");
        }
        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Stage2([Bind(Include = "Answer")] Stage2Model stage)
        {
            //This done nothing
            if (stage == null)
            {
                return View(stage);
            }
            var playerId = Request.Cookies.Get("PlayerId").Value;
            var playerGuid = new Guid(playerId);
            var player = db.Players.First(x => x.UniqueId == playerGuid);
            if (player.EinteinsAnswer.ToString() == stage.Answer)
            {
                player.GameStage = GameStage.SecondDone;
            }
            else
            {
                player.GameStage = GameStage.Lost;
            }
            db.SaveChanges();

            return RedirectToAction("Stage3");
        }

        public ActionResult Stage2()
        {
            if (PlayerFailed())
            {
                return RedirectToAction("Index");
            }

            var playerId = Request.Cookies.Get("PlayerId").Value;
            var stage = new Stage2Model()
            {
                UserId = new Guid(playerId),
                TaskText = "Many many texts"
            };
            return View(stage);
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Stage3([Bind(Include = "Answer")] Stage3Model stage)
        {
            //This done nothing
            if (stage == null)
            {
                return View(stage);
            }
            var playerId = Request.Cookies.Get("PlayerId").Value;
            var playerGuid = new Guid(playerId);
            var player = db.Players.First(x => x.UniqueId == playerGuid);
            player.GameStage = GameStage.Finished;
            player.Stage3Answer = stage.Answer;
            db.SaveChanges();

            return RedirectToAction("Stage3");
        }

        public ActionResult Stage3()
        {
            if (PlayerFailed())
            {
                return RedirectToAction("Index");
            }

            var playerId = Request.Cookies.Get("PlayerId").Value;
            var stage = new Stage3Model()
            {
                UserId = new Guid(playerId),
                TaskText =
@"Many many texts wdqwd qwdqw dqwdqwdqwdqwdqw dqw dqwd qwdqw dqwdq
qwdqwdqwdqwdqwd
qwd
qw
d
        qwdqwdqwdqw
            dqwdqwdqwd
qwd
qwdqwdwdqwd
qwdqwdddqwqwdw dqwd"
            };
            return View(stage);
        }

        private Drink GetEinsteinsAnswer(int einsteinsPuzzleId)
        {
            var einsteins = db.EinteinsPuzzles.First(x => x.Id == einsteinsPuzzleId);
            var puzzleSet = db.EinteinsPuzzles.Where(x => x.PuzzleId == einsteins.PuzzleId).ToList();
            return puzzleSet[3].Drink;
        }
    }
}