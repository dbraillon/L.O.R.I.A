using Loria.Module.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

        public IEnumerable<string> OnDemand(LoriaAction loriaAction)
        {
            List<string> answers = new List<string>();

            string serieName = loriaAction.Name;
            string seasonString = loriaAction.AdditionalAttributes.FirstOrDefault(aa => aa.Key == "season").Value ?? "1";
            string episodeString = loriaAction.AdditionalAttributes.FirstOrDefault(aa => aa.Key == "episode").Value ?? "1";
            int minSeason = int.Parse(seasonString);
            int minEpisode = int.Parse(episodeString);

            // First find the serie on imdb
            Imdb imdb = new Imdb();
            ImdbSerie imdbSerie = imdb.Search(serieName, minSeason, minEpisode);

            foreach (ImdbSeason season in imdbSerie.Seasons)
            {
                foreach (ImdbEpisode episode in season.Episodes)
                {
                    answers.Add(string.Format("{0} Season {1} Episode {2} {3}", imdbSerie.Title, season.Number, episode.Number, episode.Name));
                }
            }

            return answers;
        }

        public void InsideLoop(LoriaAction loriaAction) { }
    }
}
