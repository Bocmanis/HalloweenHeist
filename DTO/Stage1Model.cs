using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloweenHeist.DTO
{
    public class Stage1Model
    {
        public Guid UserId { get; set; }
        public int Answer { get; set; }
        public string TaskText { get; set; }
    }
}