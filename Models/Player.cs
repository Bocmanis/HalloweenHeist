using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloweenHeist.Models
{
    public class Player
    {
        public Guid? UniqueId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Alias { get; set; }
        public int Id { get; set; }
        public GameStage GameStage { get; set; }
        public int RicketyBridgeId { get; set; }
        public int EinsteinsPuzzleId { get; set; }
        public Drink EinteinsAnswer { get; set; }
        public RicketyBridge RicketyBridge { get; set; }
        public EinteinsPuzzle EinteinsPuzzle { get; set; }

        public string Stage3Answer { get; set; }
    }
}