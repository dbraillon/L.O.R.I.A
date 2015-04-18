using Loria.Core.Debug;
using Loria.Core.src.Loria.Module.CoreModules;
using Loria.Module;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Loria.Core.Loria.Module.LoriaActions
{
    public class LoriaActionOnDemand : LoriaAction
    {
        public int ResponseWaitTime { get; set; }
        public List<string> Phrases { get; set; }

        public LoriaActionOnDemand(LoriaModule loriaModule, XmlNode actionNode, ILoggable logManager = null)
            : base(loriaModule, actionNode, logManager) 
        {
            ResponseWaitTime = 5000;
            Phrases = new List<string>();

            XmlNodeList phraseNodes = actionNode.SelectNodes(".//phrase");
            foreach (XmlNode phraseNode in phraseNodes)
            {
                string phrase = phraseNode.InnerText;

                Phrases.Add(phrase);
            }
        }

        public override IEnumerable<LoriaAnswer> DoAction(LoriaCore loriaCore)
        {
            if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Start the action '{0}'.", Name);

            List<LoriaAnswer> loriaAnswers = new List<LoriaAnswer>();

            try
            {
                SetQuestion(Id);

                using (var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = LoriaModule.ProgramFile.FullName,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                })
                {
                    process.Start();
                    process.WaitForExit(ResponseWaitTime);

                    if (!process.HasExited)
                    {
                        process.Kill();
                    }

                    loriaAnswers.Add(GetAnswer());
                }
            }
            catch (Exception e)
            {
                if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Something went wrong while running the action '{0}', see error below.{1}{2}", Name, Environment.NewLine, e.ToString());

                loriaAnswers.Add(new LoriaAnswer(false, true, string.Format("Je ne trouve pas le module {0}.", LoriaModule.ModuleName)));
            }

            return loriaAnswers;
        }

        private void SetQuestion(string question)
        {
            if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Add a question for the module '{0}'.", LoriaModule.ModuleName);

            // Load database.xml
            XmlDocument databaseXml = new XmlDocument();
            databaseXml.Load(LoriaModule.DatabaseFile.FullName);

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
            databaseXml.Save(LoriaModule.DatabaseFile.FullName);
        }

        private LoriaAnswer GetAnswer()
        {
            if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Get answer for the module '{0}'.", LoriaModule.ModuleName);

            string answer = "Le module n'a pas répondu a temps.";

            // Load database.xml
            XmlDocument databaseXml = new XmlDocument();
            databaseXml.Load(LoriaModule.DatabaseFile.FullName);

            XmlNode questionNode = databaseXml.SelectSingleNode("//question[@answered='True' and @delayed='False']");
            if (questionNode != null)
            {
                if (!string.IsNullOrEmpty(questionNode.InnerText))
                {
                    answer = questionNode.InnerText;
                }

                questionNode.ParentNode.RemoveChild(questionNode);
                databaseXml.Save(LoriaModule.DatabaseFile.FullName);
            }

            return new LoriaAnswer(true, true, answer);
        }
    }
}
