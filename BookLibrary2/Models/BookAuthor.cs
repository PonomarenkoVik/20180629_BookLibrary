using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using BookLibrary2.Controllers;

namespace BookLibrary2.Models
{
    public class BookAuthor : DbContext
    {
        public long IdBook { get; set; }
        public long IdAuthor { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public short YearOfBirth { get; set; }

        public async Task<DataTable> GetBooksAuthors(long idBook)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@idBook", idBook)
            };
            string query = "GetBooksAuthors";
            return await GetDataAsync(query, parameters, CommandType.StoredProcedure);
        }

        public async Task<DataTable> GetAbsentBooksAuthors(long idBook)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@idBook", idBook)
            };
            string query = "GetAbsentBooksAuthors";
            return await GetDataAsync(query, parameters, CommandType.StoredProcedure);
        }

        public async Task DeleteAsync(long idBook, long idAuthor)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@idBook", idBook),
                new SqlParameter("@idAuthor", idAuthor)
            };
            string query = "DeleteBookAuthorRelationship";
            await ExecuteAsync(query, parameters, CommandType.StoredProcedure);
        }

        public async Task Add(long idBook, long idAuthor)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@idBook", idBook),
                new SqlParameter("@idAuthor", idAuthor)
            };
            string query = "AddBookAuthorRelationship";
            await ExecuteAsync(query, parameters, CommandType.StoredProcedure);
        }

    }
}