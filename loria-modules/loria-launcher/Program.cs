using Loria.Module.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Module.Launcher
{
    public class Program : ILoriaActionHandler
    {
        static void Main(string[] args)
        {
            LoriaModule loriaModule = new LoriaModule();
            loriaModule.Start(new Program());
        }

        public IEnumerable<string> OnDemand(LoriaAction loriaAction, System.IO.FileInfo databaseFile)
        {
            List<string> answers = new List<string>();

            try
            {
                var programAttribute = loriaAction.AdditionalAttributes.FirstOrDefault(kvp => kvp.Key == "program");
                var argumentsAttribute = loriaAction.AdditionalAttributes.FirstOrDefault(kvp => kvp.Key == "arguments");

                if (!programAttribute.Equals(default(KeyValuePair<string, string>)))
                {
                    using (var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = programAttribute.Value,
                            Arguments = !argumentsAttribute.Equals(default(KeyValuePair<string, string>)) ? argumentsAttribute.Value : "",
                            UseShellExecute = false,
                            CreateNoWindow = true
                        }
                    })
                    {
                        process.Start();
                    }
                }

                answers.Add(string.Format("C'est fait !"));
            }
            catch (Exception)
            {
                answers.Add("Je n'arrive pas à lancer le programme.");
            }

            return answers;
        }

        public void InsideLoop(LoriaAction loriaAction, System.IO.FileInfo databaseFile)
        {
            throw new NotImplementedException();
        }
    }
}
