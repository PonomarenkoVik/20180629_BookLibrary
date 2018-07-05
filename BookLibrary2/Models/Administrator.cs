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
    public class Administrator : DbContext
    {

        public async Task<List<string>> GetLogins()
        {
            string query = "GetAdministratorLogins";
            DataTable tbl = await GetDataAsync(query, null, CommandType.StoredProcedure);
            return Convert(tbl);
        }
   
        private static List<string> Convert(DataTable table)
        {
            List<string> adminLogins = new List<string>();
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    adminLogins.Add(table.Rows[i][0].ToString());
                } 
            }
            return adminLogins;
        }
    }
}