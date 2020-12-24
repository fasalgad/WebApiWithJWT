using System;
using System.Data;
using System.Data.SqlClient;

namespace webapi.Helpers
{
    public class Conexion
    {
        private string DataBaseName = "";
        private string IPServer = "";
        private string UserDataBase = "";
        private string PassDataBase = "";
        private string connString;
        private string query;
        private DataTable results;
        private string mensaje;
        private bool error;
        private string[,] param = null;
        private void setConextionDataBase()
        {
            connString = $"Data Source={this.IPServer};Initial Catalog={this.DataBaseName};User ID={this.UserDataBase};Password={this.PassDataBase};Connection Timeout=30000;";
        }
        protected Conexion()
        {
            this.setConextionDataBase();

            // DataTable results = new DataTable();
            this.error = false;
            this.mensaje = "";
        }
        protected Conexion(string query)
        {
            this.setConextionDataBase();
            // DataTable results = new DataTable();
            this.error = false;
            this.mensaje = "";
            this.query = query;
            this.param = null;
        }
        protected Conexion(string query, string[,] param)
        {
            this.setConextionDataBase();
            // DataTable results = new DataTable();
            this.error = false;
            this.mensaje = "";
            this.query = query;
            this.param = param;
        }


        protected void setQuery(string query)
        {
            this.query = query;
        }
        protected void setParam(string[,] param = null)
        {
            this.param = param;
        }
        public string GetMensaje()
        {
            return this.mensaje;
        }
        public bool GetError()
        {
            return this.error;
        }
        public DataTable GetResult()
        {
            return this.results;
        }

        protected DataTable Consulta()
        {
            this.results = new DataTable();
            SqlConnection CN = new SqlConnection(connString);
            try
            {
                SqlDataAdapter consulta = new SqlDataAdapter(this.query, CN);
                if (this.param != null)
                {
                    if (this.param.Length > 0)
                    {
                        consulta.SelectCommand.CommandTimeout = 3000000;
                        for (int i = 0; i < (this.param.Length / 2); i++)
                        {
                            consulta.SelectCommand.Parameters.AddWithValue(this.param[i, 0], this.param[i, 1]);
                        }
                    }
                }
                consulta.Fill(this.results);
                return results;
            }
            catch (System.Exception e)
            {
                this.results = new DataTable();
                this.error = true;
                this.mensaje = e.Message;
                CN.Close();
                return this.results;
            }
            finally
            {
                CN.Close();
            }
        }
    }
}
