using Loria.Core.Debug;
using Loria.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Loria.Core.Loria.Module.Config
{
    public class LoriaConfig
    {
        private ILoggable Loggable;
        private XmlDocument ConfigDocument;

        public LoriaConfig(string configDocumentPath, ILoggable log)
        {
            ConfigDocument = new XmlDocument();
            ConfigDocument.Load(configDocumentPath);
        }

        public IEnumerable<LoriaModule> LoadModules()
        {
            
        }

        private void Log(LogType logType, string logMessage, params object[] logVariables)
        {
            if (Loggable != null)
            {
                Loggable.WriteLog(logType, logMessage, logVariables);
            }
        }
    }
}
