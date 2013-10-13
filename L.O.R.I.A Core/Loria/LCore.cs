using Loria.Action;
using Loria.Speech;
using Loria.Text;
using System;

namespace Loria
{
    public class LCore : IDisposable
    {
        internal LRecognizer Recognizer;
        internal LVocalizer Vocalizer;

        internal LActions Actions;

        public LCore()
        {
            Actions = new LActions();

            Recognizer = new LRecognizer(Actions, Vocalizer);
            Vocalizer = new LVocalizer();
        }

        public void Dispose()
        {
            Actions.Dispose();
            Recognizer.Dispose();
            Vocalizer.Dispose();
        }

        public void Listen()
        {
            Recognizer.Start();
        }

        public void EmulateAsk(string choice)
        {
            string result = Actions.Ask(choice);

            Vocalizer.Speech(result);
        }
    }
}
