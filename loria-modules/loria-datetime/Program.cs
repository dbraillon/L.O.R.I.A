using Loria.Module.Core;
using System.Collections.Generic;

namespace Loria.Module.DateTime
{
    public class Program : ILoriaActionHandler
    {
        static void Main(string[] args)
        {
            LoriaModule loriaModule = new LoriaModule();
            loriaModule.Start(new Program());
        }

        public string OnDemand(LoriaAction loriaAction)
        {
            string answer = null;

            if (loriaAction.Name == "Date")
                answer = string.Format("Nous sommes le {0}.", System.DateTime.Now.ToLongDateString());

            if (loriaAction.Name == "Time")
                answer = string.Format("Il est {0} heures {1}.", System.DateTime.Now.Hour, System.DateTime.Now.Minute);

            return answer;
        }

        public void InsideLoop(List<LoriaAction> loriaActions)
        {
            // Nothing
        }
    }
}
