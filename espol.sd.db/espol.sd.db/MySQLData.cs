using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace espol.sd.db
{
    public class MySQLData
    {
        private MySqlConnection oCon;
        private MySqlCommand oCom;

        public MySQLData()
        {
            string MySQLConn = ConfigurationManager.ConnectionStrings["MySQLConn"].ConnectionString;
            this.oCon = new MySqlConnection(MySQLConn);
            this.oCom = this.oCon.CreateCommand();
        }

        public void Open()
        {
            this.oCon.Open();
        }

        public void Close()
        {
            this.oCon.Close();
        }

        public DataTable Select(string SQLQuery)
        {
            return this.SelectFrom(SQLQuery, CommandType.Text);
        }

        public DataTable SelectFromStoredProcedure(string StoredProcedure)
        {
            return this.SelectFrom(StoredProcedure, CommandType.StoredProcedure);
        }

        public DataSet SelectDataSetFromStoredProcedure(string StoredProcedure)
        {
            return this.SelectDataSetFrom(StoredProcedure, CommandType.StoredProcedure);
        }

        public DataSet SelectDataSetFrom(string SQLQuery)
        {
            return this.SelectDataSetFrom(SQLQuery, CommandType.Text);
        }

        private DataTable SelectFrom(string StoredProcedure, CommandType tipo)
        {
            this.oCom.CommandText = StoredProcedure;
            this.oCom.CommandType = tipo;
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(this.oCom);
            DataTable dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);
            return dataTable;
        }

        private DataSet SelectDataSetFrom(string StoredProcedure, CommandType tipo)
        {
            this.oCom.CommandText = StoredProcedure;
            this.oCom.CommandType = tipo;
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(this.oCom);
            DataSet ds = new DataSet();
            mySqlDataAdapter.Fill(ds);
            return ds;
        }

        public object ExecuteScalar(string SQL)
        {
            this.oCom.CommandText = SQL;
            this.oCom.CommandType = CommandType.Text;
            object oData = this.oCom.ExecuteScalar();
            return oData;
        }

        public int ExecuteNonQuery(string SQLNonQuery)
        {
            return this.ExecuteSQLNonQuery(SQLNonQuery, CommandType.Text);
        }

        public int ExecuteNonQueryFromStoredProcedure(string StoredProcedure)
        {
            return this.ExecuteSQLNonQuery(StoredProcedure, CommandType.StoredProcedure);
        }

        private int ExecuteSQLNonQuery(string StoredProcedure, CommandType Tipo)
        {
            this.oCom.CommandText = StoredProcedure;
            this.oCom.CommandType = Tipo;
            return this.oCom.ExecuteNonQuery();
        }

        public void ClearParameters()
        {
            this.oCom.Parameters.Clear();
        }

        public void AddParameter(string Name, object Value)
        {
            this.oCom.Parameters.AddWithValue(Name, Value);
        }

        public void AddParameterNull(string name)
        {
            this.oCom.Parameters.AddWithValue(name, DBNull.Value);
        }
    }

}