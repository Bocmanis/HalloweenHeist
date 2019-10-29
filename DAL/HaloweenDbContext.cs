using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using HaloweenHeist.Models;

namespace HaloweenHeist.DAL
{
    public class HaloweenDbContext : DbContext
    {
        public HaloweenDbContext() : base("HaloweenDbContext")
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<EinteinsPuzzle> EinteinsPuzzles { get; set; }
        public DbSet<RicketyBridge> RicketyBridges { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}