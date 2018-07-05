using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace BookLibrary2.Controllers
{
    public abstract class DbContext
    {
        protected static string GetConnString()
        {
            SqlConnectionStringBuilder connString = new SqlConnectionStringBuilder()
            {
                DataSource = "amazondatabase1.csp1feq9qvqu.us-east-2.rds.amazonaws.com,1433",
                InitialCatalog = "BookLibrary",
                UserID = "booklibrary1",
                Password = "booklibrary1",
                Pooling = true
            };
            return connString.ConnectionString;
        }

       
        protected async Task<DataTable> GetDataAsync(string query, List<SqlParameter> parameters, CommandType commType)
        {
            DataTable table = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(DbContext.GetConnString()))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.CommandType = commType;
                    AddParameters(parameters, adapter.SelectCommand);
                    await Task<int>.Factory.StartNew(() => adapter.Fill(table));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
            
            return table;
        }

        protected async Task ExecuteAsync(string query, List<SqlParameter> parameters, CommandType commType)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnString()))
                {
                    connection.Open();
                    SqlCommand comm = new SqlCommand(query, connection);
                    comm.CommandType = commType;
                    AddParameters(parameters, comm);
                    await Task<int>.Factory.StartNew(() => comm.ExecuteNonQuery());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }


        private static void AddParameters(List<SqlParameter> parameters, SqlCommand comm)
        {
            if (parameters != null)
            {
                foreach (var sqlParameter in parameters)
                {
                    comm.Parameters.Add(sqlParameter);
                }
            }
        }
    }
}