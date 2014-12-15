using Loria.Core.src.Loria.Module.CoreModules;
using Loria.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core.src.Loria.Module
{
    public enum LoriaActionType
    {
        OnDemand, Repeat, Background
    }

    public class LoriaAction
    {
        private LoriaModule LoriaModule;

        public string Id { get; set; }
        public string Name { get; set; }
        public LoriaActionType Type { get; set; }
        public bool CanDoAsleep { get; set; }
        public List<string> Phrases { get; set; }

        public LoriaAction(LoriaModule loriaModule)
        {
            LoriaModule = loriaModule;
        }

        public IEnumerable<LoriaAnswer> DoAction(LoriaCore loriaCore)
        {
            if (!loriaCore.IsAsleep ||
                loriaCore.IsAsleep && CanDoAsleep)
            {
                return LoriaModule.DoAction(loriaCore, this);
            }
            else
            {
                return new List<LoriaAnswer>();
            }
        }
    }
}
