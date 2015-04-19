using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core.Abilities.Learned
{
    public static class LoriaLearnedAbilities
    {
        private static IList<LoriaAbility> LearnedAbilities;

        public static IList<LoriaAbility> GetLearnedAbilities()
        {
            if (LearnedAbilities == null)
            {
                LearnedAbilities = new List<LoriaAbility>();
            }

            return LearnedAbilities;
        }
    }
}
