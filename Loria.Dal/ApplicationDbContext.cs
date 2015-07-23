using Loria.Dal.Entities;
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

        public DbSet<Action> Actions { get; set; }
        public DbSet<ActionItem> ActionItems { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Receipe> Receipes { get; set; }
        public DbSet<ReceipeIn> ReceipeIns { get; set; }
        public DbSet<ReceipeOut> ReceipeOuts { get; set; }
        public DbSet<Trigger> Triggers { get; set; }
        public DbSet<TriggerItem> TriggerItems { get; set; }
    }
}
