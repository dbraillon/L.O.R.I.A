using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace L.O.R.I.A_Core
{
    public class LoriaTextToSpeech : IDisposable
    {
        private SpeechSynthesizer SpeechSynthesizer;

        public LoriaTextToSpeech()
        {
            SpeechSynthesizer = new SpeechSynthesizer();
            SpeechSynthesizer.SetOutputToDefaultAudioDevice();
            SpeechSynthesizer.Rate = 0;
        }

        public void Dispose()
        {
            SpeechSynthesizer.Dispose();
        }

        public void Speech(string text)
        {
            SpeechSynthesizer.Speak(text);
        }
    }
}
