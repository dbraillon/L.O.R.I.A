using Loria.Core.Debug;
using Loria.Core.src.Loria.Module;
using Loria.Core.src.Loria.Module.CoreModules;
using Loria.Speech;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace Loria.Module
{
    public class LoriaModule
    {
        private ILoggable LogManager;
        protected FileInfo ConfigFile, DatabaseFile, ProgramFile;
        
        public string ModuleName { get; set; }
        public List<LoriaAction> LoriaActions { get; set; }

        public LoriaModule(ILoggable logManager = null, FileInfo configFile = null, FileInfo databaseFile = null, FileInfo programFile = null)
        {
            LogManager = logManager;

            LoriaActions = new List<LoriaAction>();
            ConfigFile = configFile;
            DatabaseFile = databaseFile;
            ProgramFile = programFile;
        }

        public LoriaAnswer LoadConfigFile()
        {
            if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Chargement d'un module.");

            LoriaActions.Clear();
            ModuleName = string.Empty;

            if (ConfigFile != null)
            {
                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(ConfigFile.FullName);

                    XmlNode moduleNode = xmlDocument.SelectSingleNode("//module");
                    XmlAttribute moduleNameNode = moduleNode.Attributes["name"];
                    ModuleName = moduleNameNode.Value;

                    XmlNodeList actionNodes = xmlDocument.SelectNodes("//action");
                    foreach (XmlNode actionNode in actionNodes)
                    {
                        XmlAttribute actionIdAttribute = actionNode.Attributes["id"];
                        XmlAttribute actionNameAttribute = actionNode.Attributes["name"];
                        XmlAttribute actionTypeAttribute = actionNode.Attributes["type"];
                        XmlAttribute canDoAsleepAttribute = actionNode.Attributes["canDoAsleep"];

                        LoriaAction loriaAction = new LoriaAction(this)
                        {
                            Id = actionIdAttribute.Value,
                            Name = actionNameAttribute.Value,
                            Type = (LoriaActionType)Enum.Parse(typeof(LoriaActionType), actionTypeAttribute.Value),
                            CanDoAsleep = canDoAsleepAttribute == null ? false : canDoAsleepAttribute.Value.ToLower() == Boolean.TrueString.ToLower() ? true : false,
                            Phrases = new List<string>()
                        };

                        XmlNodeList phraseNodes = actionNode.SelectNodes(".//phrase");
                        foreach (XmlNode phraseNode in phraseNodes)
                        {
                            string phrase = phraseNode.InnerText;

                            loriaAction.Phrases.Add(phrase);
                        }

                        LoriaActions.Add(loriaAction);
                    }
                }
                catch (Exception e)
                {
                    if (LogManager != null) LogManager.WriteLog(LogType.ERROR, "Le chargement d'un module a échoué '{0}'.", e.ToString());

                    return new LoriaAnswer(false, true, "Je n'arrive pas à charger un module.");
                }
            }

            return new LoriaAnswer(true);
        }

        public virtual IEnumerable<LoriaAnswer> DoAction(LoriaCore loriaCore, LoriaAction loriaAction)
        {
            if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Start the action '{0}'.", loriaAction.Name);

            List<LoriaAnswer> loriaAnswers = new List<LoriaAnswer>();

            try
            {
                SetQuestion(loriaAction.Id);

                using (var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = ProgramFile.FullName,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                })
                {
                    process.Start();
                    process.WaitForExit(10000);

                    if (!process.HasExited)
                    {
                        process.Kill();
                    }

                    loriaAnswers.Add(GetAnswer());
                }
            }
            catch (Exception e)
            {
                if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Something went wrong while running the action '{0}', see error below.{1}{2}", loriaAction.Name, Environment.NewLine, e.ToString());

                loriaAnswers.Add(new LoriaAnswer(false, true, string.Format("Je ne trouve pas le module {0}.", ModuleName)));
            }

            return loriaAnswers;
        }

        private void SetQuestion(string question)
        {
            if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Add a question for the module '{0}'.", ModuleName);

            // Load database.xml
            XmlDocument databaseXml = new XmlDocument();
            databaseXml.Load(DatabaseFile.FullName);

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

            XmlAttribute answeredAttribute = databaseXml.CreateAttribute("answered");
            answeredAttribute.Value = "False";

            XmlAttribute delayedAttribute = databaseXml.CreateAttribute("delayed");
            delayedAttribute.Value = "False";

            XmlElement questionElement = databaseXml.CreateElement("question");
            questionElement.InnerText = question;
            questionElement.Attributes.Append(answeredAttribute);
            questionElement.Attributes.Append(delayedAttribute);

            questionsNode.AppendChild(questionElement);
            databaseXml.Save(DatabaseFile.FullName);
        }

        private LoriaAnswer GetAnswer()
        {
            if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Get answer for the module '{0}'.", ModuleName);

            string answer = "Le module n'a pas répondu a temps.";

            // Load database.xml
            XmlDocument databaseXml = new XmlDocument();
            databaseXml.Load(DatabaseFile.FullName);

            XmlNode questionNode = databaseXml.SelectSingleNode("//question[@answered='True' and @delayed='False']");
            if (questionNode != null)
            {
                if (!string.IsNullOrEmpty(questionNode.InnerText))
                {
                    answer = questionNode.InnerText;
                }

                questionNode.ParentNode.RemoveChild(questionNode);
                databaseXml.Save(DatabaseFile.FullName);
            }

            return new LoriaAnswer(true, true, answer);
        }

        private IEnumerable<string> GetDelayedAnswers()
        {
            List<string> answers = new List<string>();
            bool isChanged = false;

            // Load database.xml
            XmlDocument databaseXml = new XmlDocument();
            databaseXml.Load(DatabaseFile.FullName);

            XmlNodeList questionNodes = databaseXml.SelectNodes("//question[@answered='True' and delayed='True']");
            foreach (XmlNode questionNode in questionNodes)
            {
                if (!string.IsNullOrEmpty(questionNode.InnerText))
                {
                    answers.Add(questionNode.InnerText);
                }

                questionNode.ParentNode.RemoveChild(questionNode);
                isChanged = true;
            }

            if (isChanged)
            {
                databaseXml.Save(DatabaseFile.FullName);
            }
            
            return answers;
        }
    }
}
