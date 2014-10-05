using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace Loria.Module
{
    public class LoriaModule
    {
        public FileInfo XmlFile { get; set; }
        public FileInfo ProgramFile { get; set; }

        public string Name { get; set; }
        public List<string> Phrases { get; set; }

        public LoriaModule(FileInfo xmlFile, FileInfo programFile)
        {
            XmlFile = xmlFile;
            ProgramFile = programFile;
            Phrases = new List<string>();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(XmlFile.FullName);

            XmlNode moduleNode = xmlDocument.SelectSingleNode("//module");
            XmlAttribute moduleNameNode = moduleNode.Attributes["name"];
            Name = moduleNameNode.Value;

            XmlNodeList phraseNodes = xmlDocument.SelectNodes("//phrase");
            foreach (XmlNode phraseNode in phraseNodes)
            {
                Phrases.Add(phraseNode.InnerText);
            }
        }

        public string Ask(string phrase)
        {
            try
            {
                using (var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = ProgramFile.FullName,
                        Arguments = string.Format("{0}{1}{0}", "\"", phrase),
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                })
                {
                    process.Start();
                    return process.StandardOutput.ReadLine();
                }
            }
            catch (Exception)
            {
                return string.Format("Je ne trouve pas le {0}.", Name);
            }
        }
    }
}
