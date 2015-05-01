using Loria.Core.Senses;
using Loria.Core.Skills;
using Loria.Core.Speeches;
using Loria.Dal.Entities;
using Loria.Dal.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core
{
    public class Brain : IBrain
    {
        private EventLog EventLog;

        public Brain(EventLog eventLog = null)
        {
            EventLog = eventLog;

            try
            {
                EventLog.WriteEntry("Configuring Loria hearing sense...");
                HearingSense.GetInstance().AddStimulus(LoriaTool.GetAllStimulus(senses: Sense.Hearing, eventLog: EventLog));
                HearingSense.GetInstance().StimulusRecognized += Sense_StimulusRecognized;
                HearingSense.GetInstance().StartSensing();
                EventLog.WriteEntry("Loria hearing sense configured...");

                MouthSpeech.GetInstance().Speech("Bonjour.");
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(string.Format("Can't talk {0}{1}", Environment.NewLine, e.ToString()), EventLogEntryType.Error);
            }
        }

        private void Sense_StimulusRecognized(string stimuli)
        {
            EventLog.WriteEntry(string.Concat("Loria recognized something : ", stimuli));

            Ability stimulatedAbility = LoriaTool.GetAbility(stimuli);

            if (stimulatedAbility != null)
            {
                foreach (Skill skill in stimulatedAbility.Skills)
                {
                    SkillAction skillAction = SkillLoader.GetSkillAction(skill);

                    if (skillAction != null)
                    {
                        skillAction.FirstLaunch();
                    }
                }
            }
        }
    }
}
