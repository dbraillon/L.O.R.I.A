using Loria.Core.Activities;
using Loria.Core.Speeches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core.Abilities.Innate
{
    public class ListAbilitiesAbility : LoriaAbility
    {
        private const string AbilityId = "list_abilities";

        public ListAbilitiesAbility()
        {
            Stimulus = LoriaInnateStimulusLoader.LoadStimulus(AbilityId);
            
            Activity = new Activity();
            Activity.OnMainLaunch += Activity_OnMainLaunch;

            Description = "lister mes abilités";
        }

        public void Activity_OnMainLaunch()
        {
            MouthSpeech.GetInstance().Speech(string.Format("Je peux {0}.", string.Join(", ", LoriaInnateAbilities.GetInnateAbilities().Select(a => a.Description))));
        }
    }
}
