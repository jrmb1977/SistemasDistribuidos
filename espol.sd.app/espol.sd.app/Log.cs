using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace espol.sd.app
{
    public class Log
    {

        #region Variables Privadas

        private DateTime pdtEvent;
        private int pAnchoLog = 180;
        private string peventsDirectory;

        #endregion

        #region Propiedades Públicas

        public DateTime dtEvent
        {
            get { return pdtEvent; }
            set { pdtEvent = value; }
        }

        public int AnchoLog
        {
            get { return pAnchoLog; }
            set { pAnchoLog = value; }
        }

        public string CabeceraLog
        {
            get { return new string('-', this.AnchoLog); }
        }

        public string eventsDirectory
        {
            get { return peventsDirectory; }
            set { peventsDirectory = value; }
        }

        public string FileName
        {
            get { return String.Format("LogApp{0}.txt", this.dtEvent.ToString("yyyyMMdd")); }
        }

        #endregion

        #region Metodos Constructores

        public Log()
        {
            this.eventsDirectory = ConfigurationManager.AppSettings["LogErrorPath"];
            this.dtEvent = DateTime.Now;
        }

        #endregion

        #region Metodos Write

        public void WriteTitulo(string Titulo)
        {
            int LenTitulo = Titulo.Length;
            int Sangria = (this.AnchoLog - LenTitulo) / 2;
            this.WriteLog(this.CabeceraLog);
            this.WriteLog(Titulo.PadLeft(Sangria + LenTitulo));
            this.WriteLog(this.CabeceraLog);
        }

        public void WriteEvent(string text)
        {
            WriteFileEvent(text, this.FileName, true, true);
        }

        public void WriteLog(string text)
        {
            WriteFileEvent(text, this.FileName, false, false);
        }

        public void WriteLogLine(string text)
        {
            WriteFileEvent(text, this.FileName, true, false);
        }

        private void WriteFileEvent(string text, string FileName, bool AddTime, bool AddENTER)
        {
            bool HuboError = false;
            this.dtEvent = DateTime.Now;
            FileStream sb = null;
            StreamWriter sw = null;
            if (!Directory.Exists(this.eventsDirectory))
            {
                Directory.CreateDirectory(this.eventsDirectory);
            }
            try
            {
                string FilePath = Path.Combine(eventsDirectory, FileName);
                sb = new FileStream(FilePath, FileMode.Append);
                sw = new StreamWriter(sb);
                string LineaTexto = "";
                if (AddTime)
                {
                    string FormatoTiempo = String.Format("[{0}]", dtEvent.ToString("HH:mm:ss:fff"));
                    string Formato = FormatoTiempo;
                    LineaTexto += Formato;
                }
                LineaTexto += text;
                if (AddENTER)
                    LineaTexto += "\r\n";
                sw.WriteLine(LineaTexto);
            }
            catch (Exception exc)
            {
                string MensajeError = exc.Message;
                HuboError = true;
            }
            finally
            {
                if (!HuboError)
                {
                    if (sw != null)
                    {
                        sw.Close();
                        sw = null;
                    }
                    if (sb != null)
                    {
                        sb.Close();
                        sb = null;
                    }
                }
            }
        }

        #endregion
    }
}