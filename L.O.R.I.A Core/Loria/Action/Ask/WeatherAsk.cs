using Loria.Action.Ask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace L.O.R.I.A_Core.Loria.Action.Ask
{
    public class WeatherAsk : IAsk
    {
        private readonly string[] WeatherStringAsk;

        private Choices AskChoices;

        public WeatherAsk()
        {
            WeatherStringAsk = new string[]
            {
                "Loria quel temps il fait aujourd'hui"
            };

            AskChoices = new Choices();
            AskChoices.Add(WeatherStringAsk);
        }

        public void Dispose()
        {
            // Nothing to dispose
        }

        public Choices GetChoices()
        {
            return AskChoices;
        }

        public string Ask(string choice)
        {
            string result = null;

            if (WeatherStringAsk.Contains(choice))
            {

            }

            return result;
        }
    }
}
