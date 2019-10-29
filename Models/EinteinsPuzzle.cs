using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloweenHeist.Models
{
    public class EinteinsPuzzle
    {
        public int Id { get; set; }
        public Guid PuzzleId { get; set; }
        public Drink Drink { get; set; }
        public ShirtColor ShirtColor { get; set; }
        public Nationality Nationality { get; set; }
        public Name Name { get; set; }
        public Hobby Hobby { get; set; }
        public int Position { get; set; }
    }
}