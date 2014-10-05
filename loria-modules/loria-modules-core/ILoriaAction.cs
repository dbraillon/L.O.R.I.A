using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Module.Core
{
    public interface ILoriaAction
    {
        void Start(string[] args);
        string Ask(string phrase, string name);
    }
}
