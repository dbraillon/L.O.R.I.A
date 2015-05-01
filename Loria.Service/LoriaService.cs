using Loria.Core;
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

namespace Loria.Service
{
    public partial class LoriaService : ServiceBase
    {
        private Thread LoriaThread;
        private bool IsRunning;

        public LoriaService()
        {
            InitializeComponent();
            LoriaThread = new Thread(LoriaLoop);
        }

        public void DebugStart()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("Service starting...");

            IsRunning = true;
            LoriaThread.Start();

            EventLog.WriteEntry("Service started...");
        }

        protected override void OnStop()
        {
            IsRunning = false;
            LoriaThread.Interrupt();
        }

        public void LoriaLoop()
        {
            EventLog.WriteEntry("Loria starting...");

            LoriaSoul loriaSoul = LoriaSoul.Live(EventLog);

            EventLog.WriteEntry("Loria started, entering the loop...");

            while (IsRunning)
            {
                Thread.Sleep(10000);
                EventLog.WriteEntry("loop...");
            }
        }
    }
}
