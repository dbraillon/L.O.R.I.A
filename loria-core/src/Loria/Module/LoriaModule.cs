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
        public FileInfo ConfigFile { get; set; }
        public FileInfo DatabaseFile { get; set; }
        public FileInfo ProgramFile { get; set; }

        public string Name { get; set; }
        public List<string> Phrases { get; set; }

        public LoriaModule(FileInfo configFile, FileInfo databaseFile, FileInfo programFile)
        {
            ConfigFile = configFile;
            DatabaseFile = databaseFile;
            ProgramFile = programFile;
            Phrases = new List<string>();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(ConfigFile.FullName);

            XmlNode moduleNode = xmlDocument.SelectSingleNode("//module");
            XmlAttribute moduleNameNode = moduleNode.Attributes["name"];
            Name = moduleNameNode.Value;

            XmlNodeList phraseNodes = xmlDocument.SelectNodes("//phrase");
            foreach (XmlNode phraseNode in phraseNodes)
            {
                Phrases.Add(phraseNode.InnerText);
            }
        }

        public IEnumerable<string> Ask(string phrase)
        {
            List<string> answers = new List<string>();

            try
            {
                if (phrase == LoriaRecognizer.GIVE_ME_NEWS_PHRASE)
                {
                    answers.AddRange(GetDelayedAnswers());
                }
                else
                {
                    SetQuestion(phrase);

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

                        answers.Add(GetAnswer());
                    }
                }
            }
            catch (Exception)
            {
                answers.Add(string.Format("Je ne trouve pas le {0}.", Name));
            }

            return answers;
        }

        private void SetQuestion(string question)
        {
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

        private string GetAnswer()
        {
            string answer = "Le module n'a pas répondu a temps.";

            // Load database.xml
            XmlDocument databaseXml = new XmlDocument();
            databaseXml.Load(DatabaseFile.FullName);

            XmlNode questionNode = databaseXml.SelectSingleNode("//question[@answered='True' and delayed='False']");
            if (questionNode != null)
            {
                if (!string.IsNullOrEmpty(questionNode.InnerText))
                {
                    answer = questionNode.InnerText;
                }

                questionNode.ParentNode.RemoveChild(questionNode);
                databaseXml.Save(DatabaseFile.FullName);
            }

            return answer;
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
