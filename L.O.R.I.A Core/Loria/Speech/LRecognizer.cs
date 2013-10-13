using Loria.Action;
using Loria.Text;
using System;
using System.Speech.Recognition;
using System.Threading;

namespace Loria.Speech
{
    internal class LRecognizer
    {
        private Thread RecognizerThread;
        private LActions Actions;
        private LVocalizer Vocalizer;

        internal LRecognizer(LActions actions, LVocalizer vocalizer)
        {
            RecognizerThread = new Thread(Listen);
            Actions = actions;
            Vocalizer = vocalizer;
        }

        internal void Dispose()
        {
            RecognizerThread.Interrupt();
            RecognizerThread.Join();
        }

        internal void Start()
        {
            RecognizerThread.Start();
        }

        private void Listen()
        {
            SpeechRecognizer recognizer = new SpeechRecognizer();

            Grammar grammar = new Grammar(Actions.GetGrammarBuilder());
            recognizer.LoadGrammar(grammar);

            recognizer.SpeechRecognized +=
              new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognized);

            // Sleep while something stop it
            Thread.Sleep(System.Threading.Timeout.Infinite);

            recognizer.Dispose();
        }

        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Vocalizer.Speech(Actions.Ask(e.Result.Text));
        }
    }
}
