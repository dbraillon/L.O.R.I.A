using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Loria.Module.Core
{
    public class LoriaModule
    {
        private const string UNKNOW_ERROR_RESPONSE = "Je n'arrive pas à charger le module.";
        private const string MODULE_ERROR_RESPONSE = "Je n'arrive pas à charger le {0}.";
        private const string ACTION_ERROR_RESPONSE = "Je n'arrive pas à utiliser le {0}.";
        private const string PHRASE_ERROR_RESPONSE = "Je n'arrive pas à me servir du {0}.";

        private string Name;
        private List<LoriaAction> LoriaActions;
        private bool IsError;

        public LoriaModule()
        {
            LoriaActions = new List<LoriaAction>();
            IsError = false;

            try
            {
                string xmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Phrases.xml");

                XmlDocument phrasesXml = new XmlDocument();
                phrasesXml.Load(xmlPath);

                XmlNode moduleNode = phrasesXml.SelectSingleNode("//module");
                XmlAttribute moduleNameNode = moduleNode.Attributes["name"];
                Name = moduleNameNode.Value;

                try
                {
                    XmlNodeList actionNodes = phrasesXml.SelectNodes("//action");
                    foreach (XmlNode actionNode in actionNodes)
                    {
                        LoriaActions.Add(new LoriaAction(actionNode));
                    }
                }
                catch (Exception)
                {
                    IsError = true;
                    Console.WriteLine(MODULE_ERROR_RESPONSE, Name);
                }
            }
            catch (Exception)
            {
                IsError = true;
                Console.WriteLine(UNKNOW_ERROR_RESPONSE);
            }
        }

        public void Ask(string[] args, ILoriaAction loriaActionHandler)
        {
            if (!IsError)
            {
                if (args.Length != 1 || !LoriaActions.Any(a => a.Phrases.Contains(args[0])))
                {
                    Console.WriteLine(ACTION_ERROR_RESPONSE, Name);
                    return;
                }

                string phrase = args[0];
                LoriaAction loriaAction = LoriaActions.First(a => a.Phrases.Contains(phrase));

                string answer = loriaActionHandler.Ask(loriaAction);
                if (string.IsNullOrEmpty(answer))
                {
                    Console.WriteLine(PHRASE_ERROR_RESPONSE);
                }
                else
                {
                    Console.WriteLine(answer);
                }
            }
        }
    }
}
