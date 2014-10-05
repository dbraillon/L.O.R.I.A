using Loria.Module.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Module.DateTime
{
    public class Program : ILoriaAction
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Start(args);
        }

        public void Start(string[] args)
        {
            LoriaModule loriaModule = new LoriaModule();
            loriaModule.Ask(args, this);
        }

        public string Ask(string phrase, string name)
        {
            string answer = null;

            if (name == "Date")
                answer = string.Format("Nous sommes le {0}.", System.DateTime.Now.ToLongDateString());

            if (name == "Time")
                answer = string.Format("Il est {0} heures {1}.", System.DateTime.Now.Hour, System.DateTime.Now.Minute);

            return answer;
        }
    }
}
