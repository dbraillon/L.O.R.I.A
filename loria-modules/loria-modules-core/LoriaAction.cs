using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Loria.Module.Core
{
    public class LoriaAction
    {
        public string Name { get; set; }
        public string Program { get; set; }
        public string Arguments { get; set; }
        public List<string> Phrases { get; set; }

        public LoriaAction(XmlNode actionNode)
        {
            XmlAttribute actionNameAttribute = actionNode.Attributes["name"];
            Name = actionNameAttribute.Value;

            XmlAttribute actionProgramAttribute = actionNode.Attributes["program"];
            if (actionProgramAttribute != null)
            {
                Program = actionProgramAttribute.Value;
                if (!Program.StartsWith("\"")) Program = string.Format("\"{0}\"", Program);
            }
            
            
            XmlAttribute actionArgumentsAttribute = actionNode.Attributes["arguments"];
            if (actionArgumentsAttribute != null)
            {
                Arguments = actionArgumentsAttribute.Value;
            }

            Phrases = new List<string>();
            XmlNodeList phraseNodes = actionNode.SelectNodes(".//phrase");
            foreach (XmlNode phraseNode in phraseNodes)
            {
                Phrases.Add(phraseNode.InnerText);
            }
        }
    }
}
