using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;

namespace Loria.Module.Core
{
    public class LoriaModule
    {
        public string Name { get; set; }
        public List<LoriaAction> LoriaActions { get; set; }

        public LoriaModule()
        {
            // Initialisation
            Name = "Module inconnu";
            LoriaActions = new List<LoriaAction>();

            try
            {
                // Get config.xml path
                string xmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.xml");

                // Load config.xml
                XmlDocument configXml = new XmlDocument();
                configXml.Load(xmlPath);

                // Retrieve module node and its name and type attributes
                XmlNode moduleNode = configXml.SelectSingleNode("//module");
                if (moduleNode == null)
                {
                    throw new XmlException("Impossible de lire le XML de configuration. La node module n'a pas été trouvé.");
                }

                XmlAttribute moduleNameAttribute = moduleNode.Attributes["name"];
                if (moduleNameAttribute == null || string.IsNullOrEmpty(moduleNameAttribute.Value))
                {
                    throw new XmlException("Impossible de lire le XML de configuration. Le module n'est pas nommé.");
                }

                // Retrieve module name from attribute
                string moduleName = moduleNameAttribute.Value;
                Name = moduleName;

                // Retrieve actions from config.xml
                LoriaActions.AddRange(LoriaAction.GetActions(configXml));
            }
            catch(XmlException xe)
            {
                HandleError(string.Format("Impossible de charger le {0}.", Name), xe.ToString());
            }
            catch (Exception)
            {
                HandleError(string.Format("Impossible de charger le {0}.", Name));
            }
        }

        public void Start(ILoriaActionHandler actionHandler)
        {
            bool isModuleOnDemand = LoriaActions.TrueForAll(a => a.Type == LoriaActionType.ONDEMAND);

            if (isModuleOnDemand)
            {
                List<string> questions = GetQuestions().ToList();

                if (questions.Count > 0)
                {
                    string question = questions.First();
                    string answer = "Aucune action pour cette phrase.";

                    LoriaAction loriaAction = LoriaActions.FirstOrDefault(a => a.Phrases.Contains(question));
                    if (loriaAction != null)
                    {
                        string actionAnswer = actionHandler.OnDemand(loriaAction).FirstOrDefault();
                        if (!string.IsNullOrEmpty(actionAnswer))
                        {
                            answer = actionAnswer;
                        }
                    }

                    SetAnswer(question, answer);
                }
            }
            else
            {
                foreach (LoriaAction repeatAction in LoriaActions.Where(a => a.Type == LoriaActionType.REPEAT))
                {
                    var timer = new System.Timers.Timer(repeatAction.RepeatDelay * 1000);
                    timer.AutoReset = true;
                    timer.Elapsed += (sender, e) => 
                        { 
                            SetDelayedAnswers(actionHandler.OnDemand(repeatAction).ToList());
                        };
                    timer.Start();

                    // Tick now
                    SetDelayedAnswers(actionHandler.OnDemand(repeatAction).ToList());
                }

                while (true)
                {
                    List<string> questions = GetQuestions().ToList();

                    foreach (LoriaAction loriaAction in LoriaActions)
                    {
                        if (loriaAction.Type == LoriaActionType.ONDEMAND)
                        {
                            var questionsForThisAction = loriaAction.Phrases.Intersect(questions);
                            foreach (string questionForThisAction in questionsForThisAction)
                            {
                                string answer = "Aucune action pour cette phrase.";
                                string actionAnswer = actionHandler.OnDemand(loriaAction).FirstOrDefault();

                                if (!string.IsNullOrEmpty(actionAnswer))
                                {
                                    answer = actionAnswer;
                                }

                                SetAnswer(questionForThisAction, answer);
                            }
                        }
                        else if (loriaAction.Type == LoriaActionType.BACKGROUND)
                        {
                            actionHandler.InsideLoop(loriaAction);
                        }
                    }

                    Thread.Sleep(1000);
                }
            }
        }

        public IEnumerable<string> GetQuestions()
        {
            List<string> questions = new List<string>();

            // Get database.xml path
            string xmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "database.xml");

            // Load database.xml
            XmlDocument databaseXml = new XmlDocument();
            databaseXml.Load(xmlPath);

            XmlNodeList questionNodes = databaseXml.SelectNodes("//question[@answered='False']");
            if (questionNodes != null && questionNodes.Count > 0)
            {
                foreach (XmlNode questionNode in questionNodes)
                {
                    if (!string.IsNullOrEmpty(questionNode.InnerText))
                    {
                        questions.Add(questionNode.InnerText);
                    }
                }
            }

            return questions;
        }

        public void SetAnswer(string question, string answer)
        {
            // Get database.xml path
            string xmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "database.xml");

            // Load database.xml
            XmlDocument databaseXml = new XmlDocument();
            databaseXml.Load(xmlPath);

            XmlNode questionNode = databaseXml.SelectSingleNode(string.Format("//question[text()='{0}']", question));
            if (questionNode != null)
            {
                XmlAttribute answeredAttribute = questionNode.Attributes["answered"];
                if (answeredAttribute != null)
                {
                    answeredAttribute.Value = "True";
                }

                questionNode.InnerText = answer;
            }

            databaseXml.Save(xmlPath);
        }

        public void SetDelayedAnswers(List<string> answers)
        {
            // Get database.xml path
            string xmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "database.xml");

            // Load database.xml
            XmlDocument databaseXml = new XmlDocument();
            databaseXml.Load(xmlPath);

            XmlNode databaseNode = databaseXml.SelectSingleNode("//database");
            if (databaseNode == null)
            {
                databaseNode = databaseXml.CreateElement("database");
                databaseXml.AppendChild(databaseNode);
            }

            XmlNode questionsNode = databaseXml.SelectSingleNode("//questions");
            if (questionsNode == null)
            {
                questionsNode = databaseXml.CreateElement("questions");
                databaseNode.AppendChild(questionsNode);
            }

            foreach (string answer in answers)
            {
                XmlAttribute answeredAttribute = databaseXml.CreateAttribute("answered");
                answeredAttribute.Value = "True";

                XmlAttribute delayedAttribute = databaseXml.CreateAttribute("delayed");
                delayedAttribute.Value = "True";

                XmlElement questionElement = databaseXml.CreateElement("question");
                questionElement.InnerText = answer;
                questionElement.Attributes.Append(answeredAttribute);
                questionElement.Attributes.Append(delayedAttribute);

                questionsNode.AppendChild(questionElement);
            }
            
            databaseXml.Save(xmlPath);
        }

        public void HandleError(string baseMessage = "Impossible de charger le module.", string message = null)
        {
            Console.WriteLine(message ?? baseMessage);
            Process.GetCurrentProcess().Kill();
        }
    }
}
