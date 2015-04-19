using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core.Abilities.Innate
{
    public static class LoriaInnateAbilities
    {
        private static IList<LoriaAbility> Abilities;

        public static IList<LoriaAbility> GetInnateAbilities()
        {
            if (Abilities == null)
            {
                Abilities = new List<LoriaAbility>();
                Abilities.Add(new CoreVersionAbility());
                Abilities.Add(new ListAbilitiesAbility());
                Abilities.Add(new SelfIntroductionAbility());
            }

            return Abilities;
        }
    }
}
