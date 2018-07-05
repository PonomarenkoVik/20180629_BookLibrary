using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BookLibrary2.Controllers;

namespace BookLibrary2.Models
{
    public class Book : DbContext
    {
        public const short PageSize =  10;
        public long IdBook { get; set; }

        [Display(Name = "Book name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Inadmissible name lenght ")]
        public string NameBook { get; set; }

        [Display(Name = "Year of publication")]
        [Range(100, 2020, ErrorMessage = "Inadmissible name lenght ")]
        public short YearOfPublic { get; set; }

        [Display(Name = "Amount")]
        public short Amount { get; set; }
     


        public async Task DeleteAsync(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@idBook", id)
            };
            string query = "DeleteBook";
            await ExecuteAsync(query, parameters, CommandType.StoredProcedure);
        }

        public async Task<DataTable> GetAllAsync()
        {
            string query = "GetAllBooks";
            return await GetDataAsync(query, null, CommandType.StoredProcedure);
        }

        public async Task<Book> GetByIdAsync(long idBook)
        {
            Book book = new Book();
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@idBook", idBook)
            };
            string query = "GetBookById";
            return ConvertToBook(await GetDataAsync(query, parameters, CommandType.StoredProcedure));
        }

        public async Task UpdateAsync()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@idBook", IdBook),
                new SqlParameter("@bookName", NameBook),
                new SqlParameter("@publicationYear", YearOfPublic),
                new SqlParameter("@amount", Amount)          
            };
            string query = "UpdateBook";
            await ExecuteAsync(query, parameters, CommandType.StoredProcedure);
        }

       

        public async Task CreateAsync()
        {           
            string query = "AddBook";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@bookName", NameBook),
                new SqlParameter("@publicationYear", YearOfPublic),
                new SqlParameter("@amountOfBook", Amount)
            };
            await ExecuteAsync(query, parameters, CommandType.StoredProcedure);
        }


        private Book ConvertToBook(DataTable dataTable)
        {
            Book book = null;
            if (dataTable.Rows.Count == 1)
            {
                book = new Book
                {
                    IdBook = Convert.ToInt64(dataTable.Rows[0][0].ToString()),
                    NameBook = dataTable.Rows[0][1].ToString(),
                    YearOfPublic = Convert.ToInt16(dataTable.Rows[0][2].ToString()),
                    Amount = Convert.ToInt16(dataTable.Rows[0][3].ToString())
                };
            }
            return book;
        }

        public async Task<DataTable> GetBooksByAuthor(long idAuthor, int pageNumber )
        {
            DataTable book = new DataTable();
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@idAuthor", idAuthor),
                new SqlParameter("@pageNumber", pageNumber),
                new SqlParameter("@pageSize", PageSize)
            };
            string query = "GetBooksByAuthor";
            return await GetDataAsync(query, parameters, CommandType.StoredProcedure);
        }

        public async Task<DataTable> GetBooksByPage(int pageNumber, string searchString)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@pageNumber", pageNumber),
                new SqlParameter("@pageSize", PageSize)
            };
            if (!string.IsNullOrEmpty(searchString))
            {
                parameters.Add(new SqlParameter("@searchString", searchString));
            }
            string query = "GetBooksByPage";
            return await GetDataAsync(query, parameters, CommandType.StoredProcedure);
        }
    }
}