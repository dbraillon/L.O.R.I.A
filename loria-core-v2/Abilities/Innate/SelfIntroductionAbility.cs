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
    public class SelfIntroductionAbility : Ability
    {
        public SelfIntroductionAbility()
        {
            Stimulus = new List<string>();
            Stimulus.Add("Loria, présente toi.");
            Stimulus.Add("Présente toi Loria.");

            Activity = new Activity();
            Activity.OnMainLaunch += Activity_OnMainLaunch;
        }

        public void Activity_OnMainLaunch()
        {
            MouthSpeech.GetInstance().Speech("Je m'appelle Loria, je suis une semi-intelligence artificielle. Enchantée.");
        }
    }
}
