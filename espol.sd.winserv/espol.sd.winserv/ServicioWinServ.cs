using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using System.Text;

namespace espol.sd.winserv
{
    public partial class ServicioWinServ : ServiceBase
    {
        private Timer timerServicio;
        private DateTime dtEvent = DateTime.Today;
        private int pNumProceso = 0;

        public int NumProceso
        {
            get { return pNumProceso; }
            set { pNumProceso = value; }
        }

        public ServicioWinServ()
        {
            InitializeComponent();
            int SegundosTimer = int.Parse(ConfigurationManager.AppSettings["SegundosTimer"]) * 1000;
            this.timerServicio = new System.Timers.Timer() { AutoReset = true, Interval = SegundosTimer };
            this.timerServicio.Elapsed += TimerServicio_Elapsed;
        }

        private void TimerServicio_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                timerServicio.Stop();
                this.dtEvent = DateTime.Today;
                bool EsPrimeraVez = this.NumProceso == 0;
                ServicioDB.ServicioDB oServicio = new ServicioDB.ServicioDB();
                if (EsPrimeraVez)
                {
                    // Bandera para considerar si debo resetear en 0 todos los accesos a los gifs en la base de datos
                    bool RestartProcess = ConfigurationManager.AppSettings["RestartProcess"] == "S";
                    if (RestartProcess)
                        oServicio.SaveGifs();
                }
                int NumClientes = int.Parse(ConfigurationManager.AppSettings["NumClientes"]);
                oServicio.SimularAccesoGifs(NumClientes, true);
                this.NumProceso++;
            }
            catch (Exception exc)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", "Exception: " + exc.Message);
                string MensajeError = String.Format("{0}: {1}", dtEvent.ToString("HH:mm:ss"), exc.Message);
            }
            finally
            {
                timerServicio.Start();
            }
        }

        protected override void OnStart(string[] args)
        {
            this.NumProceso = 0;
            this.dtEvent = DateTime.Today;
            timerServicio.Start();
        }

        protected override void OnStop()
        {
            timerServicio.Stop();
        }
    }
}
