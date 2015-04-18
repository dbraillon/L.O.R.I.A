using Loria.Core.Debug;
using Loria.Core.src.Loria.Module.CoreModules;
using Loria.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Loria.Core.Loria.Module.LoriaActions
{

    public abstract class LoriaAction
    {
        protected ILoggable LogManager;
        protected LoriaModule LoriaModule;

        public string Id { get; set; }
        public string Name { get; set; }
        public bool CanDoAsleep { get; set; }

        public LoriaAction(LoriaModule loriaModule, XmlNode actionNode, ILoggable logManager = null)
        {
            LoriaModule = loriaModule;
            LogManager = logManager;

            XmlAttribute actionIdAttribute = actionNode.Attributes["id"];
            XmlAttribute actionNameAttribute = actionNode.Attributes["name"];
            XmlAttribute canDoAsleepAttribute = actionNode.Attributes["canDoAsleep"];

            Id = actionIdAttribute.Value;
            Name = actionNameAttribute.Value;
            CanDoAsleep = canDoAsleepAttribute == null ? false : canDoAsleepAttribute.Value.ToLower() == Boolean.TrueString.ToLower() ? true : false;
        }

        public abstract IEnumerable<LoriaAnswer> DoAction(LoriaCore loriaCore);
    }
}
