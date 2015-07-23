using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Channels.VoiceChannel.Out
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
                {
                    synthesizer.SetOutputToDefaultAudioDevice();
                    synthesizer.Rate = 0;
                    synthesizer.SelectVoice("ScanSoft Virginie_Dri40_16kHz");
                    synthesizer.Speak(args[0]);
                }
            }
        }
    }
}
