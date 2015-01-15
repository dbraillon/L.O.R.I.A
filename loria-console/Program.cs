using Loria.Core.Debug;

namespace Loria.Console
{
    class Program : ILoggable
    {
        static void Main(string[] args)
        {
            using (LoriaCore lCore = new LoriaCore(new Program()))
            {
                lCore.StartListening();

                System.Console.Read();
            }
        }

        public void WriteLog(LogType logType, string logMessage, params object[] logVariables)
        {
            System.Console.WriteLine("{0} {1}",
                logType == LogType.INFO ? "[inf]" :
                logType == LogType.WARNING ? "[warn]" :
                logType == LogType.ERROR ? "[err]" :
                "", string.Format(logMessage, logVariables));
        }
    }
}
