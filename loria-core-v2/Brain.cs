using Loria.Core.Abilities;
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
        private IList<ISense> Senses;
        private IList<Ability> Abilities;
        private IList<ISpeech> Speeches;

        public Brain(IList<ISense> senses, IList<Ability> abilities, IList<ISpeech> speeches)
        {
            Senses = senses;
            Abilities = abilities;
            Speeches = speeches;

            foreach (Ability ability in Abilities)
            {
                foreach (ISense sense in Senses)
                {
                    sense.AddStimulus(ability.Stimulus.ToArray());
                }
            }

            foreach (ISense sense in Senses)
            {
                sense.StimulusRecognized += Sense_StimulusRecognized;
                sense.StartSensing();
            }

            MouthSpeech.GetInstance().Speech("Bonjour.");
        }

        private void Sense_StimulusRecognized(string stimulus)
        {
            List<Ability> stimulatedAbilities = Abilities.Where(a => a.Stimulus.Contains(stimulus)).ToList();
            foreach (Ability stimulatedAbility in stimulatedAbilities)
            {
                stimulatedAbility.Activity.MainLaunch();
            } 
        }
    }
}
