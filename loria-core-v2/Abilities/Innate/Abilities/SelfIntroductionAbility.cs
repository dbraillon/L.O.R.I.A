using Loria.Core.Actions;
using Loria.Core.Activities;
using Loria.Core.Speeches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core.Abilities.Innate
{
    public class SelfIntroductionAbility : LoriaAbility
    {
        private const string AbilityId = "self_introduction";

        public SelfIntroductionAbility()
        {
            Stimulus = LoriaInnateStimulusLoader.LoadStimulus(AbilityId);
            
            Activity = new Activity();
            Activity.OnMainLaunch += Activity_OnMainLaunch;

            Description = "me présenter";
        }

        public void Activity_OnMainLaunch()
        {
            MouthSpeech.GetInstance().Speech("Je m'appelle Loria, je suis une semi-intelligence artificielle. Enchantée.");
        }
    }
}
