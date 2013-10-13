using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace L.O.R.I.A_Core.Actions
{
    public class AskDateTime : AskAction
    {
        private readonly string[] TimeChoices;

        private Choices AskChoices;
        
        public AskDateTime()
        {
            TimeChoices = new string[] 
            { 
                "Loria quelle heure est-il",
                "Loria quelle heure il est",
                "Loria quelle heure c'est",
                "Loria c'est quelle heure",
                "Quelle heure est-il Loria",
                "Quelle heure il est Loria",
                "C'est quelle heure Loria"
            };

            AskChoices = new Choices();
            AskChoices.Add(TimeChoices);
        }

        public void Dispose()
        {

        }

        public string[] GetChoices()
        {
            return TimeChoices;
        }

        public string Ask(string choice)
        {
            string result = null;

            if (TimeChoices.Contains(choice))
                result = string.Format("It's {0} hours {1}.", DateTime.Now.Hour, DateTime.Now.Minute);

            if (result == null)
                result = "I don't know.";

            return result;
        }
    }
}
