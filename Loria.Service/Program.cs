using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                if (args.Length > 0)
                {
                    switch (args[0])
                    {
                        case "-install":
                            {
                                // Install
                                ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });

                                // Start
                                ServiceController thisService = new ServiceController("Loria.Service");
                                thisService.Start();

                                break;
                            }
                        case "-uninstall":
                            {
                                // Stop
                                ServiceController thisService = new ServiceController("Loria.Service");
                                thisService.Stop();

                                // Uninstall
                                ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });

                                break;
                            }
                    }
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                { 
                    new LoriaService() 
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
