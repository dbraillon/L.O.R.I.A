using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Loria.Core.Senses
{
    public class SmellSense : ISense
    {
        private const string SightDirectoryKey = "SightDirectoryKey";
        
        private List<string> StimulusLoaded;
        private DirectoryInfo SightDirectory;
        private DirectoryInfo SightTempDirectory;
        private DirectoryInfo SightDoneDirectory;
        private DirectoryInfo SightErrorDirectory;
        private bool IsRunning;

        public List<string> Stimulus
        {
            get
            {
                if (StimulusLoaded == null) StimulusLoaded = new List<string>();

                return StimulusLoaded;
            }
            set
            {
                StimulusLoaded = value;
            }
        }

        public SmellSense()
        {
            string sightDirectoryPath = ConfigurationManager.AppSettings.Get(SightDirectoryKey);
            string sightTempDirectoryPath = Path.Combine(sightDirectoryPath, "temp");
            string sightDoneDirectoryPath = Path.Combine(sightDirectoryPath, "done");
            string sightErrorDirectoryPath = Path.Combine(sightDirectoryPath, "error");

            SightDirectory = new DirectoryInfo(sightDirectoryPath);
            SightTempDirectory = Directory.CreateDirectory(sightTempDirectoryPath);
            SightDoneDirectory = Directory.CreateDirectory(sightDoneDirectoryPath);
            SightErrorDirectory = Directory.CreateDirectory(sightErrorDirectoryPath);
        }

        public void StartSensing()
        {
            IsRunning = true;

            while (IsRunning)
            {
                Thread.Sleep(1000);
            }
        }

        public void StopSensing()
        {
            IsRunning = false;
        }

        public event StimulusRecognizedEventHandler StimulusRecognized;


        public void AddStimulus(params string[] stimulus)
        {
            throw new NotImplementedException();
        }
    }
}
