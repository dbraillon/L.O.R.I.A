using Loria.Core.Debug;
using Loria.Core.Loria.Module.Config.Models;
using Loria.Core.Loria.Module.LoriaActions;
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
        public FileInfo ConfigFile, DatabaseFile, ProgramFile;
        
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
                        XmlAttribute actionTypeAttribute = actionNode.Attributes["type"];
                        LoriaActionType actionType = (LoriaActionType)Enum.Parse(typeof(LoriaActionType), actionTypeAttribute.Value);


                        LoriaAction loriaAction = null;

                        switch (actionType)
                        {
                            case LoriaActionType.Async:
                                break;

                            case LoriaActionType.Background:
                                break;

                            case LoriaActionType.OnDemand:
                                loriaAction = new LoriaActionOnDemand(this, actionNode, LogManager);
                                break;

                            case LoriaActionType.Planned:
                                break;
                        }

                        if (loriaAction != null)
                        {
                            if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Chargement d'une action.");
                            LoriaActions.Add(loriaAction);
                        }
                        else
                        {
                            if (LogManager != null) LogManager.WriteLog(LogType.WARNING, "Echec du chargement d'une action.");
                        }
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
