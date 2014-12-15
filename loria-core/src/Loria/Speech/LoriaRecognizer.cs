using Loria.Core.Debug;
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
    public delegate void SpeechRecognized(string phrase);

    public class LoriaRecognizer
    {
        private ILoggable LogManager;
        private Thread RecognizerThread;
        private SpeechRecognitionEngine RecognitionEngine;
        private List<string> Phrases;

        public event SpeechRecognized LoriaSpeechRecognized;
        public bool IsRunning { get; set; }

        public LoriaRecognizer(ILoggable logManager = null)
        {
            LogManager = logManager;

            RecognizerThread = new Thread(Listen); 
            IsRunning = false;
            Phrases = new List<string>();

            if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Start SpeechRecognitionEngine to default input audio device and set language to fr-FR.");
            RecognitionEngine = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("fr-FR"));
            RecognitionEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognized);
            RecognitionEngine.SetInputToDefaultAudioDevice();
        }

        public void ReplacePhrases(IEnumerable<string> phrases)
        {
            if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Replace all phrases ({0}) and reload Grammar.", phrases.Count());

            Phrases.Clear();
            Phrases.AddRange(phrases);

            Choices choices = new Choices();
            choices.Add(phrases.ToArray());

            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(choices);

            Grammar grammar = new Grammar(grammarBuilder);

            RecognitionEngine.UnloadAllGrammars();
            RecognitionEngine.LoadGrammar(grammar);
        }

        public IEnumerable<string> GetPhrases()
        {
            if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Get all known phrases ({0}).", Phrases.Count);

            return Phrases;
        }

        public void Dispose()
        {
            if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Stop listening and dispose SpeechRecognitionEngine.");
            
            StopListening();
            RecognitionEngine.Dispose();
        }

        public void StartListening()
        {
            if (!IsRunning)
            {
                if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Start listening.");
                
                RecognizerThread.Start();
                IsRunning = true;
            }
        }

        public void StopListening()
        {
            if (IsRunning)
            {
                if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Stop listening.");

                RecognizerThread.Interrupt();
                IsRunning = false;
            }
        }

        // Recognizer thread
        private void Listen()
        {
            while (RecognizerThread.ThreadState == System.Threading.ThreadState.Running)
            {
                try
                {
                    if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Ready to listen.");

                    RecognitionEngine.Recognize();
                }
                catch (Exception e) 
                {
                    if (LogManager != null) LogManager.WriteLog(LogType.ERROR, "Something goes wrong in recognition '{0}'.", e.ToString());
                    return;
                }
            }
        }

        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Choice recognized '{0}' with confidence value of '{1}'.", e.Result.Text, e.Result.Confidence);
            
            if (e.Result.Confidence >= 0.5f)
            {
                if (LoriaSpeechRecognized != null) LoriaSpeechRecognized(e.Result.Text);
            }
        }
    }
}
