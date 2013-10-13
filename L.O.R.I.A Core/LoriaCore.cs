using L.O.R.I.A_Core.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.O.R.I.A_Core
{
    public class LoriaCore : IDisposable
    {
        public LoriaSpeechRecognizer SpeechRecognizer;
        public LoriaTextToSpeech TextToSpeech;

        public ActionDispatcher ActionDispatcher;

        public LoriaCore()
        {
            ActionDispatcher = new ActionDispatcher();

            SpeechRecognizer = new LoriaSpeechRecognizer(this);
            TextToSpeech = new LoriaTextToSpeech();
        }

        public void Dispose()
        {
            ActionDispatcher.Dispose();
            SpeechRecognizer.Dispose();
            TextToSpeech.Dispose();
        }

        public void StartListening()
        {
            SpeechRecognizer.StartListening();
        }

        public void EmulateAsk(string choice)
        {
            string result = ActionDispatcher.Ask(choice);

            TextToSpeech.Speech(result);
        }
    }
}
