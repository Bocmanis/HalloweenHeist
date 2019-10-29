using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloweenHeist.Models
{
    public class RicketyBridge
    {
        public int Id { get; set; }
        public int WrongAnswer { get; set; }
        public int CorrectAnswer { get; set; }
        public int Speedster1 { get; set; }
        public int Speedster2 { get; set; }
        public int SlowPoke1 { get; set; }
        public int SlowPoke2 { get; set; }
    }
}