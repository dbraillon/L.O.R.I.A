using Loria.Module.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Loria.Module.Weather
{
    public class Program : ILoriaActionHandler
    {
        static void Main(string[] args)
        {
            LoriaModule loriaModule = new LoriaModule();
            loriaModule.Start(new Program());
        }

        public IEnumerable<string> OnDemand(LoriaAction loriaAction, System.IO.FileInfo databaseFile)
        {
            List<string> answers = new List<string>();
            
            #region Load additional attributes

            string city = "Grenoble";
            string country = "fr";
            string lang = "fr";

            try
            {
                city = loriaAction.AdditionalAttributes.First(a => a.Key == "city").Value;
            }
            catch { }

            try
            {
                country = loriaAction.AdditionalAttributes.First(a => a.Key == "country").Value;
            }
            catch { }

            try
            {
                lang = loriaAction.AdditionalAttributes.First(a => a.Key == "lang").Value;
            }
            catch { }

            #endregion

            try
            {
                if (loriaAction.Id == "weather-now")
                {
                    OpenWeatherMap openWeatherMap = new OpenWeatherMap();
                    var result = openWeatherMap.GetWeatherNow(city, lang, country);

                    answers.Add(string.Format("Le temps est {0} avec une température de {1} degrés.", result.Weather, result.Temperature));
                }

                if (loriaAction.Id == "weather-tomorow")
                {
                    OpenWeatherMap openWeatherMap = new OpenWeatherMap();
                    var results = openWeatherMap.GetWeatherForecast(city, lang, country);

                    answers.Add(string.Format("Le temps est {0} avec une température de {1} degrés.", results[1].Weather, results[1].Temperature));
                }

                if (loriaAction.Id == "weather-monday")
                {
                    OpenWeatherMap openWeatherMap = new OpenWeatherMap();
                    var results = openWeatherMap.GetWeatherForecast(city, lang, country);

                    DateTime next = DateTime.Now.Next(DayOfWeek.Monday);
                    int day = (int)(next - DateTime.Now).TotalDays;

                    answers.Add(string.Format("Le temps est {0} avec une température de {1} degrés.", results[day].Weather, results[day].Temperature));
                }

                if (loriaAction.Id == "weather-tuesday")
                {
                    OpenWeatherMap openWeatherMap = new OpenWeatherMap();
                    var results = openWeatherMap.GetWeatherForecast(city, lang, country);

                    DateTime next = DateTime.Now.Next(DayOfWeek.Tuesday);
                    int day = (int)(next - DateTime.Now).TotalDays;

                    answers.Add(string.Format("Le temps est {0} avec une température de {1} degrés.", results[day].Weather, results[day].Temperature));
                }

                if (loriaAction.Id == "weather-wednesday")
                {
                    OpenWeatherMap openWeatherMap = new OpenWeatherMap();
                    var results = openWeatherMap.GetWeatherForecast(city, lang, country);

                    DateTime next = DateTime.Now.Next(DayOfWeek.Wednesday);
                    int day = (int)(next - DateTime.Now).TotalDays;

                    answers.Add(string.Format("Le temps est {0} avec une température de {1} degrés.", results[day].Weather, results[day].Temperature));
                }

                if (loriaAction.Id == "weather-thursday")
                {
                    OpenWeatherMap openWeatherMap = new OpenWeatherMap();
                    var results = openWeatherMap.GetWeatherForecast(city, lang, country);

                    DateTime next = DateTime.Now.Next(DayOfWeek.Thursday);
                    int day = (int)(next - DateTime.Now).TotalDays;

                    answers.Add(string.Format("Le temps est {0} avec une température de {1} degrés.", results[day].Weather, results[day].Temperature));
                }

                if (loriaAction.Id == "weather-friday")
                {
                    OpenWeatherMap openWeatherMap = new OpenWeatherMap();
                    var results = openWeatherMap.GetWeatherForecast(city, lang, country);

                    DateTime next = DateTime.Now.Next(DayOfWeek.Friday);
                    int day = (int)(next - DateTime.Now).TotalDays;

                    answers.Add(string.Format("Le temps est {0} avec une température de {1} degrés.", results[day].Weather, results[day].Temperature));
                }

                if (loriaAction.Id == "weather-saturday")
                {
                    OpenWeatherMap openWeatherMap = new OpenWeatherMap();
                    var results = openWeatherMap.GetWeatherForecast(city, lang, country);

                    DateTime next = DateTime.Now.Next(DayOfWeek.Saturday);
                    int day = (int)(next - DateTime.Now).TotalDays;

                    answers.Add(string.Format("Le temps est {0} avec une température de {1} degrés.", results[day].Weather, results[day].Temperature));
                }

                if (loriaAction.Id == "weather-sunday")
                {
                    OpenWeatherMap openWeatherMap = new OpenWeatherMap();
                    var results = openWeatherMap.GetWeatherForecast(city, lang, country);

                    DateTime next = DateTime.Now.Next(DayOfWeek.Sunday);
                    int day = (int)(next - DateTime.Now).TotalDays;

                    answers.Add(string.Format("Le temps est {0} avec une température de {1} degrés.", results[day].Weather, results[day].Temperature));
                }
            }
            catch (Exception)
            {
                answers.Add("Je n'arrive pas à récupérer la météo.");
            }

            return answers;
        }

        public void InsideLoop(LoriaAction loriaAction, System.IO.FileInfo databaseFile)
        {
            throw new NotImplementedException();
        }
    }

    public static class MyDateTime
    {
        public static DateTime Next(this DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;
            if (target < start)
                target += 7;
            return from.AddDays(target - start);
        }
    }
}
