using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.O.R.I.A_Core.Actions
{
    public class ActionDispatcher : IDisposable
    {
        private IEnumerable<AskAction> AskActions;

        public ActionDispatcher()
        {
            List<AskAction> askActions = new List<AskAction>();
            askActions.Add(new AskDateTime());

            AskActions = askActions;
        }

        public void Dispose()
        {
            foreach (AskAction askAction in AskActions)
                askAction.Dispose();
        }

        public string[] GetChoices()
        {
            List<string> choices = new List<string>();
            
            foreach(AskAction askAction in AskActions)
                choices.AddRange(askAction.GetChoices());

            return choices.ToArray();
        }

        public string Ask(string choice)
        {
            string result = null;

            foreach (AskAction askAction in AskActions)
                if ((result = askAction.Ask(choice)) != null)
                    break;

            return result;
        }
    }
}
