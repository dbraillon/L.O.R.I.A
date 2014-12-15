using Loria.Module.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

namespace Loria.Module.Series
{
    public class Program : ILoriaActionHandler
    {
        static void Main(string[] args)
        {
            LoriaModule loriaModule = new LoriaModule();
            loriaModule.Start(new Program());
        }

        public Stopwatch Stopwatch { get; set; }

        public Program()
        {
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }

        public IEnumerable<string> OnDemand(LoriaAction loriaAction, FileInfo databaseFile)
        {
            List<string> answers = new List<string>();

            string serieName = loriaAction.Name;
            int minSeason = GetLastSeason(databaseFile, serieName);
            int minEpisode = GetLastEpisode(databaseFile, serieName, minSeason);
            if (minSeason == 1 && minEpisode == 1)
            {
                string seasonString = loriaAction.AdditionalAttributes.FirstOrDefault(aa => aa.Key == "season").Value ?? "1";
                string episodeString = loriaAction.AdditionalAttributes.FirstOrDefault(aa => aa.Key == "episode").Value ?? "1";
                minSeason = int.Parse(seasonString);
                minEpisode = int.Parse(episodeString);
            }
            
            // First find the serie on imdb
            Imdb imdb = new Imdb();
            ImdbSerie imdbSerie = imdb.Search(serieName, minSeason, minEpisode);

            foreach (ImdbSeason season in imdbSerie.Seasons)
            {
                foreach (ImdbEpisode episode in season.Episodes)
                {
                    answers.Add(string.Format("L'épisode {2} de la saison {1} de {0} est disponible, voulez-vous le télécharger ?", imdbSerie.Title, season.Number, episode.Number, episode.Name));
                }
            }

            SaveToDatabase(databaseFile, imdbSerie);

            return answers;
        }

        public void InsideLoop(LoriaAction loriaAction, FileInfo databaseFile) { }

        public int GetLastSeason(FileInfo databaseFile, string serieName)
        {
            int lastSeason = 1;

            // Load database.xml
            XmlDocument databaseXml = new XmlDocument();
            databaseXml.Load(databaseFile.FullName);

            //TODO: Bug that with "Under The Dome" for example
            XmlNode serieNode = databaseXml.SelectSingleNode(string.Format("//serie[@title='{0}']", serieName.ToLower()));
            if (serieNode != null)
            {
                XmlNodeList seasonNodes = serieNode.SelectNodes(".//season");
                if (seasonNodes != null && seasonNodes.Count > 0)
                {
                    XmlNode lastSeasonNode = seasonNodes[seasonNodes.Count - 1];
                    XmlAttribute lastSeasonValueAttribute = lastSeasonNode.Attributes["value"];
                    lastSeason = int.Parse(lastSeasonValueAttribute.Value);
                }
            }

            return lastSeason;
        }

        public int GetLastEpisode(FileInfo databaseFile, string serieName, int season)
        {
            int lastEpisode = 1;

            // Load database.xml
            XmlDocument databaseXml = new XmlDocument();
            databaseXml.Load(databaseFile.FullName);

            XmlNode serieNode = databaseXml.SelectSingleNode(string.Format("//serie[@title='{0}']", serieName.ToLower()));
            if (serieNode != null)
            {
                XmlNode seasonNode = serieNode.SelectSingleNode(string.Format(".//season[@value='{0}']", season));
                if (seasonNode != null)
                {
                    XmlNodeList episodeNodes = seasonNode.SelectNodes(".//episode");
                    if (episodeNodes != null && episodeNodes.Count > 0)
                    {
                        XmlNode lastEpisodeNode = episodeNodes[episodeNodes.Count - 1];
                        XmlAttribute lastEpisodeValueAttribute = lastEpisodeNode.Attributes["value"];
                        lastEpisode = int.Parse(lastEpisodeValueAttribute.Value);
                    }
                }
            }

            return lastEpisode;
        }

        public void SaveToDatabase(FileInfo databaseFile, ImdbSerie imdbSerie)
        {
            // Load database.xml
            XmlDocument databaseXml = new XmlDocument();
            databaseXml.Load(databaseFile.FullName);

            XmlNode databaseNode = databaseXml.SelectSingleNode("//database");
            if (databaseNode == null)
            {
                databaseNode = databaseXml.CreateElement("database");
                databaseXml.AppendChild(databaseNode);
            }

            XmlNode seriesNode = databaseXml.SelectSingleNode("//series");
            if (seriesNode == null)
            {
                seriesNode = databaseXml.CreateElement("series");
                databaseNode.AppendChild(seriesNode);
            }

            XmlNode serieNode = databaseXml.SelectSingleNode(string.Format("//serie[@title='{0}']", imdbSerie.Title.ToLower()));
            if (serieNode == null)
            {
                XmlAttribute serieNameAttribute = databaseXml.CreateAttribute("title");
                serieNameAttribute.Value = imdbSerie.Title.ToLower();

                serieNode = databaseXml.CreateElement("serie");
                serieNode.Attributes.Append(serieNameAttribute);
                seriesNode.AppendChild(serieNode);
            }

            foreach (ImdbSeason imdbSeason in imdbSerie.Seasons)
            {
                XmlNode seasonNode = serieNode.SelectSingleNode(string.Format(".//season[@value='{0}']", imdbSeason.Number));
                if (seasonNode == null)
                {
                    XmlAttribute seasonValueAttribute = databaseXml.CreateAttribute("value");
                    seasonValueAttribute.Value = imdbSeason.Number.ToString();

                    seasonNode = databaseXml.CreateElement("season");
                    seasonNode.Attributes.Append(seasonValueAttribute);
                    serieNode.AppendChild(seasonNode);
                }

                foreach (ImdbEpisode imdbEpisode in imdbSeason.Episodes)
                {
                    XmlNode epidoseNode = seasonNode.SelectSingleNode(string.Format(".//episode[@value='{0}']", imdbEpisode.Number));
                    if (epidoseNode == null)
                    {
                        XmlAttribute episodeValueAttribute = databaseXml.CreateAttribute("value");
                        episodeValueAttribute.Value = imdbEpisode.Number.ToString();

                        XmlAttribute episodeDownloadedAttribute = databaseXml.CreateAttribute("downloaded");
                        episodeDownloadedAttribute.Value = "False";

                        epidoseNode = databaseXml.CreateElement("episode");
                        epidoseNode.Attributes.Append(episodeValueAttribute);
                        epidoseNode.Attributes.Append(episodeDownloadedAttribute);
                        seasonNode.AppendChild(epidoseNode);
                    }

                    epidoseNode.InnerText = imdbEpisode.Name;
                }
            }

            databaseXml.Save(databaseFile.FullName);
        }
    }
}
