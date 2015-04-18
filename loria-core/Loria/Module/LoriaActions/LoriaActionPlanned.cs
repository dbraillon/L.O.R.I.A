using Loria.Core.Debug;
using Loria.Core.src.Loria.Module.CoreModules;
using Loria.Module;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Loria.Core.Loria.Module.LoriaActions
{
    public class LoriaActionPlanned : LoriaAction
    {
        public DateTime FirstStart { get; set; }

        public LoriaActionPlanned(LoriaModule loriaModule, XmlNode actionNode, ILoggable logManager = null)
            : base(loriaModule, actionNode, logManager) 
        {
            XmlNodeList planNodes = actionNode.SelectNodes(".//plan");
            foreach (XmlNode planNode in planNodes)
            {
                XmlAttribute firstStartAttribute = planNode.Attributes["firstStart"];
                string firstStart = firstStartAttribute.Value;
                FirstStart = DateTime.ParseExact(firstStart, "dd/MM/yyyy HH:mm:ss", CultureInfo.CurrentCulture);
            }
        }

        public override IEnumerable<LoriaAnswer> DoAction(LoriaCore loriaCore)
        {
            return null;
        }
    }
}
