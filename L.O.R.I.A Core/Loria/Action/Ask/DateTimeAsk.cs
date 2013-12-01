using System;
using System.Linq;
using System.Speech.Recognition;

namespace Loria.Action.Ask
{
    public class AskDateTime : IAsk
    {
        private readonly string[] DateStringAsk;
        private readonly string[] TimeStringAsk;

        private Choices AskChoices;
        
        public AskDateTime()
        {
            DateStringAsk = new string[]
            {
                "Loria on est quand",
                "On est quand Loria",
                "Quel jour on est Loria",
                "Loria quel jour on est",
                "Quel jour c'est Loria",
                "Loria c'est quel jour",
                "Loria on est quel jour",
                "Loria on est quelle date",
                "Quelle date on est Loria"
            };

            TimeStringAsk = new string[] 
            { 
                "Loria quelle heure est-il",
                "Loria quelle heure il est",
                "Loria quelle heure c'est",
                "Loria c'est quelle heure",
                "Loria il est quelle heure",
                "Quelle heure est-il Loria",
                "Quelle heure il est Loria",
                "Quelle heure c'est Loria",
                "C'est quelle heure Loria"
            };

            AskChoices = new Choices();
            AskChoices.Add(DateStringAsk);
            AskChoices.Add(TimeStringAsk);
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

            if (DateStringAsk.Contains(choice))
                result = string.Format("Nous sommes le {0}", DateTime.Now.ToLongDateString());

            if (TimeStringAsk.Contains(choice))
                result = string.Format("Il est {0} heures {1}.", DateTime.Now.Hour, DateTime.Now.Minute);

            if (result == null)
                result = "Je ne sais pas.";

            return result;
        }
    }
}
