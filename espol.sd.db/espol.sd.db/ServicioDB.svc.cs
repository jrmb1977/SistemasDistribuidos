using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;
using Orleans;
using Orleans.Runtime.Configuration;

namespace espol.sd.db
{
    public class ServicioDB : IServicioDB
    {
        #region Metodos que traen los 10 gifs mas populares

        public RespuestaMetodoTop10Gifs SelectTop10GifsDB()
        {
            Log objLog = new Log();
            string Codigo = "000";
            string Mensaje = "OK";
            List<GifImage> Imagenes = new List<GifImage>();
            MySQLData oDataBase = new MySQLData();
            try
            {
                oDataBase.Open();
                DataSet dsDatos = Data.SelectTop10Gifs(ref oDataBase);
                if (dsDatos.Tables.Count > 0)
                {
                    DataTable tbl = dsDatos.Tables[0];
                    foreach (DataRow Registro in tbl.Rows)
                    {
                        int Id = int.Parse(Registro["Id"].ToString());
                        int num_accesos = int.Parse(Registro["num_accesos"].ToString());
                        string nombrearchivo = Registro["nombrearchivo"].ToString();
                        byte[] archivo = (byte[])Registro["archivo"];
                        GifImage Imagen = new GifImage();
                        Imagen.Id = Id;
                        Imagen.NumAccesos = num_accesos;
                        Imagen.Nombre = nombrearchivo;
                        Imagen.Imagen = archivo;
                        Imagenes.Add(Imagen);
                    }
                }
            }
            catch (Exception exc)
            {
                Codigo = "999";
                Mensaje = String.Format("Error.  Detalles: {0}", exc.Message);
            }

            oDataBase.Close();

            RespuestaMetodoTop10Gifs Respuesta = new RespuestaMetodoTop10Gifs();
            Respuesta.CodigoRetorno = Codigo;
            Respuesta.MensajeRetorno = Mensaje;
            Respuesta.Imagenes = Imagenes;

            LogSelectTop10Gifs(objLog, Respuesta);

            return Respuesta;
        }

        public RespuestaMetodoTop10Gifs SelectTop10GifsDBId()
        {
            Log objLog = new Log();
            string Codigo = "000";
            string Mensaje = "OK";
            MySQLData oDataBase = new MySQLData();
            List<GifImage> Imagenes = new List<GifImage>();
            try
            {
                oDataBase.Open();
                DataSet dsDatos = Data.SelectTop10GifsId(ref oDataBase);
                if (dsDatos.Tables.Count > 0)
                {
                    DataTable tbl = dsDatos.Tables[0];
                    foreach (DataRow Registro in tbl.Rows)
                    {
                        int Id = int.Parse(Registro["Id"].ToString());
                        int num_accesos = int.Parse(Registro["num_accesos"].ToString());
                        string nombrearchivo = Registro["nombrearchivo"].ToString();
                        GifImage Imagen = new GifImage();
                        Imagen.Id = Id;
                        Imagen.NumAccesos = num_accesos;
                        Imagen.Nombre = nombrearchivo;
                        Imagenes.Add(Imagen);
                    }
                }
            }
            catch (Exception exc)
            {
                Codigo = "999";
                Mensaje = String.Format("Error.  Detalles: {0}", exc.Message);
            }

            oDataBase.Close();

            RespuestaMetodoTop10Gifs Respuesta = new RespuestaMetodoTop10Gifs();
            Respuesta.CodigoRetorno = Codigo;
            Respuesta.MensajeRetorno = Mensaje;
            Respuesta.Imagenes = Imagenes;

            LogSelectTop10GifsId(objLog, Respuesta);

            return Respuesta;
        }

        #endregion

        #region Logs

        private void LogSelectTop10Gifs(Log objLog, RespuestaMetodoTop10Gifs Respuesta)
        {
            string Metodo = "SelectTop10Gifs";
            string Titulo = String.Format("Método {0} - Respuesta del Método", Metodo);
            objLog.WriteTitulo(Titulo);
            objLog.WriteLogLine(String.Format("Codigo                   : {0}", Respuesta.CodigoRetorno));
            objLog.WriteLogLine(String.Format("Mensaje                  : {0}", Respuesta.MensajeRetorno));

            int i = 0;
            foreach (GifImage imgn in Respuesta.Imagenes)
            {
                i++;
                objLog.WriteLogLine(String.Format("{0:00}.- Archivo GIF Id {1:0000} : {2:0000} acceso(s). Nombre: {3}", i, imgn.Id, imgn.NumAccesos, imgn.Nombre));
            }
        }

        private void LogSelectTop10GifsId(Log objLog, RespuestaMetodoTop10Gifs Respuesta)
        {
            string Metodo = "SelectTop10GifsId";
            string Titulo = String.Format("Método {0} - Respuesta del Método", Metodo);
            objLog.WriteTitulo(Titulo);
            objLog.WriteLogLine(String.Format("Codigo                   : {0}", Respuesta.CodigoRetorno));
            objLog.WriteLogLine(String.Format("Mensaje                  : {0}", Respuesta.MensajeRetorno));

            int i = 0;
            foreach (GifImage imgn in Respuesta.Imagenes)
            {
                i++;
                objLog.WriteLogLine(String.Format("{0:00}.- Archivo GIF Id {1:0000} : {2:0000} acceso(s).", i, imgn.Id, imgn.NumAccesos));
            }
        }

        private void LogSimularAccesoGifs(Log objLog, int[] Accesos)
        {
            string Metodo = "SimularAccesoGifs";
            string Titulo = String.Format("Método {0} - Respuesta del Método", Metodo);
            objLog.WriteTitulo(Titulo);

            int n = Accesos.Length;
            for (int i = 0; i < n; i++)
            {
                objLog.WriteLogLine(String.Format("Archivo GIF Id {0:0000}      : {1:0000} acceso(s).", i + 1, Accesos[i]));
            }
        }

        #endregion

        #region Metodos para la Simulacion

        // Base de Datos -> Carpeta
        public string[] ImportGifs()
        {
            string Codigo = "000";
            string Mensaje = "OK";
            int n = 0;
            int IndiceRead = 0;
            try
            {
                RespuestaMetodoTop10Gifs RespuestaSelect = SelectTop10GifsDB();
                if (RespuestaSelect.CodigoRetorno == "000")
                {
                    n = RespuestaSelect.Imagenes.Count;
                    int i = 0;
                    foreach (GifImage imgn in RespuestaSelect.Imagenes)
                    {
                        object img = imgn.Imagen;
                        string nombrearchivo = imgn.Nombre;
                        Image imagen = StreamToImage(img, nombrearchivo);
                        if (imagen != null)
                        {
                            IndiceRead++;
                        }
                        i++;
                    }
                }
                else
                {
                    Codigo = RespuestaSelect.CodigoRetorno;
                    Mensaje = RespuestaSelect.MensajeRetorno;
                }
            }
            catch (Exception exc)
            {
                Codigo = "999";
                Mensaje = String.Format("Error.  Detalles: {0}", exc.Message);
            }

            string[] Respuesta = new string[] { Codigo, Mensaje, n.ToString(), IndiceRead.ToString() };
            return Respuesta;
        }

        // Carpeta -> Base de Datos
        public string[] SaveGifs()
        {
            Log objLog = new Log();
            string Codigo = "000";
            string Mensaje = "OK";
            MySQLData oDataBase = new MySQLData();
            string GifExportPath = ConfigurationManager.AppSettings["GifExportPath"];
            int i = 0;
            int IndiceInsert = 0;
            int n = 0;
            try
            {
                oDataBase.Open();                
                string[] Archivos = Directory.GetFiles(GifExportPath, "*.gif");
                n = Archivos.Length;
                if (n > 0)
                    Data.DeleteGifs(ref oDataBase);
                foreach (string Archivo in Archivos)
                {
                    i++;
                    string NombreArchivo = Path.GetFileName(Archivo);
                    Image img = Image.FromFile(Archivo);
                    Byte[] imgByte = ImageToStream(img);
                    bool GifLeido = imgByte.Length > 0;
                    if (GifLeido)
                    {
                        Data.InsertGif(ref oDataBase, i, imgByte, NombreArchivo);
                        IndiceInsert++;
                    }
                }
            }
            catch (Exception exc)
            {
                Codigo = "999";
                Mensaje = String.Format("Error.  Detalles: {0}", exc.Message);
            }

            oDataBase.Close();

            string[] Respuesta = new string[] { Codigo, Mensaje, n.ToString(), IndiceInsert.ToString() };
            return Respuesta;
        }

        public string[] ResetAccesosGifs()
        {
            Log objLog = new Log();
            string Codigo = "000";
            string Mensaje = "OK";
            MySQLData oDataBase = new MySQLData();
            int n = 0;
            try
            {
                oDataBase.Open();
                n = Data.UpdateGifs_ResetNumAccesos(ref oDataBase);
            }
            catch (Exception exc)
            {
                Codigo = "999";
                Mensaje = String.Format("Error.  Detalles: {0}", exc.Message);
            }

            oDataBase.Close();

            string[] Respuesta = new string[] { Codigo, Mensaje, String.Format("{0} Registro(s) reseteado(s)", n) };
            return Respuesta;
        }

        // Aumenta los contadores de manera aleatoria
        public int[] SimularAccesoGifs(int NumeroClientes)
        {
            Log objLog = new Log();
            int NumeroGifs = 0;
            int[] Accesos = new int[NumeroGifs];
            Random rnd = new Random();
            MySQLData oDataBase = new MySQLData();
            try
            {
                oDataBase.Open();
                NumeroGifs = Data.SelectCountGif(ref oDataBase);
                Accesos = new int[NumeroGifs];
                for (int j = 0; j < NumeroGifs; j++)
                    Accesos[j] = 0;
                for (int i = 0; i < NumeroClientes; i++)
                {
                    // IndiceGifGenerado >= 1 and < NumeroGifs + 1
                    int IndiceGifGenerado = rnd.Next(1, NumeroGifs + 1);
                    Accesos[IndiceGifGenerado - 1] = Accesos[IndiceGifGenerado - 1] + 1;
                }
                for (int j = 0; j < NumeroGifs; j++)
                {
                    int n = Accesos[j];
                    int Id = j + 1;
                    for (int k = 0; k < n; k++)
                        Data.UpdateContadorGif(ref oDataBase, Id);
                }
            }
            catch (Exception exc)
            {
                string Error = exc.Message;
            }

            oDataBase.Close();

            LogSimularAccesoGifs(objLog, Accesos);

            return Accesos;
        }

        #endregion

        #region Metodos de Ayuda

        // Base de Datos -> Carpeta
        private Image StreamToImage(object img, string nombre)
        {
            Image imagen = null;
            try
            {
                MemoryStream stream = new MemoryStream((byte[])img);
                imagen = Image.FromStream(stream);
                string GifImportPath = ConfigurationManager.AppSettings["GifImportPath"];
                string FileName = String.Format("{0}{1}", GifImportPath, nombre);
                imagen.Save(FileName, ImageFormat.Gif);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                imagen = null;
            }
            return imagen;
        }

        // Carpeta -> Base de Datos
        private byte[] ImageToStream(Image img)
        {
            MemoryStream stream = new MemoryStream();
            try
            {
                img.Save(stream, ImageFormat.Gif);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return stream.ToArray();
        }

        #endregion

        public RespuestaMetodoTop10Gifs SelectTop10GifsCache()
        {
            Log objLog = new Log();
            string Codigo = "000";
            string Mensaje = "OK";
            List<GifImage> Imagenes = new List<GifImage>();

            String fqdnSILO1 = ConfigurationManager.AppSettings["fqdnSILO"];
            int tcpSILO1 = int.Parse(ConfigurationManager.AppSettings["tcpSILO"]);
            DataSet oDS = new DataSet();
            try
            {
                //Se define la configuración y se instancia el FrontEnd (Cliente del SILO)
                ClientConfiguration clientConfig = new ClientConfiguration { GatewayProvider = ClientConfiguration.GatewayProviderType.Config };

                //Para ello usaremos la dirección IPv4 que nos resuelva el DNS
                IPHostEntry ipHostInfo = Dns.GetHostEntry(fqdnSILO1);
                IPAddress ipSILO1 = ipHostInfo.AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
                clientConfig.Gateways.Add(new IPEndPoint(ipSILO1, tcpSILO1));
                IClusterClient client = new ClientBuilder().UseConfiguration(clientConfig).Build();

                //Se realiza la conexión al SILO
                client.Connect().Wait();

                //Se define el objeto objRemoto para referenciar el grain de la interfaz IGIFSGrain del SILO
                IGIFSGrain objRemoto = client.GetGrain<IGIFSGrain>(0);

                //Definimos el DataSet oDS y registramos en el mismo, el DataSet de noticias de la cache, que se 
                //obtiene del método "LeerNoticias", declarado en el SILO

                objRemoto.CargarGIFSALaCache().Wait();
                oDS = objRemoto.LeerGIFSDesdeLaCache().Result;

                //Se cierra la conexión con el SILO
                client.Close();

                if (oDS.Tables.Count > 0)
                {
                    DataTable tbl = oDS.Tables[0];
                    foreach (DataRow Registro in tbl.Rows)
                    {
                        int Id = int.Parse(Registro["Id"].ToString());
                        int num_accesos = int.Parse(Registro["num_accesos"].ToString());
                        string nombrearchivo = Registro["nombrearchivo"].ToString();
                        byte[] archivo = (byte[])Registro["archivo"];
                        GifImage Imagen = new GifImage();
                        Imagen.Id = Id;
                        Imagen.NumAccesos = num_accesos;
                        Imagen.Nombre = nombrearchivo;
                        Imagen.Imagen = archivo;
                        Imagenes.Add(Imagen);
                    }
                }

            }
            catch (Exception exc)
            {
                Codigo = "999";
                Mensaje = String.Format("Error.  Detalles: {0}", exc.Message);
            }

            RespuestaMetodoTop10Gifs Respuesta = new RespuestaMetodoTop10Gifs();
            Respuesta.CodigoRetorno = Codigo;
            Respuesta.MensajeRetorno = Mensaje;
            Respuesta.Imagenes = Imagenes;

            LogSelectTop10Gifs(objLog, Respuesta);

            return Respuesta;
        }
    }
}