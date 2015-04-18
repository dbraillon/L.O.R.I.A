using Loria.Core.Loria.Module.LoriaActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core.Loria.Module.Config.Models
{
    public enum LoriaActionType
    {
        Async, Background, OnDemand, Planned
    }

    public class LoriaActionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public LoriaActionType Type { get; set; }
        public IList<string> Phrases { get; set; }
    }
}
