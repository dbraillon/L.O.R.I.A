using Loria.Core.Debug;
using Loria.Core.Loria.Module.LoriaActions;
using Loria.Core.src.Loria.Module.CoreModules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Speech.Recognition;

namespace Loria.Module
{
    public class LoriaModuleLoader
    {
        private ILoggable LogManager;

        public List<LoriaModule> LoriaModules { get; set; }

        public LoriaModuleLoader(ILoggable logManager = null)
        {
            LogManager = logManager;

            LoriaModules = new List<LoriaModule>();
        }

        public void LoadModules()
        {
            if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Load Loria modules.");

            LoriaModules.Clear();

            // Core modules
            LoriaCoreModule loriaCoreModule = new LoriaCoreModule();
            loriaCoreModule.LoadConfigFile();
            LoriaModules.Add(loriaCoreModule);

            // Modules
            Directory.CreateDirectory("modules");
            foreach (string moduleDirectoryPath in Directory.EnumerateDirectories("modules"))
            {
                try
                {
                    if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Found a module named '{0}', trying to load it.", Path.GetFileName(moduleDirectoryPath));

                    string configFilePath = Path.Combine(moduleDirectoryPath, "config.xml");
                    string databaseFilePath = Path.Combine(moduleDirectoryPath, "database.xml");
                    string programFilePath = Path.Combine(moduleDirectoryPath, string.Format("{0}.exe", Path.GetFileName(moduleDirectoryPath)));

                    LoriaModule loriaModule = new LoriaModule(LogManager, new FileInfo(configFilePath), new FileInfo(databaseFilePath), new FileInfo(programFilePath));
                    loriaModule.LoadConfigFile();

                    LoriaModules.Add(loriaModule);
                }
                catch (Exception e) 
                {
                    if (LogManager != null) LogManager.WriteLog(LogType.ERROR, "Can't load the module '{0}'.", e.ToString());
                }
            }
        }

        public IEnumerable<string> GetPhrases()
        {
            var loriaActionOnDemands = LoriaModules.SelectMany(m => m.LoriaActions).Where(a => a is LoriaActionOnDemand).Cast<LoriaActionOnDemand>();

            return loriaActionOnDemands.SelectMany(a => a.Phrases);
        }
    }
}
