using Loria.Core.Debug;
using Loria.Core.Loria.Module.LoriaActions;
using Loria.Core.src.Loria.Module;
using Loria.Core.src.Loria.Module.CoreModules;
using Loria.Module;
using Loria.Speech;
using Loria.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Loria
{
    public class LoriaCore : IDisposable
    {
        private ILoggable LogManager;
        private Thread LoriaThread;

        public LoriaModuleLoader ModuleLoader { get; set; }
        public LoriaRecognizer Recognizer { get; set; }
        public LoriaVocalizer Vocalizer { get; set; }

        public bool IsRunning { get; set; }
        public bool IsAsleep { get; set; }

        public LoriaCore(ILoggable logManager)
        {
            LogManager = logManager;

            Vocalizer = new LoriaVocalizer(logManager);
            Vocalizer.Speech("Bonjour !");

            ModuleLoader = new LoriaModuleLoader(logManager);
            ModuleLoader.LoadModules();

            Recognizer = new LoriaRecognizer(logManager);
            Recognizer.ReplacePhrases(ModuleLoader.GetPhrases());
            Recognizer.LoriaSpeechRecognized += Recognizer_SpeechRecognized;

            LoriaThread = new Thread(LoriaLoop);
        }

        private void LoriaLoop()
        {
            try
            {
                while (IsRunning)
                {
                    
                    Thread.Sleep(500);
                }
            }
            catch (ThreadInterruptedException)
            {
                if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Thread has been aborted.");
            }
            catch (Exception e)
            {
                if (LogManager != null) LogManager.WriteLog(LogType.ERROR, string.Format("Error: {0}{1}", Environment.NewLine, e.ToString()));
            }
        }

        public void Start()
        {
            IsRunning = true;
            StartListening();
            LoriaThread.Start();
        }

        public void Stop()
        {
            StopListening();
            IsRunning = false;
        }

        public void StartListening()
        {
            Recognizer.StartListen();
            Vocalizer.Speech("Je vous écoute.");
        }

        public void StopListening()
        {
            Recognizer.StopListen();
            Vocalizer.Speech("Je ne vous écoute plus.");
        }

        public void GoToSleep()
        {
            IsAsleep = true;
            Vocalizer.Speech("D'accord.");
        }

        public void WakeUp()
        {
            IsAsleep = false;
            Vocalizer.Speech("Je suis là.");
        }

        public void ReloadModule()
        {
            ModuleLoader.LoadModules();
            Recognizer.ReplacePhrases(ModuleLoader.GetPhrases());
            Vocalizer.Speech("C'est fait.");
        }

        public void Recognizer_SpeechRecognized(string phrase)
        {
            LoriaActionOnDemand loriaActionOnDemand = ModuleLoader.LoriaModules.SelectMany(m => m.LoriaActions)
                                                                               .Where(a => a is LoriaActionOnDemand)
                                                                               .Cast<LoriaActionOnDemand>()
                                                                               .FirstOrDefault(a => a.Phrases.Contains(phrase));
            if (loriaActionOnDemand != null)
            {
                List<LoriaAnswer> loriaAnswers = loriaActionOnDemand.DoAction(this).ToList();
                foreach (LoriaAnswer loriaAnswer in loriaAnswers)
                {
                    if (loriaAnswer.Speech)
                    {
                        Vocalizer.Speech(loriaAnswer.SpeechText);
                    }
                }
            }
        }

        public void Dispose()
        {
            Recognizer.Dispose();
            Vocalizer.Dispose();
            IsRunning = false;
        }
    }
}
