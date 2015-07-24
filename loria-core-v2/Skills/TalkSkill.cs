using Loria.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core.Skills
{
    public class TalkSkill : SkillAction
    {
        private SpeechSynthesizer Synthesizer;

        public TalkSkill()
        {
            Synthesizer = new SpeechSynthesizer();
            Synthesizer.SetOutputToDefaultAudioDevice();
            Synthesizer.Rate = 0;
            Synthesizer.SelectVoice("ScanSoft Virginie_Dri40_16kHz");

            OnFirstLaunch += TalkSkill_OnFirstLaunch;
        }

        private void TalkSkill_OnFirstLaunch()
        {
            //Synthesizer.Speak(FirstValue);
        }

        public void Dispose()
        {
            Synthesizer.Dispose();
        }
    }
}
