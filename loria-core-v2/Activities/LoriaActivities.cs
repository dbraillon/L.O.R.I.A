using Loria.Core.Abilities.Innate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core.Activities
{
    public static class LoriaActivities
    {
        private static IList<Activity> Activities;

        public static IList<Activity> GetActivities()
        {
            if (Activities == null)
            {
                Activities = new List<Activity>(LoriaInnateAbilities.GetInnateAbilities().Select(a => a.Activity));
            }

            return Activities;
        }
    }
}
