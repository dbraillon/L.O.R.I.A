using Loria.Core.Senses;
using Loria.Core.Speeches;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core
{
    public class LoriaSoul
    {
        private static LoriaSoul Soul;

        public static LoriaSoul Live(EventLog eventLog = null)
        {
            if (Soul == null) Soul = new LoriaSoul(eventLog);

            return Soul;
        }


        private IBrain Brain;

        private LoriaSoul(EventLog eventLog = null) 
        {
            Brain = new Brain(eventLog);
        }
    }
}
