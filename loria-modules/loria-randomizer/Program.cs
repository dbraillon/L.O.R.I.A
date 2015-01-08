using Loria.Module.Core;
using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;

namespace Loria.Module.Randomizer
{
    public class Program : ILoriaActionHandler
    {
        static void Main(string[] args)
        {
            LoriaModule loriaModule = new LoriaModule();
            loriaModule.Start(new Program());
        }

        public IEnumerable<string> OnDemand(LoriaAction loriaAction, FileInfo databaseFile)
        {
            List<string> answers = new List<string>();

            XmlDocument databaseXmlDocument = new XmlDocument();
            databaseXmlDocument.Load(databaseFile.FullName);

            XmlNodeList poolOfDataNodes = databaseXmlDocument.SelectNodes(string.Concat("//", loriaAction.Id));
            Random randomizer = new Random();
            int randomIndex = randomizer.Next(poolOfDataNodes.Count);
            XmlNode randomData = poolOfDataNodes.Item(randomIndex);

            answers.Add(randomData.InnerText);

            return answers;
        }

        public void InsideLoop(LoriaAction loriaAction, System.IO.FileInfo databaseFile)
        {
            throw new NotImplementedException();
        }
    }
}
