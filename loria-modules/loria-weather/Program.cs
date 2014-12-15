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

            if (loriaAction.Name == "Weather")
            {
                try
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        string httpResult = httpClient.GetStringAsync("http://api.openweathermap.org/data/2.5/weather?q=Grenoble,fr&mode=xml&lang=fr").Result;

                        XmlDocument xmlResult = new XmlDocument();
                        xmlResult.LoadXml(httpResult);

                        XmlNode temperatureNode = xmlResult.SelectSingleNode("//temperature");
                        XmlAttribute temperatureValueAttribute = temperatureNode != null ? temperatureNode.Attributes["value"] : null;

                        XmlNode weatherNode = xmlResult.SelectSingleNode("//weather");
                        XmlAttribute weatherValueAttribute = weatherNode != null ? weatherNode.Attributes["value"] : null;

                        if (temperatureValueAttribute != null && weatherValueAttribute != null)
                        {
                            WeatherResponse weatherResponse = new WeatherResponse()
                            {
                                Temperature = double.Parse(temperatureValueAttribute.Value.Replace('.', ',')),
                                Weather = weatherValueAttribute.Value
                            };

                            answers.Add(weatherResponse.ToString());
                        }
                        else
                        {
                            throw new ArgumentException("Can't read weather response.");
                        }
                    }
                }
                catch (Exception)
                {
                    answers.Add("Je n'arrive pas à récupérer la météo.");
                }
            }

            return answers;
        }

        public void InsideLoop(LoriaAction loriaAction, System.IO.FileInfo databaseFile)
        {
            throw new NotImplementedException();
        }
    }

    public class WeatherResponse
    {
        public double Temperature { get; set; }
        public string Weather { get; set; }

        public override string ToString()
        {
            return string.Format("Le temps sera {0} avec une températeure de {1} degrés.", Weather, Temperature - 273.15);
        }
    }
}
