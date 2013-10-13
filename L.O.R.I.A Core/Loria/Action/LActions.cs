using Loria.Action.Ask;
using System;
using System.Collections.Generic;
using System.Speech.Recognition;

namespace Loria.Action
{
    internal class LActions : IDisposable
    {
        private IEnumerable<IAskAction> AskActions;

        internal LActions()
        {
            List<IAskAction> askActions = new List<IAskAction>();
            askActions.Add(new AskDateTime());

            AskActions = askActions;
        }

        public void Dispose()
        {
            foreach (IAskAction askAction in AskActions)
                askAction.Dispose();
        }

        internal GrammarBuilder GetGrammarBuilder()
        {
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            
            foreach(IAskAction askAction in AskActions)
                grammarBuilder.Append(askAction.GetChoices());

            return grammarBuilder;
        }

        internal string Ask(string choice)
        {
            string result = null;

            foreach (IAskAction askAction in AskActions)
                if ((result = askAction.Ask(choice)) != null)
                    break;

            return result;
        }
    }
}
