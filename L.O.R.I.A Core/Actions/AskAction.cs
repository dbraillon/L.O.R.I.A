using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.O.R.I.A_Core.Actions
{
    public interface AskAction : IDisposable
    {
        string[] GetChoices();
        string Ask(string choice);
    }
}
