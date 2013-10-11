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
        private LoriaTextToSpeech TextToSpeech;

        private ActionDispatcher ActionDispatcher;

        public LoriaCore()
        {
            TextToSpeech = new LoriaTextToSpeech();
            ActionDispatcher = new ActionDispatcher();
        }

        public void Dispose()
        {
            TextToSpeech.Dispose();
            ActionDispatcher.Dispose();
        }

        public void EmulateAsk(string choice)
        {
            string result = ActionDispatcher.Ask(choice);

            TextToSpeech.Speech(result);
        }
    }
}
