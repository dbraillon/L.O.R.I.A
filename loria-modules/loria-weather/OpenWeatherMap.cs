using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Loria.Module.Weather
{
    public enum OpenWeatherMapVersion
    {
        VERSION_25
    }

    public enum OpenWeatherMapMode
    {
        XML, JSON
    }

    public enum OpenWeatherMapUnit
    {
        METRIC
    }

    public class OpenWeatherMap
    {
        public string BaseUrl { get; set; }
        public OpenWeatherMapVersion Version { get; set; }
        public OpenWeatherMapMode Mode { get; set; }
        public OpenWeatherMapUnit Unit { get; set; }

        public OpenWeatherMap(OpenWeatherMapVersion version = OpenWeatherMapVersion.VERSION_25,
            OpenWeatherMapMode mode = OpenWeatherMapMode.XML,
            OpenWeatherMapUnit unit = OpenWeatherMapUnit.METRIC)
        {
            BaseUrl = "http://api.openweathermap.org/data/";

            Version = version;
            Mode = mode;
            Unit = unit;
        }

        public WeatherNowResponse GetWeatherNow(string city, string lang, string country = null)
        {
            country = string.IsNullOrEmpty(country) ? "" : string.Concat(",", country);

            string usedUrl = string.Format("{0}{1}/weather?q={2}{3}&mode={4}&lang={5}&units={6}",
                BaseUrl, GetVersion(), city, country, GetMode(), lang, GetUnit());


            using (HttpClient httpClient = new HttpClient())
            {
                string httpResult = httpClient.GetStringAsync(usedUrl).Result;

                if (Mode == OpenWeatherMapMode.XML)
                {
                    XmlDocument xmlResult = new XmlDocument();
                    xmlResult.LoadXml(httpResult);

                    XmlNode temperatureNode = xmlResult.SelectSingleNode("//temperature");
                    XmlAttribute temperatureValueAttribute = temperatureNode != null ? temperatureNode.Attributes["value"] : null;

                    XmlNode weatherNode = xmlResult.SelectSingleNode("//weather");
                    XmlAttribute weatherValueAttribute = weatherNode != null ? weatherNode.Attributes["value"] : null;

                    if (temperatureValueAttribute != null && weatherValueAttribute != null)
                    {
                        WeatherNowResponse weatherResponse = new WeatherNowResponse()
                        {
                            Temperature = double.Parse(temperatureValueAttribute.Value.Replace('.', ',')),
                            Weather = weatherValueAttribute.Value
                        };

                        return weatherResponse;
                    }
                    else
                    {
                        throw new ArgumentException("Can't read weather response.");
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public List<WeatherForecastResponse> GetWeatherForecast(string city, string lang, string country = null)
        {
            List<WeatherForecastResponse> responses = new List<WeatherForecastResponse>();


            country = string.IsNullOrEmpty(country) ? "" : string.Concat(",", country);

            string usedUrl = string.Format("{0}{1}/forecast/daily?q={2}{3}&mode={4}&lang={5}&units={6}&cnt={7}",
                BaseUrl, GetVersion(), city, country, GetMode(), lang, GetUnit(), 7);


            using (HttpClient httpClient = new HttpClient())
            {
                string httpResult = httpClient.GetStringAsync(usedUrl).Result;

                if (Mode == OpenWeatherMapMode.XML)
                {
                    XmlDocument xmlResult = new XmlDocument();
                    xmlResult.LoadXml(httpResult);

                    XmlNodeList timeNodes = xmlResult.SelectNodes("//time");
                    foreach (XmlNode timeNode in timeNodes)
                    {
                        string dayString = timeNode.Attributes["day"].Value;
                        DateTime day = DateTime.ParseExact(dayString, "yyyy-MM-dd", CultureInfo.CurrentCulture);

                        XmlNode symbolNode = timeNode.SelectSingleNode(".//symbol");
                        string weather = symbolNode.Attributes["name"].Value;

                        XmlNode temperatureNode = timeNode.SelectSingleNode(".//temperature");
                        string temperature = temperatureNode.Attributes["day"].Value;

                        WeatherForecastResponse weatherResponse = new WeatherForecastResponse()
                        {
                            Date = day,
                            Temperature = temperature,
                            Weather = weather
                        };
                        responses.Add(weatherResponse);
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            return responses;
        }


        private string GetVersion()
        {
            string version = Version == OpenWeatherMapVersion.VERSION_25 ? "2.5" :
                             "2.5";

            return version;
        }

        private string GetMode()
        {
            string mode = Mode == OpenWeatherMapMode.XML ? "xml" :
                          Mode == OpenWeatherMapMode.JSON ? "json" :
                          "xml";

            return mode;
        }

        private string GetUnit()
        {
            string unit = Unit == OpenWeatherMapUnit.METRIC ? "metric" :
                          "metric";

            return unit;
        }
    }

    public class WeatherNowResponse
    {
        public double Temperature { get; set; }
        public string Weather { get; set; }
    }

    public class WeatherForecastResponse
    {
        public DateTime Date { get; set; }
        public string Weather { get; set; }
        public string Temperature { get; set; }
    }
}
