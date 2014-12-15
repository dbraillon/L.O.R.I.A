using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Loria.Module.Core
{
    public enum LoriaActionType
    {
        ONDEMAND, REPEAT, BACKGROUND, UNKNOW
    }

    public class LoriaAction
    {
        public const string ANYTHING_NEW_ACTION = "Loria, tu as des choses à me dire ?";

        public string Id { get; set; }
        public string Name { get; set; }
        public LoriaActionType Type { get; set; }
        public int RepeatDelay { get; set; }
        public List<KeyValuePair<string, string>> AdditionalAttributes { get; set; }
        public List<string> Phrases { get; set; }

        public static IEnumerable<LoriaAction> GetActions(XmlDocument configXml)
        {
            List<LoriaAction> loriaActions = new List<LoriaAction>();

            XmlNodeList actionNodes = configXml.SelectNodes("//action");
            if (actionNodes == null || actionNodes.Count <= 0)
            {
                throw new XmlException("Impossible de lire le XML de configuration. Un module doit avoir au moins une action.");
            }

            foreach (XmlNode actionNode in actionNodes)
            {
                LoriaAction loriaAction = new LoriaAction();

                XmlAttribute actionIdAttribute = actionNode.Attributes["id"];
                if (actionIdAttribute == null || string.IsNullOrEmpty(actionIdAttribute.Value))
                {
                    throw new XmlException("Impossible de lire le XML de configuration. Une action n'a pas d'ID.");
                }

                XmlAttribute actionNameAttribute = actionNode.Attributes["name"];
                if (actionNameAttribute == null || string.IsNullOrEmpty(actionNameAttribute.Value))
                {
                    throw new XmlException("Impossible de lire le XML de configuration. Une action n'est pas nommée.");
                }

                XmlAttribute actionTypeAttribute = actionNode.Attributes["type"];
                if (actionTypeAttribute == null || string.IsNullOrEmpty(actionTypeAttribute.Value))
                {
                    throw new XmlException("Impossible de lire le XML de configuration. Une action n'est pas typé.");
                }

                XmlAttribute actionRepeatAttribute = actionNode.Attributes["repeat"];
                if (actionRepeatAttribute != null && !string.IsNullOrEmpty(actionRepeatAttribute.Value))
                {
                    // Retrieve repeat name from attribute
                    string actionRepeat = actionRepeatAttribute.Value;
                    loriaAction.RepeatDelay = int.Parse(actionRepeat);
                }

                // Retrieve action id from attribute
                string actionId = actionIdAttribute.Value;
                loriaAction.Id = actionId;

                // Retrieve action name from attribute
                string actionName = actionNameAttribute.Value;
                loriaAction.Name = actionName;

                // Retrieve action type from attribute
                string actionTypeString = actionTypeAttribute.Value;
                LoriaActionType actionType = LoriaActionType.UNKNOW;
                if (!Enum.TryParse(actionTypeString, true, out actionType))
                {
                    // Can't find corresponding module type
                    throw new XmlException("Impossible de lire le XML de configuration. Le type d'une action n'est pas reconnu.");
                }
                else
                {
                    loriaAction.Type = actionType;
                }
                
                // Retrieve additional attributes
                XmlAttributeCollection actionAttributes = actionNode.Attributes;
                foreach (XmlAttribute actionAttribute in actionAttributes)
                {
                    if (actionAttribute.Name != "name")
                    {
                        loriaAction.AdditionalAttributes.Add(new KeyValuePair<string, string>(actionAttribute.Name, actionAttribute.Value));
                    }
                }

                // Retrieve phrases
                XmlNodeList phraseNodes = actionNode.SelectNodes(".//phrase");
                foreach (XmlNode phraseNode in phraseNodes)
                {
                    string phrase = phraseNode.InnerText;
                    if (string.IsNullOrEmpty(phrase))
                    {
                        throw new XmlException("Impossible de lire le XML de configuration. Une phrase ne peut pas être vide.");
                    }

                    loriaAction.Phrases.Add(phrase);
                }

                // Additional checks
                if (loriaAction.Type == LoriaActionType.ONDEMAND && loriaAction.Phrases.Count <= 0)
                {
                    throw new XmlException("Impossible de lire le XML de configuration. Les modules OnDemand ne doivent pas avoir d'action sans phrase.");
                }

                loriaActions.Add(loriaAction);
            }

            return loriaActions;
        }

        private LoriaAction()
        {
            Name = "Action inconnue";
            Type = LoriaActionType.UNKNOW;
            RepeatDelay = 0;
            AdditionalAttributes = new List<KeyValuePair<string, string>>();
            Phrases = new List<string>();
        }
    }
}
