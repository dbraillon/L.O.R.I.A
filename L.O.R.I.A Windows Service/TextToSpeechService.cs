using L.O.R.I.A_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace L.O.R.I.A_Windows_Service
{
    partial class TextToSpeechService : ServiceBase
    {
        private bool IsRunning;

        public TextToSpeechService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Thread textToSpeechThread = new Thread(Start);
            textToSpeechThread.Start();

            IsRunning = true;
        }

        private void Start()
        {
            using (LoriaCore core = new LoriaCore())
            {
                while (IsRunning)
                    Thread.Sleep(1000);
            }
        }

        protected override void OnStop()
        {
            IsRunning = false;
        }
    }
}
