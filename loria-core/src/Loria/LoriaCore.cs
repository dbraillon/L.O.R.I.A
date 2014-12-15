using Loria.Core.Debug;
using Loria.Core.src.Loria.Module;
using Loria.Core.src.Loria.Module.CoreModules;
using Loria.Module;
using Loria.Speech;
using Loria.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Loria
{
    public class LoriaCore : IDisposable
    {
        private ILoggable LogManager;

        public LoriaModuleLoader ModuleLoader;
        public LoriaRecognizer Recognizer;
        public LoriaVocalizer Vocalizer;

        public bool IsAsleep;

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
        }

        public void StartListening()
        {
            Recognizer.StartListening();
            Vocalizer.Speech("Je vous écoute.");
        }

        public void StopListening()
        {
            Recognizer.StopListening();
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
            LoriaAction loriaAction = ModuleLoader.LoriaModules.SelectMany(m => m.LoriaActions)
                                                               .FirstOrDefault(a => a.Phrases.Contains(phrase));
            if (loriaAction != null)
            {
                List<LoriaAnswer> loriaAnswers = loriaAction.DoAction(this).ToList();
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
        }
    }
}
