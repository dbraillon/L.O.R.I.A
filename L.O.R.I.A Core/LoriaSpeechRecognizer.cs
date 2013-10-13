using L.O.R.I.A_Core.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace L.O.R.I.A_Core
{
    public class LoriaSpeechRecognizer
    {
        private Thread SpeechRecognizerThread;

        private LoriaCore LoriaCore;

        internal LoriaSpeechRecognizer(LoriaCore loriaCore)
        {
            SpeechRecognizerThread = new Thread(Listen);

            LoriaCore = loriaCore;
        }

        internal void Dispose()
        {
            SpeechRecognizerThread.Interrupt();
            SpeechRecognizerThread.Join();
        }

        internal void StartListening()
        {
            SpeechRecognizerThread.Start();
        }

        private void Listen(object obj)
        {
            SpeechRecognizer recognizer = new SpeechRecognizer();

            Choices colors = new Choices();
            colors.Add(LoriaCore.ActionDispatcher.GetChoices());

            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(colors);

            Grammar g = new Grammar(gb);
            recognizer.LoadGrammar(g);

            recognizer.SpeechRecognized +=
              new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognized);

            Thread.Sleep(System.Threading.Timeout.Infinite);

            recognizer.Dispose();
        }

        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            LoriaCore.TextToSpeech.Speech(LoriaCore.ActionDispatcher.Ask(e.Result.Text));
        }
    }
}
