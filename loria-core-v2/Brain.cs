using Loria.Core.Abilities;
using Loria.Core.Abilities.Innate;
using Loria.Core.Actions;
using Loria.Core.Activities;
using Loria.Core.Senses;
using Loria.Core.Speeches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core
{
    public class Brain : IBrain
    {
        public Brain()
        {
            foreach (LoriaAbility ability in LoriaAbilities.GetAbilities())
            {
                foreach (ISense sense in LoriaSenses.GetSenses())
                {
                    sense.AddStimulus(ability.Stimulus.ToArray());
                }
            }

            foreach (ISense sense in LoriaSenses.GetSenses())
            {
                sense.StimulusRecognized += Sense_StimulusRecognized;
                sense.StartSensing();
            }

            MouthSpeech.GetInstance().Speech("Bonjour.");
        }

        private void Sense_StimulusRecognized(string stimulus)
        {
            List<LoriaAbility> stimulatedAbilities = LoriaInnateAbilities.GetInnateAbilities().Where(a => a.Stimulus.Contains(stimulus)).ToList();
            foreach (LoriaAbility stimulatedAbility in stimulatedAbilities)
            {
                stimulatedAbility.Activity.MainLaunch();
            } 
        }
    }
}
