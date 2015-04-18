using Loria.Core.Debug;
using Loria.Core.src.Loria.Module.CoreModules;
using Loria.Module;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Loria.Core.Loria.Module.LoriaActions
{
    public class LoriaActionOnDemandCore : LoriaActionOnDemand
    {
        public LoriaActionOnDemandCore(LoriaModule loriaModule, XmlNode actionNode, ILoggable logManager = null)
            : base(loriaModule, actionNode, logManager)
        {

        }

        public override IEnumerable<LoriaAnswer> DoAction(LoriaCore loriaCore)
        {
            List<LoriaAnswer> loriaAnswers = new List<LoriaAnswer>();

            if (Id.ToLower() == CoreLoriaModuleType.VERSION.ToString().ToLower())
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                string versionPhrase = string.Format("Je suis en version {0}.", fvi.FileVersion);

                loriaAnswers.Add(new LoriaAnswer(true, true, versionPhrase));
            }

            if (Id.ToLower() == CoreLoriaModuleType.RELOAD_MODULE.ToString().ToLower())
            {
                loriaCore.ReloadModule();
            }

            if (Id.ToLower() == CoreLoriaModuleType.LIST_MODULE.ToString().ToLower())
            {
                string moduleList = string.Join(", ", loriaCore.ModuleLoader.LoriaModules.Select(m => m.ModuleName));

                loriaAnswers.Add(new LoriaAnswer(true, true, string.Format("J'ai {0} modules chargés. {1}.", loriaCore.ModuleLoader.LoriaModules.Count, moduleList)));
            }

            if (Id.ToLower() == CoreLoriaModuleType.SLEEP.ToString().ToLower())
            {
                loriaCore.GoToSleep();
            }

            if (Id.ToLower() == CoreLoriaModuleType.WAKEUP.ToString().ToLower())
            {
                loriaCore.WakeUp();
            }

            if (Id.ToLower() == CoreLoriaModuleType.NEWS.ToString().ToLower())
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
