using Loria.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core.Senses
{
    public delegate void StimulusRecognizedEventHandler(string stimulus);

    public interface ISense
    {
        //Sense GetSense();
        void StartSensing();
        void StopSensing();
        void AddStimulus(params string[] stimulus);
        void ClearStimulus();

        event StimulusRecognizedEventHandler StimulusRecognized;
    }
}
