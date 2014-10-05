using Loria.Speech;
using Loria.Text;
using System;

namespace Loria
{
    public class LoriaCore : IDisposable
    {
        public LoriaRecognizer Recognizer;
        public LoriaVocalizer Vocalizer;

        public LoriaCore()
        {
            Vocalizer = new LoriaVocalizer();
            Recognizer = new LoriaRecognizer(Vocalizer);
        }

        public void Dispose()
        {
            Recognizer.Dispose();
            Vocalizer.Dispose();
        }

        public void StartListening()
        {
            Recognizer.StartListening();
        }

        public void GoToSleep()
        {
            Recognizer.GoToSleep();
        }

        public void StopListening()
        {
            Recognizer.StopListening();
        }
    }
}
