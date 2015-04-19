using Loria.Core.Abilities.Innate;
using Loria.Core.Abilities.Learned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core.Abilities
{
    public class LoriaAbilities
    {
        private static IList<LoriaAbility> Abilities;

        public static IList<LoriaAbility> GetAbilities()
        {
            if (Abilities == null)
            {
                Abilities = new List<LoriaAbility>();
                
                foreach (LoriaAbility innateAbility in LoriaInnateAbilities.GetInnateAbilities())
                {
                    Abilities.Add(innateAbility);
                }

                foreach (LoriaAbility learnedAbility in LoriaLearnedAbilities.GetLearnedAbilities())
                {
                    Abilities.Add(learnedAbility);
                }
            }

            return Abilities;
        }
    }
}
