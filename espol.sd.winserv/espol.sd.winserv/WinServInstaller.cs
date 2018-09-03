using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace espol.sd.winserv
{
    [RunInstaller(true)]
    public partial class WinServInstaller : Installer
    {
        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;

        public WinServInstaller()
        {
            InitializeComponent();

            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;

            serviceInstaller.StartType = ServiceStartMode.Manual;
            serviceInstaller.ServiceName = "ESPOLWINSERVSimuladorGif";
            serviceInstaller.Description = "Simula el acceso a los Gifs Animados";

            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller); 


        }
    }
}
