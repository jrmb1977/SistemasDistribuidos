using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace espol.sd.app
{
    public partial class wfTop10Gifs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SelectTop10GifsFromDataBase();
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            SelectTop10GifsFromDataBase();
        }

        private System.Drawing.Image StreamToImage(Log objLog, byte[] img, string nombre)
        {
            System.Drawing.Image imagen = null;
            try
            {
                MemoryStream stream = new MemoryStream(img);
                imagen = System.Drawing.Image.FromStream(stream);
                string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes");
                string FileName = String.Format("{0}\\{1}", FilePath, nombre);
                imagen.Save(FileName, System.Drawing.Imaging.ImageFormat.Gif);
            }
            catch (InvalidOperationException ex1)
            {
                string error = ex1.Message;
                objLog.WriteEvent(error);
                imagen = null;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                objLog.WriteEvent(error);
                imagen = null;
            }
            return imagen;
        }

        private void SelectTop10GifsFromDataBase()
        {
            ServicioDB.ServicioDB oServicio = new ServicioDB.ServicioDB();
            DataTable tbl = new DataTable();
            Log objLog = new Log();
            try
            {
                ServicioDB.RespuestaMetodoTop10Gifs Respuesta = new ServicioDB.RespuestaMetodoTop10Gifs();

                #region Cargo los Datos de la Base de Datos

                DateTime dtDBIni = DateTime.Now;
                if (rtbnlstOrigen.SelectedValue == "1")
                    Respuesta = oServicio.SelectTop10GifsDB();
                else
                    Respuesta = oServicio.SelectTop10GifsCache();
                DateTime dtDBFin = DateTime.Now;

                #endregion

                #region Leo el buffer con los gifs y los grabo en la carpeta Imagenes

                DateTime dtProcessGifIni = DateTime.Now;
                int j = 0;
                foreach (ServicioDB.GifImage imgn in Respuesta.Imagenes)
                {
                    string nombrearchivo = imgn.Nombre;
                    System.Drawing.Image imagen = StreamToImage(objLog, imgn.Imagen, nombrearchivo);
                    j++;
                }
                DateTime dtProcessGifFin = DateTime.Now;

                #endregion

                #region Convierto a Tabla los campos que vienen separados en listas 

                tbl.Columns.Add("Id");
                tbl.Columns.Add("nombrearchivo");
                tbl.Columns.Add("num_accesos");
                foreach (ServicioDB.GifImage imgn in Respuesta.Imagenes)
                {
                    DataRow Registro = tbl.NewRow();
                    Registro["Id"] = imgn.Id;
                    Registro["nombrearchivo"] = imgn.Nombre;
                    Registro["num_accesos"] = imgn.NumAccesos;
                    tbl.Rows.Add(Registro);
                }
                grdGifs.DataSource = tbl;
                grdGifs.DataBind();

                #endregion

                #region Direcciono las imagenes a la ruta url correspondiente

                DateTime dtLoadGifIni = DateTime.Now;
                j = 0;
                foreach (GridViewRow Registro in grdGifs.Rows)
                {
                    Image img = (Image)Registro.FindControl("img");
                    string FileName = Respuesta.Imagenes[j].Nombre;
                    img.ImageUrl = String.Format("Imagenes\\{0}", FileName);
                    j++;
                }
                DateTime dtLoadGifFin = DateTime.Now;

                #endregion

                #region Estadisticas

                TimeSpan spanDB = dtDBFin.Subtract(dtDBIni);
                TimeSpan spanProcessGif = dtProcessGifFin.Subtract(dtProcessGifIni);
                TimeSpan spanLoadGif = dtLoadGifFin.Subtract(dtLoadGifIni);
                
                objLog.WriteTitulo("Estadisticas del Proceso - Seleccionar TOP 10 Gifs de la Base de Datos");
                objLog.WriteEvent(String.Format("Acceso BBDD - Tiempo Transcurrido  : {0} Hora(s) {1} Minuto(s) {2} Segundo(s)", spanDB.Hours, spanDB.Minutes, spanDB.Seconds));
                objLog.WriteEvent(String.Format("Conversion  - Tiempo Transcurrido  : {0} Hora(s) {1} Minuto(s) {2} Segundo(s)", spanProcessGif.Hours, spanProcessGif.Minutes, spanProcessGif.Seconds));
                objLog.WriteEvent(String.Format("Carga Gifs  - Tiempo Transcurrido  : {0} Hora(s) {1} Minuto(s) {2} Segundo(s)", spanLoadGif.Hours, spanLoadGif.Minutes, spanLoadGif.Seconds));

                #endregion

                lblMensaje.Text = String.Format("Respuesta Servicio: {0} - {1}", Respuesta.CodigoRetorno, Respuesta.MensajeRetorno);
            }
            catch (Exception exc)
            {
                lblMensaje.Text = String.Format("Error. Detalles: {0}", exc.Message);
            }
        }
    }
}