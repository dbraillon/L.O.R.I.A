using Loria.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Dal
{
    public class ApplicationDbContext : DbContext
    {
        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public ApplicationDbContext() : base("LoriaDbConnection") { }

        public DbSet<Ability> Abilities { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Stimuli> Stimulus { get; set; }
    }
}
