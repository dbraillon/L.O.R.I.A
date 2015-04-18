using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core.Debug
{
    public enum LogType
    {
        INFO, WARNING, ERROR
    }

    public interface ILoggable
    {
        void WriteLog(LogType logType, string logMessage, params object[] logVariables);
    }
}
