using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core.Senses
{
    public static class LoriaSenses
    {
        private static IList<ISense> Senses;

        public static IList<ISense> GetSenses()
        {
            if (Senses == null)
            {
                Senses = new List<ISense>();
                Senses.Add(HearingSense.GetInstance());
                //Senses.Add(SmellSense.GetInstance());
            }

            return Senses;
        }
    }
}
