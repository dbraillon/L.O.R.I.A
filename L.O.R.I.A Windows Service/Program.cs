using System.ServiceProcess;

namespace L.O.R.I.A_Windows_Service
{
    static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new SpeechRecognitionService(),
                new TextToSpeechService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
