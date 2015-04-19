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


        private IBrain Brain;
        
        private LoriaSoul() 
        {
            Brain = new Brain();
        }
    }
}
