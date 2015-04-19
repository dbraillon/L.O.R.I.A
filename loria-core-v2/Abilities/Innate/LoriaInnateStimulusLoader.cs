using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Loria.Core.Abilities.Innate
{
    public static class LoriaInnateStimulusLoader
    {
        public static IList<string> LoadStimulus(string abilityId)
        {
            IList<string> stimulus = new List<string>();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("innate_abilities.xml");

            XmlNode abilityNode = xmlDocument.SelectSingleNode(string.Format("//ability[@id='{0}']", abilityId));
            if (abilityNode != null)
            {
                XmlNodeList stimulusNodes = abilityNode.SelectNodes(".//stimulus");
                foreach (XmlNode stimulusNode in stimulusNodes)
                {
                    stimulus.Add(stimulusNode.InnerText);
                }
            }

            return stimulus;
        }
    }
}
