using Loria.Core.Activities;
using Loria.Core.Speeches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core.Abilities.Innate
{
    public class CoreVersionAbility : LoriaAbility
    {
        private const string AbilityId = "core_version";

        public CoreVersionAbility()
        {
            Stimulus = LoriaInnateStimulusLoader.LoadStimulus(AbilityId);
            
            Activity = new Activity();
            Activity.OnMainLaunch += Activity_OnMainLaunch;

            Description = "donner ma version";
        }

        public void Activity_OnMainLaunch()
        {
            MouthSpeech.GetInstance().Speech(string.Format("Mon noyau est en version {0}.", Assembly.GetExecutingAssembly().GetName().Version.ToString()));
        }
    }
}
