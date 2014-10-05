using Loria.Module.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Module.Launcher
{
    public class Program : ILoriaAction
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Start(args);
        }

        public void Start(string[] args)
        {
            LoriaModule loriaModule = new LoriaModule();
            loriaModule.Ask(args, this);
        }

        public string Ask(LoriaAction loriaAction)
        {
            string answer = null;

            try
            {
                using (var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = loriaAction.Program,
                        Arguments = loriaAction.Arguments,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                })
                {
                    process.Start();
                }

                answer = string.Format("C'est fait !");
            }
            catch (Exception)
            {
                answer = "Je n'arrive pas à lancer le programme.";
            }

            return answer;
        }
    }
}
