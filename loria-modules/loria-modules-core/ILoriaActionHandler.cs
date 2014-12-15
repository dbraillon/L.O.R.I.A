using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Module.Core
{
    public interface ILoriaActionHandler
    {
        IEnumerable<string> OnDemand(LoriaAction loriaAction, FileInfo databaseFile);
        void InsideLoop(LoriaAction loriaAction, FileInfo databaseFile);
    }
}
