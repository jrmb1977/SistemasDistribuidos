using System;
using System.Data;
using System.Threading.Tasks;
using Orleans;

namespace espol.sd.silohost
{
    public class GIFSGrainState
    {
        //Actualiza la cache
        public DataSet GifsAnimados { get; set; }
    }

    public class GIFSGrain : Grain<GIFSGrainState>, IGIFSGrain
    {
        // Carga en la caché los TOP 10 Gifs que estan en la Base de Datos 
        public async Task CargarGIFSALaCache()
        {
            bool CargarCache = false;
            DataSet oDS = new DataSet();
            ServicioDB.ServicioDB oServicio = new ServicioDB.ServicioDB();
            DataTable tbl = new DataTable();

            if (this.State.GifsAnimados == null)
                CargarCache = true;
            else
            {
                #region Comparo la caché con la Base de Datos para ver si es necesario actualizar la caché

                try
                {
                    if (this.State.GifsAnimados.Tables.Count > 0)
                    {
                        DataTable tblCache = this.State.GifsAnimados.Tables[0];
                        if (tblCache.Rows.Count > 0)
                        {
                            ServicioDB.RespuestaMetodoTop10Gifs Respuesta = oServicio.SelectTop10GifsDBId();
                            bool CacheEstaActualizada = true;
                            foreach (ServicioDB.GifImage Img in Respuesta.Imagenes)
                            {
                                int IdBBDD = Img.Id;
                                bool IdBBDDEstaEnCache = false;
                                foreach (DataRow Registro in tblCache.Rows)
                                {
                                    int IdCache = int.Parse(Registro["Id"].ToString());
                                    if (IdBBDD == IdCache)
                                    {
                                        IdBBDDEstaEnCache = true;
                                        break;
                                    }
                                }
                                if (!IdBBDDEstaEnCache)// Caché no esta actualizada
                                {
                                    CacheEstaActualizada = false;
                                    break;
                                }
                            }
                            if (CacheEstaActualizada)
                            {
                                foreach (ServicioDB.GifImage Img in Respuesta.Imagenes)
                                {
                                    int IdBBDD = Img.Id;
                                    int NumAccesos = Img.NumAccesos;
                                    // Actualizo el Numero de Accesos
                                    foreach (DataRow Registro in tblCache.Rows)
                                    {
                                        int IdCache = int.Parse(Registro["Id"].ToString());                                        
                                        if (IdBBDD == IdCache)
                                        {
                                            Registro["num_accesos"] = NumAccesos;
                                            break;
                                        }
                                    }
                                }

                                DataView dv = tblCache.DefaultView;
                                dv.Sort = "num_accesos desc";
                                tblCache = dv.ToTable();

                            }
                            else
                                CargarCache = true;
                        }
                        else
                            CargarCache = true;
                    }
                    else
                        CargarCache = true;
                }
                catch (Exception exc)
                {
                    string Error = string.Format("Error. Detalles:{0}", exc.Message);
                }

                #endregion
            }

            if (CargarCache)
            {
                #region Leo los TOP 10 Gifs de la Base de Datos con el objetivo de guardarlos en caché

                try
                {
                    #region Cargo los Datos de la Base de Datos

                    DateTime dtDBIni = DateTime.Now;
                    ServicioDB.RespuestaMetodoTop10Gifs Respuesta = oServicio.SelectTop10GifsDB();
                    DateTime dtDBFin = DateTime.Now;

                    #endregion

                    

                    #region Convierto a Tabla los campos que vienen separados en listas 

                    tbl.Columns.Add("Id", Type.GetType("System.Int32"));
                    tbl.Columns.Add("nombrearchivo", Type.GetType("System.String"));
                    tbl.Columns.Add("num_accesos", Type.GetType("System.Int32"));
                    tbl.Columns.Add("Imagen");
                    foreach (ServicioDB.GifImage Img in Respuesta.Imagenes)
                    {
                        int Id = Img.Id;
                        int NumAccesos = Img.NumAccesos;
                        string Nombre = Img.Nombre;
                        object Imagen = Img.Imagen;


                        DataRow Registro = tbl.NewRow();
                        Registro["Id"] = Id;
                        Registro["nombrearchivo"] = Nombre;
                        Registro["num_accesos"] = NumAccesos;
                        Registro["Imagen"] = Imagen;
                        tbl.Rows.Add(Registro);
                    }

                    #endregion

                    oDS.Tables.Add(tbl);

                    #region Estadisticas

                    //TimeSpan spanDB = dtDBFin.Subtract(dtDBIni);

                    //Log objLog = new Log();
                    //objLog.WriteTitulo("Estadisticas del Proceso - Seleccionar TOP 10 Gifs de la Base de Datos");
                    //objLog.WriteEvent(String.Format("Acceso BBDD - Tiempo Transcurrido  : {0} Hora(s) {1} Minuto(s) {2} Segundo(s)", spanDB.Hours, spanDB.Minutes, spanDB.Seconds));

                    #endregion
                }
                catch (Exception exc)
                {
                    string Error = string.Format("Error. Detalles:{0}", exc.Message);
                }

                #endregion

                //Se registra el DataSet traido de la Base de Datos en la caché
                this.State.GifsAnimados = oDS;

                //Se establece la escritura asincrónica para la actualización
                await this.WriteStateAsync();
            }
        }

        // Retorna los TOP 10 Gifs que están en el DataSet de la caché
        public Task<DataSet> LeerGIFSDesdeLaCache()
        {
            return Task.FromResult(this.State.GifsAnimados);
        }
    }
}
