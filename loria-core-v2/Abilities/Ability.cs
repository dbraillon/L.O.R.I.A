using Loria.Core.Actions;
using Loria.Core.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core.Abilities
{
    public class Ability
    {
        public IList<string> Stimulus { get; set; }
        public Activity Activity { get; set; }
    }
}
