using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BookLibrary2.Controllers;

namespace BookLibrary2.Models
{
    public class History : DbContext
    {
        public long IdUsing { get; set; }
        public long IdBook { get; set; }
        public long IdUser { get; set; }
        public DateTime DateGetting { get; set; }
        public DateTime DateReturn { get; set; }


        public async Task<DataTable> GetByUser(long idUser)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@idUser", idUser)
            };
            string query = "GetUserHistory";
            return await GetDataAsync(query, parameters, CommandType.StoredProcedure);
        }

        public async Task<DataTable> GetByBook(long idBook)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@idBook", idBook)
            };
            string query = "GetBookHistory";
            return await GetDataAsync(query, parameters, CommandType.StoredProcedure);
        }

        public async Task<DataTable> GetAll()
        {          
            string query = "GetAllHistory";
            return await GetDataAsync(query, null, CommandType.StoredProcedure);
        }
    }
}