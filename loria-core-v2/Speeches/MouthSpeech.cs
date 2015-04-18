using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core.Speeches
{
    public class MouthSpeech : IDisposable
    {
        private static MouthSpeech Instance;
        public static MouthSpeech GetInstance()
        {
            if (Instance == null) Instance = new MouthSpeech();

            return Instance;
        }

        private SpeechSynthesizer Synthesizer;

        private MouthSpeech()
        {
            Synthesizer = new SpeechSynthesizer();
            Synthesizer.SetOutputToDefaultAudioDevice();
            Synthesizer.Rate = 0;
            Synthesizer.SelectVoice("ScanSoft Virginie_Dri40_16kHz");
        }

        public void Speech(string textToSpeech)
        {
            Synthesizer.Speak(textToSpeech);
        }

        public void Dispose()
        {
            Synthesizer.Dispose();
        }
    }
}
