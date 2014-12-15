using Loria.Module.Core;
using System;
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

        public IEnumerable<string> OnDemand(LoriaAction loriaAction, System.IO.FileInfo databaseFile)
        {
            List<string> answers = new List<string>();

            if (loriaAction.Name == "Date")
                answers.Add(string.Format("Nous sommes le {0}.", System.DateTime.Now.ToLongDateString()));

            if (loriaAction.Name == "Time")
                answers.Add(string.Format("Il est {0} heures {1}.", System.DateTime.Now.Hour, System.DateTime.Now.Minute));

            return answers;
        }

        public void InsideLoop(LoriaAction loriaAction, System.IO.FileInfo databaseFile)
        {
            throw new NotImplementedException();
        }
    }
}
