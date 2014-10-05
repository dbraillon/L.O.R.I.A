using System;
using System.Speech.Synthesis;

namespace Loria.Text
{
    public class LoriaVocalizer : IDisposable
    {
        private SpeechSynthesizer Synthesizer;

        public LoriaVocalizer()
        {
            Synthesizer = new SpeechSynthesizer();
            Synthesizer.SetOutputToDefaultAudioDevice();
            Synthesizer.Rate = 0;

            // User have to install this voice in order to use it
            Synthesizer.SelectVoice("ScanSoft Virginie_Dri40_16kHz");
        }

        public void Dispose()
        {
            Synthesizer.Dispose();
        }

        public void Speech(string text)
        {
            Synthesizer.Speak(text);
        }
    }
}
