using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core.src.Loria.Module.CoreModules
{
    public class LoriaAnswer
    {
        public bool Suceeded { get; set; }
        public bool Speech { get; set; }
        public string SpeechText { get; set; }

        public LoriaAnswer() { }
        public LoriaAnswer(bool suceeded)
        {
            Suceeded = suceeded;
            Speech = false;
            SpeechText = string.Empty;
        }
        public LoriaAnswer(bool suceeded, bool speech, string speechText)
        {
            Suceeded = suceeded;
            Speech = speech;
            SpeechText = speechText;
        }
    }
}
