using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Recognition;

namespace Loria.Module
{
    public class LoriaModuleLoader
    {
        public List<LoriaModule> LoriaModules { get; set; }

        public LoriaModuleLoader()
        {
            LoriaModules = new List<LoriaModule>();

            Directory.CreateDirectory("modules");
            foreach (string moduleDirectoryPath in Directory.EnumerateDirectories("modules"))
            {
                try
                {
                    string xmlFilePath = Path.Combine(moduleDirectoryPath, "Phrases.xml");
                    string programFilePath = Path.Combine(moduleDirectoryPath, string.Format("{0}.exe", Path.GetFileName(moduleDirectoryPath)));

                    LoriaModules.Add(new LoriaModule(new FileInfo(xmlFilePath), new FileInfo(programFilePath)));
                }
                catch (Exception) { }
            }
        }

        public Choices GetChoices()
        {
            Choices choices = new Choices();

            foreach (LoriaModule loriaModule in LoriaModules)
                choices.Add(loriaModule.Phrases.ToArray());

            return choices;
        }

        public LoriaModule GetModule(string phrase)
        {
            return LoriaModules.FirstOrDefault(m => m.Phrases.Contains(phrase));
        }
    }
}
