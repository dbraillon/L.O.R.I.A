using Loria.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Dal.Tools
{
    public static class LoriaTool
    {
        public static string[] GetAllStimulus(Sense? senses = null, EventLog eventLog = null)
        {
            List<string> stimulus = new List<string>();

            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    if (senses == null)
                    {
                        stimulus.AddRange(db.Stimulus.Select(s => s.Value));
                    }
                    else
                    {
                        stimulus.AddRange(db.Abilities.Where(a => a.Senses.HasFlag(senses))
                                                      .SelectMany(a => a.Stimulus)
                                                      .Select(s => s.Value));
                    }
                }
            }
            catch (Exception e)
            {
                eventLog.WriteEntry(string.Format("Can't access database.{0}{1}", Environment.NewLine, e.ToString()), EventLogEntryType.Error);
            }
            
            return stimulus.ToArray();
        }
        
        public static Ability GetAbility(string stimuli)
        {
            Ability ability = null;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ability = db.Abilities.Include(a => a.Skills)
                                      .FirstOrDefault(a => a.Stimulus.Any(s => s.Value == stimuli));
            }

            return ability;
        }
    }
}
