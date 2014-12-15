using Loria.Module;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core.src.Loria.Module.CoreModules
{
    public enum CoreLoriaModuleType
    {
        VERSION, RELOAD_MODULE, LIST_MODULE, SLEEP, WAKEUP, NEWS
    }

    public class LoriaCoreModule : LoriaModule
    {
        public LoriaCoreModule()
        {
            ConfigFile = new FileInfo("config.xml");
        }

        public override IEnumerable<LoriaAnswer> DoAction(LoriaCore loriaCore, LoriaAction loriaAction)
        {
            List<LoriaAnswer> loriaAnswers = new List<LoriaAnswer>();

            if (loriaAction.Id.ToLower() == CoreLoriaModuleType.VERSION.ToString().ToLower())
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                string versionPhrase = string.Format("Je suis en version {0}.", fvi.FileVersion);

                loriaAnswers.Add(new LoriaAnswer(true, true, versionPhrase));
            }

            if (loriaAction.Id.ToLower() == CoreLoriaModuleType.RELOAD_MODULE.ToString().ToLower())
            {
                loriaCore.ReloadModule();
            }

            if (loriaAction.Id.ToLower() == CoreLoriaModuleType.LIST_MODULE.ToString().ToLower())
            {
                string moduleList = string.Join(", ", loriaCore.ModuleLoader.LoriaModules.Select(m => m.ModuleName));

                loriaAnswers.Add(new LoriaAnswer(true, true, string.Format("J'ai {0} modules chargés. {1}.", loriaCore.ModuleLoader.LoriaModules.Count, moduleList)));
            }

            if (loriaAction.Id.ToLower() == CoreLoriaModuleType.SLEEP.ToString().ToLower())
            {
                loriaCore.GoToSleep();
            }

            if (loriaAction.Id.ToLower() == CoreLoriaModuleType.WAKEUP.ToString().ToLower())
            {
                loriaCore.WakeUp();
            }

            if (loriaAction.Id.ToLower() == CoreLoriaModuleType.NEWS.ToString().ToLower())
            {
                foreach (LoriaModule loriaModule in loriaCore.ModuleLoader.LoriaModules)
                {
                    /*List<string> answers = loriaModule.Ask(GIVE_ME_NEWS_PHRASE).ToList();

                    foreach (string answer in answers)
                    {
                        Vocalizer.Speech(answer);
                        Thread.Sleep(500);
                    }*/
                }
            }

            return loriaAnswers;
        }
    }
}
