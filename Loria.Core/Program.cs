using Loria.Dal;
using Loria.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core
{
    public class Program
    {
        static List<Receipe> Receipes = new List<Receipe>();

        static void Main(string[] args)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<Trigger> triggers = db.Triggers.ToList();
                List<KeyValuePair<Guid, Process>> triggersProcess = new List<KeyValuePair<Guid, Process>>();

                foreach (Trigger trigger in triggers)
                {
                    Process triggerProcess = new Process()
                    {
                        StartInfo = new ProcessStartInfo()
                        {
                            FileName = string.Format("{0}.exe", trigger.Id),
                            RedirectStandardOutput = true
                        }
                    };
                    triggerProcess.OutputDataReceived += triggerProcess_OutputDataReceived;
                    triggerProcess.Start();

                    triggersProcess.Add(new KeyValuePair<Guid, Process>(trigger.Id, triggerProcess));
                }

                Receipes.AddRange(db.Receipes.Include(x => x.ReceipeIns)
                                             .Include(x => x.ReceipeOuts)
                                             .Include(x => x.ReceipeOuts.Select(r => r.ActionItem)));
            }
        }

        static void triggerProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            List<Receipe> receipesTriggered = Receipes.Where(x => x.ReceipeIns.Any(r => r.Value == e.Data)).ToList();
            
            foreach (Receipe receipe in receipesTriggered)
            {
                foreach (ReceipeOut receipeOut in receipe.ReceipeOuts)
                {
                    Console.WriteLine("Receipe triggered: " + receipe.Name);
                }
            }
        }
    }
}
