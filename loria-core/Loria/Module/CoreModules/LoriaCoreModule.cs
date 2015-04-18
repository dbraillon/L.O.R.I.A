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
    }
}
