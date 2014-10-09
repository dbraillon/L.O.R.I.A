using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Module.Series
{
    public class Imdb
    {
        public Imdb()
        {

        }

        public ImdbSerie Search(string serieName, int lastSeason, int lastEpisode)
        {
            ImdbSerie foundSerie = null;

            string firstLetter = serieName.Substring(0, 1).ToLower();
            string firstFiveLetters = serieName.Replace(" ", "").Substring(0, 5).ToLower();
            string url = string.Format("http://sg.media-imdb.com/suggests/{0}/{1}.json", firstLetter, firstFiveLetters);

            while (firstFiveLetters.Length > 1 && foundSerie == null)
            {
                try
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        string stringResult = httpClient.GetStringAsync(url).Result;
                        stringResult = stringResult.Substring(5 + firstFiveLetters.Count());
                        stringResult = stringResult.Substring(1, stringResult.Length - 2);
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ImdbRequest));
                        MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(stringResult));


                        ImdbRequest imdbRequest = (ImdbRequest)serializer.ReadObject(stream);
                        foundSerie = imdbRequest.Series.FirstOrDefault(s => s.Type == "TV series" && s.Title.Replace(" ", "").ToLower() == serieName.Replace(" ", "").ToLower());
                        foundSerie = foundSerie ?? imdbRequest.Series.FirstOrDefault(s => s.Type == "TV series");

                        if (foundSerie != null)
                        {
                            foundSerie.Seasons = new List<ImdbSeason>();

                            int seasonCount = GetSeasonsCount(foundSerie);
                            for (int i = lastSeason; i <= seasonCount; i++)
                            {
                                foundSerie.Seasons.Add(GetSeason(foundSerie, i, lastSeason == i ? lastEpisode + 1 : 1));
                            }
                        }
                    }
                }
                catch (Exception) 
                {
                    firstFiveLetters = firstFiveLetters.Substring(0, firstFiveLetters.Length - 1);
                }
            }

            return foundSerie;
        }

        private int GetSeasonsCount(ImdbSerie imdbSerie)
        {
            string url = string.Format("http://www.imdb.com/title/{0}/", imdbSerie.Id);

            using (HttpClient httpClient = new HttpClient())
            {
                string stringResult = httpClient.GetStringAsync(url).Result;

                int season = 0;
                int lastindex = stringResult.IndexOf("season=");
                string seasonPart = stringResult.Substring(lastindex, stringResult.IndexOf("&", lastindex) - lastindex);
                seasonPart = seasonPart.Remove(0, 7);
                season = int.Parse(seasonPart);

                if (season == -1)
                {
                    lastindex = stringResult.IndexOf("season=", lastindex + 1);
                    seasonPart = stringResult.Substring(lastindex, stringResult.IndexOf("&", lastindex) - lastindex);
                    seasonPart = seasonPart.Remove(0, 7);
                    season = int.Parse(seasonPart);
                }

                return season;
            }
        }

        private ImdbSeason GetSeason(ImdbSerie imdbSerie, int season, int episode)
        {
            ImdbSeason imdbSeason = new ImdbSeason();
            imdbSeason.Number = season;
            imdbSeason.Episodes = new List<ImdbEpisode>();

            string url = string.Format("http://www.imdb.com/title/{0}/episodes?season={1}", imdbSerie.Id, season);

            using (HttpClient httpClient = new HttpClient())
            {
                string stringResult = httpClient.GetStringAsync(url).Result;
                List<KeyValuePair<int, string>> ep = new List<KeyValuePair<int, string>>();

                int index = stringResult.IndexOf("ep" + episode);
                while (index != -1)
                {
                    int indexTitle = stringResult.IndexOf("title=", index);
                    indexTitle += 7;
                    string subtitle = stringResult.Substring(indexTitle);
                    indexTitle = subtitle.IndexOf("\"");
                    subtitle = subtitle.Substring(0, indexTitle);

                    imdbSeason.Episodes.Add(
                        new ImdbEpisode()
                        {
                            Name = subtitle,
                            Number = episode
                        });

                    episode++;
                    index = stringResult.IndexOf("ep" + episode);
                }

                return imdbSeason;
            }
        }
    }

    [DataContract]
    public class ImdbRequest
    {
        [DataMember(Name = "q")]
        public string Question { get; set; }

        [DataMember(Name = "d")]
        public IEnumerable<ImdbSerie> Series { get; set; }
    }
}
