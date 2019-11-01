using HaloweenHeist.DAL;
using HaloweenHeist.DTO;
using HaloweenHeist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HaloweenHeist.Controllers
{
    public class HalloweenHeistController : Controller
    {
        private HaloweenDbContext db = new HaloweenDbContext();

        public ActionResult Index()
        {
            var playerIdCookie = Request.Cookies.Get("PlayerId");
            if (playerIdCookie == null || string.IsNullOrEmpty(playerIdCookie.Value))
            {
                var emptyPlayer = new Player() { GameStage = GameStage.None };
                return View(emptyPlayer);
            }

            var playerGuid = new Guid(playerIdCookie.Value);
            var player = db.Players.FirstOrDefault(x => x.UniqueId == playerGuid);
            if (player == null)
            {
                var emptyPlayer = new Player() { GameStage = GameStage.None };
                return View(emptyPlayer);
            }
            return View(player);
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Name,Surname,Alias,UniqueId")] Player player)
        {
            //This done nothing
            if (player == null)
            {
                return View(player);
            }

            if (player.UniqueId != null && player.UniqueId != Guid.Empty)
            {
                Response.Cookies.Add(new HttpCookie("PlayerId", player.UniqueId.ToString()));
                var stage = GetGameStage();
                switch (stage)
                {
                    case GameStage.Start:
                        return RedirectToAction("Stage1");

                    case GameStage.FirstDone:
                        return RedirectToAction("Stage2");

                    case GameStage.SecondDone:
                        return RedirectToAction("Stage3");

                    case GameStage.Lost:
                    case GameStage.Finished:
                        break;
                    default:
                        return RedirectToAction("Index");
                }
            }

            if (ModelState.IsValid)
            {
                player.UniqueId = Guid.NewGuid();
                var smallestId = db.EinteinsPuzzles.Min(x => x.Id);
                player.EinsteinsPuzzleId = smallestId + RandomHolder.Random.Next(10, 490);
                player.EinteinsAnswer = GetEinsteinsAnswer(player.EinsteinsPuzzleId);
                var smallestRicketyBridgeId = db.RicketyBridges.Min(x => x.Id);
                player.RicketyBridgeId = smallestRicketyBridgeId + RandomHolder.Random.Next(4, 89);
                player.GameStage = GameStage.Start;
                player.StartTime = DateTime.Now;
                Response.Cookies.Add(new HttpCookie("PlayerId", player.UniqueId.ToString()));
                db.Players.Add(player);
                db.SaveChanges();
                return RedirectToAction("Stage1");
            }

            return View(player);
        }

        public ActionResult Stage1()
        {
            if (GetGameStage() != GameStage.Start)
            {
                return RedirectToAction("Index");
            }

            var playerId = Request.Cookies.Get("PlayerId").Value;
            var playerGuid = new Guid(playerId);
            var player = db.Players.First(x => x.UniqueId == playerGuid);
            var ricketyBridge = db.RicketyBridges.First(x => x.Id == player.RicketyBridgeId);

            var stage = new Stage1Model()
            {
                UserId = new Guid(playerId),
                TaskText = $@"
{Environment.NewLine}Four travelers are going to their friend house. As it is getting late, they decide to take a shortcut that is a long, long dark tunnel
{Environment.NewLine} with many turns and bends. Sadly, they are not completely prepared, and brought only one phone with flashlight. 
{Environment.NewLine}The light from it is too little for it to be safe to have 2 walk together at same time.(2 can cross, and then one them has to bring the light back)
They each can move at different speed, which are given here: {ricketyBridge.Speedster1} min, {ricketyBridge.Speedster2} min,  {ricketyBridge.SlowPoke1} min, {ricketyBridge.SlowPoke2} min| {Environment.NewLine}
What is the fastest time they all can get through the tunnel?
{Environment.NewLine}Hint: its faster than {ricketyBridge.WrongAnswer} min."
            };
            return View(stage);
        }

        private GameStage GetGameStage()
        {
            var playerIdCookie = Request.Cookies.Get("PlayerId");
            if (playerIdCookie == null)
            {
                return GameStage.None;
            }
            var playerGuid = new Guid(playerIdCookie.Value);
            var player = db.Players.FirstOrDefault(x => x.UniqueId == playerGuid);

            return player.GameStage;
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Stage1([Bind(Include = "Answer")] Stage1Model stage)
        {
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
            if (GetGameStage() != GameStage.FirstDone)
            {
                return RedirectToAction("Stage1");
            }

            var playerId = Request.Cookies.Get("PlayerId").Value;
            var playerGuid = new Guid(playerId);
            var einsteinsPuzzleId = db.Players.First(x => x.UniqueId == playerGuid).EinsteinsPuzzleId;
            var einsteins = db.EinteinsPuzzles.First(x => x.Id == einsteinsPuzzleId);
            var puzzleSet = db.EinteinsPuzzles.Where(x => x.PuzzleId == einsteins.PuzzleId).ToList();

            var stage = new Stage2Model()
            {
                UserId = new Guid(playerId),
                TaskText = GetTaskTwoText(puzzleSet),
            };

            return View(stage);
        }

        private string GetTaskTwoText(List<EinteinsPuzzle> puzzleSet)
        {
            var sb = new StringBuilder();
            sb.Append("The four travelers meet their friend and all of them decide to celebrate. However, in the morning, they discover that one of them has been poisoned.\n They all put their heads togehter to recall events from previous night, in order to find out which type of the drinks was poisoned.\n Luckily, each of them chose a different drink that night. Also, they sat on the couch in a row.");
            sb.AppendLine($"These are the clues they gathered.\n Help them to find out which drink was poisoned and killed their friend {puzzleSet[3].Name} \n");

            sb.AppendLine($"The person who is {puzzleSet[2].Nationality} wore {puzzleSet[2].ShirtColor} shirt");
            sb.AppendLine($"{puzzleSet[4].Name} is {puzzleSet[4].Nationality}");
            sb.AppendLine($"The person who is {puzzleSet[1].Nationality} drank {puzzleSet[1].Drink}");
            sb.AppendLine($"The person wearing {puzzleSet[3].ShirtColor} shirt sat on the left of person wearing {puzzleSet[4].ShirtColor} shirt");
            sb.AppendLine($"The person wearing {puzzleSet[3].ShirtColor} drank {puzzleSet[3].Drink}");
            sb.AppendLine($"{puzzleSet[2].Name} enjoys {puzzleSet[2].Hobby}");
            sb.AppendLine($"The person who enjoys {puzzleSet[0].Hobby} wore {puzzleSet[0].ShirtColor} shirt");
            sb.AppendLine($"The person sitting is the middle drank {puzzleSet[2].Drink}");
            sb.AppendLine($"The person who is {puzzleSet[0].Nationality} sat one the leftmost side");
            sb.AppendLine($"The person who enjoys {puzzleSet[1].Hobby} sat next to {puzzleSet[0].Name}");
            sb.AppendLine($"{puzzleSet[1].Name} sat next to the person who enjoys {puzzleSet[0].Hobby}");
            sb.AppendLine($"The person who enjoys {puzzleSet[4].Hobby} drank {puzzleSet[4].Drink}");
            sb.AppendLine($"The person who is {puzzleSet[3].Nationality} enjoys {puzzleSet[3].Hobby}");
            sb.AppendLine($"The person who is {puzzleSet[0].Nationality} sat next to person wearing {puzzleSet[1].ShirtColor} shirt");
            sb.AppendLine($"The person who enjoys {puzzleSet[1].Hobby} sat next to a person who drank {puzzleSet[0].Drink}");

            return sb.ToString();
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
            player.EndTime = DateTime.Now;
            db.SaveChanges();

            return RedirectToAction("Stage3");
        }

        public ActionResult Stage3()
        {
            if (GetGameStage() != GameStage.SecondDone)
            {
                return RedirectToAction("Stage2");
            }

            var playerId = Request.Cookies.Get("PlayerId").Value;
            var playerGuid = new Guid(playerId);
            var player = db.Players.First(x => x.UniqueId == playerGuid);
            var stage = new Stage3Model()
            {
                UserId = new Guid(playerId),
                TaskText = player.EinteinsAnswer.ToString(),
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