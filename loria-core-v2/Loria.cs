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
    public class LoriaSoul
    {
        private static LoriaSoul Soul;

        public static LoriaSoul Live()
        {
            if (Soul == null) Soul = new LoriaSoul();

            return Soul;
        }


        private IList<ISense> Senses;
        private IList<Ability> Abilities;
        private IList<ISpeech> Speeches; 
        private IBrain Brain;
        
        private LoriaSoul() 
        {
            Senses = new List<ISense>();
            Senses.Add(new HearingSense());
            //Senses.Add(new SmellSense());

            SetInnateAbility();

            Brain = new Brain(Senses, Abilities, Speeches);
        }

        private void SetInnateAbility()
        {
            Abilities = new List<Ability>();
            Abilities.Add(new SelfIntroductionAbility());
        }
    }
}
