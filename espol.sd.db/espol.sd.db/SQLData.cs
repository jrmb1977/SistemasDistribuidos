using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System;

namespace espol.sd.db
{
    public class SQLData
    {
        private SqlConnection oCon;
        private SqlCommand oCom;

        public SQLData()
        {
            string SQLConnstr = ConfigurationManager.ConnectionStrings["SQLCon"].ConnectionString;
            this.oCon = new SqlConnection(SQLConnstr);
            this.oCom = this.oCon.CreateCommand();            
        }

        public void Open()
        {
            this.oCon.Open();
        }

        public void Close()
        {
            if (this.oCon.State == ConnectionState.Open)
                this.oCon.Close();
        }

        public void ClearParameters()
        {
            this.oCom.Parameters.Clear();
        }

        public void AddParameter(string Name, object Value)
        {
            this.oCom.Parameters.AddWithValue(Name, Value);
        }

        public void AddParameterInputOutput(string Name, object Value)
        {
            this.oCom.Parameters.AddWithValue(Name, Value);
            this.oCom.Parameters[Name].Direction = ParameterDirection.InputOutput;
        }

        public void AddParameterOutput(string Name, object Value)
        {
            this.oCom.Parameters.AddWithValue(Name, Value);
            this.oCom.Parameters[Name].Direction = ParameterDirection.Output;
        }

        public DataTable SelectTable(string SQLQuery)
        {
            return this.GetDataSet(SQLQuery, CommandType.Text).Tables[0];
        }

        public DataTable SelectTableFromStoredProcedure(string StoredProcedureName)
        {
            return this.GetDataSet(StoredProcedureName, CommandType.StoredProcedure).Tables[0];
        }

        public DataSet Select(string SQLQuery)
        {
            return this.GetDataSet(SQLQuery, CommandType.Text);
        }

        public DataSet SelectFromStoredProcedure(string StoredProcedureName)
        {
            return this.GetDataSet(StoredProcedureName, CommandType.StoredProcedure);
        }

        private DataSet GetDataSet(string SQLQueryOrStoredProcedure, CommandType Tipo)
        {
            this.oCom.CommandText = SQLQueryOrStoredProcedure;
            this.oCom.CommandType = Tipo;
            SqlDataAdapter oDA = new SqlDataAdapter(this.oCom);
            DataSet ds = new DataSet();
            oDA.Fill(ds);
            return ds; 
        }

        public DataSet GetDataSetDosTablas(DataTable tblData)
        {
            DataSet ds = new DataSet();
            int NumProcesos = 2;
            int NumRegistros = tblData.Rows.Count;
            int NR = NumRegistros / NumProcesos;

            string Filtro1 = String.Format("RowNum>={0} and RowNum<={1}", 1, NR);
            string Filtro2 = String.Format("RowNum>={0} and RowNum<={1}", 1 + NR, NumRegistros);

            DataView dv = tblData.DefaultView;
            dv.RowFilter = Filtro1;
            DataTable tbl1 = dv.ToTable("Tabla1");

            dv.RowFilter = Filtro2;
            DataTable tbl2 = dv.ToTable("Tabla2");

            ds.Tables.Add(tbl1);
            ds.Tables.Add(tbl2);
            return ds;
        }

        public DataSet GetDataSetTresTablas_v1(DataTable tblData)
        {
            DataSet ds = new DataSet();
            int NumProcesos = 3;
            int NumRegistros = tblData.Rows.Count;
            int NR = NumRegistros / NumProcesos;
            int FR = 2 * NR;

            string Filtro1 = String.Format("RowNum>={0} and RowNum<={1}", 1, NR);
            string Filtro2 = String.Format("RowNum>={0} and RowNum<={1}", 1 + NR, FR);
            string Filtro3 = String.Format("RowNum>={0} and RowNum<={1}", 1 + FR, NumRegistros);

            DataView dv = tblData.DefaultView;
            dv.RowFilter = Filtro1;
            DataTable tbl1 = dv.ToTable("Tabla1");

            dv.RowFilter = Filtro2;
            DataTable tbl2 = dv.ToTable("Tabla2");

            dv.RowFilter = Filtro3;
            DataTable tbl3 = dv.ToTable("Tabla3");            

            ds.Tables.Add(tbl1);
            ds.Tables.Add(tbl2);
            ds.Tables.Add(tbl3);
            return ds;
        }

        public DataSet GetDataSetTresTablas(DataTable tblData)
        {
            DataSet ds = new DataSet();

            string Filtro1 = String.Format("Hilo={0}", 1);
            string Filtro2 = String.Format("Hilo={0}", 2);
            string Filtro3 = String.Format("Hilo={0}", 3);

            DataView dv = tblData.DefaultView;
            dv.RowFilter = Filtro1;
            DataTable tbl1 = dv.ToTable("Tabla1");

            dv.RowFilter = Filtro2;
            DataTable tbl2 = dv.ToTable("Tabla2");

            dv.RowFilter = Filtro3;
            DataTable tbl3 = dv.ToTable("Tabla3");

            ds.Tables.Add(tbl1);
            ds.Tables.Add(tbl2);
            ds.Tables.Add(tbl3);
            return ds;
        }

        public int ExecuteNonQuery(string SQLNonQuery)
        {
            return this.ExecuteNonQuery(SQLNonQuery, CommandType.Text);
        }

        public int ExecuteNonQueryFromStoredProcedure(string StoredProcedureName)
        {
            return this.ExecuteNonQuery(StoredProcedureName, CommandType.StoredProcedure);
        }

        private int ExecuteNonQuery(string SQLNonQueryOrStoredProcedure, CommandType Tipo)
        {
            this.oCom.CommandText = SQLNonQueryOrStoredProcedure;
            this.oCom.CommandType = Tipo;
            return this.oCom.ExecuteNonQuery();
        }

        public object ExecuteScalar(string SQLQuery)
        {
            return this.ExecuteScalar(SQLQuery, CommandType.Text);
        }

        public object ExecuteScalarFromStoredProcedure(string StoredProcedureName)
        {
            return this.ExecuteScalar(StoredProcedureName, CommandType.StoredProcedure);
        }

        private object ExecuteScalar(string SQLQueryOrStoredProcedure, CommandType Tipo)
        {
            this.oCom.CommandText = SQLQueryOrStoredProcedure;
            this.oCom.CommandType = Tipo;
            object result = this.oCom.ExecuteScalar();
            return result;            
        }

        public int GetLastInsertedIDInteger()
        {
            string SQLQuery = "Select @@Identity";
            object oID = this.ExecuteScalar(SQLQuery);
            return int.Parse(oID.ToString());
        }

        public int GetParameterOutInteger(string Name)
        {
            object oID = this.oCom.Parameters[Name].Value;
            return int.Parse(oID.ToString());
        }
    }
}