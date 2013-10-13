using System;
using System.Collections.ObjectModel;
using System.Speech.Synthesis;

namespace Loria.Text
{
    internal class LVocalizer : IDisposable
    {
        private SpeechSynthesizer Synthesizer;

        internal LVocalizer()
        {
            Synthesizer = new SpeechSynthesizer();
            Synthesizer.SetOutputToDefaultAudioDevice();
            Synthesizer.Rate = 0;
            Synthesizer.SelectVoice("ScanSoft Virginie_Dri40_16kHz");
        }

        public void Dispose()
        {
            Synthesizer.Dispose();
        }

        internal void Speech(string text)
        {
            Synthesizer.Speak(text);
        }
    }
}
