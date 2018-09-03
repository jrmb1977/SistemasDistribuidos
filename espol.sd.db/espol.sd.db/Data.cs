using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace espol.sd.db
{
    public class Data
    {
        #region Querys

        public static DataSet SelectTop10GifsId(ref SQLData oDataBase)
        {            
            string SQLQuery = "SELECT TOP 10 [Id],[nombrearchivo],[num_accesos] ";
            SQLQuery += "FROM [dbo].[gifs_animados] ";
            SQLQuery += "ORDER BY[num_accesos] desc";
            oDataBase.ClearParameters();
            DataSet ds = oDataBase.Select(SQLQuery);
            return ds;
        }

        public static DataSet SelectTop10Gifs(ref SQLData oDataBase)
        {
            string SQLQuery = "SELECT TOP 10 [Id],[nombrearchivo],[archivo],[num_accesos] ";
            SQLQuery += "FROM [dbo].[gifs_animados] ";
            SQLQuery += "ORDER BY[num_accesos] desc";
            oDataBase.ClearParameters();
            DataSet ds = oDataBase.Select(SQLQuery);
            return ds;
        }

        public static int SelectCountGif(ref SQLData oDataBase)
        {
            string SQLQuery = "SELECT COUNT(*) FROM [dbo].[gifs_animados]";
            int Count = (int)oDataBase.ExecuteScalar(SQLQuery);
            return Count;
        }

        #region MySQL

        public static DataSet SelectTop10GifsId(ref MySQLData oDataBase)
        {
            string SQLQuery = "SELECT Id,nombrearchivo,num_accesos ";
            SQLQuery += "FROM gifs_animados ";
            SQLQuery += "ORDER BY num_accesos desc ";
            SQLQuery += "LIMIT 10 ";
            oDataBase.ClearParameters();
            DataSet ds = oDataBase.SelectDataSetFrom(SQLQuery);
            return ds;
        }

        public static DataSet SelectTop10Gifs(ref MySQLData oDataBase)
        {
            string SQLQuery = "SELECT Id,nombrearchivo,archivo,num_accesos ";
            SQLQuery += "FROM gifs_animados ";
            SQLQuery += "ORDER BY num_accesos desc ";
            SQLQuery += "LIMIT 10 ";
            oDataBase.ClearParameters();
            DataSet ds = oDataBase.SelectDataSetFrom(SQLQuery);
            return ds;
        }

        public static int SelectCountGif(ref MySQLData oDataBase)
        {
            string SQLQuery = "SELECT COUNT(*) FROM gifs_animados";
            object oData = oDataBase.ExecuteScalar(SQLQuery);
            int Count = int.Parse(oData.ToString());
            return Count;
        }

        #endregion

        #endregion

        #region Non Querys

        public static void UpdateContadorGif(ref SQLData oDataBase, int Id)
        {
            string SQLNonQuery = "UPDATE [dbo].[gifs_animados] ";
            SQLNonQuery += "SET [num_accesos] = [num_accesos] + 1 ";
            SQLNonQuery += "WHERE [Id] = @Id ";
            oDataBase.ClearParameters();
            oDataBase.AddParameter("@Id", Id);
            oDataBase.ExecuteNonQuery(SQLNonQuery);
        }

        public static void InsertGif(ref SQLData oDataBase, int Id, Byte[] imgByte, string nombrearchivo)
        {
            string SQLNonQuery = "INSERT INTO [dbo].[gifs_animados]([Id],[archivo],[nombrearchivo],[num_accesos]) ";
            SQLNonQuery += "VALUES(@Id, @archivo, @nombrearchivo,0) ";
            oDataBase.ClearParameters();
            oDataBase.AddParameter("@Id", Id);
            oDataBase.AddParameter("@archivo", imgByte);
            oDataBase.AddParameter("@nombrearchivo", nombrearchivo);
            oDataBase.ExecuteNonQuery(SQLNonQuery);
        }

        public static void DeleteGifs(ref SQLData oDataBase)
        {
            string SQLNonQuery = "DELETE FROM [dbo].[gifs_animados] ";
            oDataBase.ClearParameters();
            oDataBase.ExecuteNonQuery(SQLNonQuery);
        }


        #region MySQL

        public static void UpdateContadorGif(ref MySQLData oDataBase, int Id)
        {
            string SQLNonQuery = "UPDATE gifs_animados ";
            SQLNonQuery += "SET num_accesos = num_accesos + 1 ";
            SQLNonQuery += "WHERE Id = ?Id ";
            oDataBase.ClearParameters();
            oDataBase.AddParameter("Id", Id);
            oDataBase.ExecuteNonQuery(SQLNonQuery);
        }

        public static void InsertGif(ref MySQLData oDataBase, int Id, Byte[] imgByte, string nombrearchivo)
        {
            string SQLNonQuery = "INSERT INTO gifs_animados(Id,archivo,nombrearchivo,num_accesos) ";
            SQLNonQuery += "VALUES(?Id, ?archivo, ?nombrearchivo, 0) ";
            oDataBase.ClearParameters();
            oDataBase.AddParameter("Id", Id);
            oDataBase.AddParameter("archivo", imgByte);
            oDataBase.AddParameter("nombrearchivo", nombrearchivo);
            oDataBase.ExecuteNonQuery(SQLNonQuery);
        }

        public static int UpdateGifs_ResetNumAccesos(ref MySQLData oDataBase)
        {
            string SQLNonQuery = "UPDATE gifs_animados SET num_accesos = 0 ";
            oDataBase.ClearParameters();
            int n = oDataBase.ExecuteNonQuery(SQLNonQuery);
            return n;
        }

        public static void DeleteGifs(ref MySQLData oDataBase)
        {
            string SQLNonQuery = "DELETE FROM gifs_animados ";
            oDataBase.ClearParameters();
            oDataBase.ExecuteNonQuery(SQLNonQuery);
        }

        public static void SetMaxAllowedPacketSession(ref MySQLData oDataBase)
        {
            string SQLNonQuery = "SET SESSION max_allowed_packet=1024*1024*1024";
            oDataBase.ClearParameters();
            oDataBase.ExecuteNonQuery(SQLNonQuery);
        }

        #endregion

        #endregion

    }
}