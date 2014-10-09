using Loria.Module;
using Loria.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Speech.Recognition;
using System.Threading;

namespace Loria.Speech
{
    public class LoriaRecognizer
    {
        private const string GIVE_VERSION_PHRASE = "Loria donne moi ta version.";
        private const string RELOAD_MODULE_PHRASE = "Loria recharge tes modules.";
        private const string LIST_MODULE_PHRASE = "Loria liste-moi tes modules.";
        private const string GO_TO_SLEEP_PHRASE = "Loria va dormir.";
        private const string WAKE_UP_PHRASE = "Loria reveille toi.";
        public const string GIVE_ME_NEWS_PHRASE = "Loria tu as des choses à me dire.";

        public bool IsRunning { get; set; }
        public bool IsSleeping { get; set; }
        private Thread RecognizerThread;
        
        private SpeechRecognitionEngine RecognitionEngine;
        private LoriaModuleLoader ModuleLoader;
        private LoriaVocalizer Vocalizer;

        public LoriaRecognizer(LoriaVocalizer vocalizer)
        {
            IsRunning = false;
            IsSleeping = false;
            RecognizerThread = new Thread(Listen);
            Vocalizer = vocalizer;

            RecognitionEngine = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("fr-FR"));
            RecognitionEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognized);
            RecognitionEngine.SetInputToDefaultAudioDevice();
        }

        public void Dispose()
        {
            StopListening();
            RecognitionEngine.Dispose();
        }

        public void StartListening()
        {
            if (!IsRunning)
            {
                RecognizerThread.Start();
                IsRunning = true;
                IsSleeping = false;
                Load();

                Vocalizer.Speech("Bonjour !");
            }
        }

        public void GoToSleep()
        {
            if (IsRunning && !IsSleeping)
            {
                IsSleeping = true;
                Load();

                Vocalizer.Speech("D'accord, je vais dormir.");
            }
        }

        public void WakeUp()
        {
            if (IsRunning && IsSleeping)
            {
                IsSleeping = false;
                Load();

                Vocalizer.Speech("Je suis là.");
            }
        }

        public void StopListening()
        {
            if (IsRunning)
            {
                RecognizerThread.Interrupt();
                IsRunning = false;
                IsSleeping = true;

                Vocalizer.Speech("Aurevoir !");
            }
        }

        private void Load()
        {
            Choices choices = null;

            if (!IsSleeping)
            {
                // Get modules phrases
                ModuleLoader = new LoriaModuleLoader();
                choices = ModuleLoader.GetChoices();

                // Add give version phrase
                choices.Add(GIVE_VERSION_PHRASE);

                // Add reload module phrase
                choices.Add(RELOAD_MODULE_PHRASE);

                // Add list module phrase
                choices.Add(LIST_MODULE_PHRASE);

                // Add go to sleep phrase
                choices.Add(GO_TO_SLEEP_PHRASE);

                // Add give me news phrase
                choices.Add(GIVE_ME_NEWS_PHRASE);
            }
            else
            {
                choices = new Choices();

                // Add wake up phrase
                choices.Add(WAKE_UP_PHRASE);
            }
            
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(choices);

            Grammar grammar = new Grammar(grammarBuilder);

            RecognitionEngine.UnloadAllGrammars();
            RecognitionEngine.LoadGrammar(grammar);
        }

        // Recognizer thread
        private void Listen()
        {
            while (RecognizerThread.ThreadState == System.Threading.ThreadState.Running)
            {
                try
                {
                    RecognitionEngine.Recognize();
                }
                catch (Exception) 
                {
                    return;
                }
            }
        }

        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("{0} ({1})", e.Result.Text, e.Result.Confidence);

            if (e.Result.Confidence >= 0.5f)
            {
                if (e.Result.Text == GIVE_VERSION_PHRASE)
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

                    string versionPhrase = string.Format("Je suis en version {0}.", fvi.FileVersion);

                    Vocalizer.Speech(versionPhrase);
                }
                else if (e.Result.Text == RELOAD_MODULE_PHRASE)
                {
                    Load();

                    Vocalizer.Speech("C'est fait.");
                }
                else if (e.Result.Text == LIST_MODULE_PHRASE)
                {
                    string moduleList = string.Join(", ", ModuleLoader.LoriaModules.Select(m => m.Name));

                    Vocalizer.Speech(string.Format("J'ai {0} modules chargés. {1}.", ModuleLoader.LoriaModules.Count, moduleList));
                }
                else if (e.Result.Text == GO_TO_SLEEP_PHRASE)
                {
                    GoToSleep();
                }
                else if (e.Result.Text == WAKE_UP_PHRASE)
                {
                    WakeUp();
                }
                else if (e.Result.Text == GIVE_ME_NEWS_PHRASE)
                {
                    foreach (LoriaModule loriaModule in ModuleLoader.LoriaModules)
                    {
                        List<string> answers = loriaModule.Ask(GIVE_ME_NEWS_PHRASE).ToList();

                        foreach (string answer in answers)
                        {
                            Vocalizer.Speech(answer);
                            Thread.Sleep(500);
                        }
                    }
                }
                else
                {
                    LoriaModule loriaModule = ModuleLoader.GetModule(e.Result.Text);
                    if (loriaModule != null)
                    {
                        Vocalizer.Speech(loriaModule.Ask(e.Result.Text).First());
                    }
                }
            }
            else
            {
                Vocalizer.Speech("Je n'ai pas compris.");
            }
        }
    }
}
