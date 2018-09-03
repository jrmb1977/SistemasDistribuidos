using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace espol.sd.db
{
    [ServiceContract]
    public interface IServicioDB
    {
        [OperationContract]
        string[] SaveGifs();

        [OperationContract]
        string[] ImportGifs();

        [OperationContract]
        string[] ResetAccesosGifs();

        [OperationContract]
        RespuestaMetodoTop10Gifs SelectTop10GifsDBId();

        [OperationContract]
        RespuestaMetodoTop10Gifs SelectTop10GifsDB();

        [OperationContract]
        RespuestaMetodoTop10Gifs SelectTop10GifsCache();

        [OperationContract]
        int[] SimularAccesoGifs(int NumeroClientes);
    }

    [DataContract]
    public class GifImage
    {
        private byte[] pImagen;
        private int pId;
        private int pNumAccesos;
        private string pNombre;

        [DataMember]
        public byte[] Imagen
        {
            get { return pImagen; }
            set { pImagen = value; }
        }

        [DataMember]
        public int Id
        {
            get { return pId; }
            set { pId = value; }
        }

        [DataMember]
        public int NumAccesos
        {
            get { return pNumAccesos; }
            set { pNumAccesos = value; }
        }

        [DataMember]
        public string Nombre
        {
            get { return pNombre; }
            set { pNombre = value; }
        }
    }

    [DataContract]
    public class RespuestaMetodoTop10Gifs
    {
        private string pCodigoRetorno;
        private string pMensajeRetorno;
        private List<GifImage> pImagenes;

        [DataMember]
        public string CodigoRetorno
        {
            get { return pCodigoRetorno; }
            set { pCodigoRetorno = value; }
        }

        [DataMember]
        public string MensajeRetorno
        {
            get { return pMensajeRetorno; }
            set { pMensajeRetorno = value; }
        }

        [DataMember]
        public List<GifImage> Imagenes
        {
            get { return pImagenes; }
            set { pImagenes = value; }
        }
    }
}
