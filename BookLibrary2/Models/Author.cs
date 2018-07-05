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
    public class Author : DbContext
    {
        public const short PageSize = 10;
       
        public long IdAuthor { get; set; }

        [Display(Name = "First name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Inadmissible name lenght ")]
        public string FirstName { get; set; }

        [Display(Name = "Second name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Inadmissible name lenght ")]
        public string SecondName { get; set; }

        [Display(Name = "Year of birth")]
        [Range(1200, 2008, ErrorMessage = "Inadmissible year")]
        public short YearOfBirth { get; set; }



        public async Task<DataTable> GetByPageAsync(int pageNumber, string searchString)
        {
            string query = "GetAuthorsByPage";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@pageNumber", pageNumber),
                new SqlParameter("@pageSize", PageSize),
                new SqlParameter("@searchString", searchString)
            };
            return await GetDataAsync(query, parameters, CommandType.StoredProcedure);
        }


        public async Task CreateAsync()
        {
            string query = "AddAuthor";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@firstName", FirstName),
                new SqlParameter("@secondName", SecondName),
                new SqlParameter("@yearOfBirth", YearOfBirth)
            };
            await ExecuteAsync(query, parameters, CommandType.StoredProcedure);
        }

        public async Task<Author> GetByIdAsync(long idAuthor)
        {
            Author author = new Author();
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@idAuthor", idAuthor)
            };
            string query = "GetAuthorById";
            return ConvertToAuthor(await GetDataAsync(query, parameters, CommandType.StoredProcedure));
        }

        public async Task UpdateAsync()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@idAuthor", IdAuthor),
                new SqlParameter("@firstName", FirstName),
                new SqlParameter("@secondName", SecondName),
                new SqlParameter("@yearOfBirth", YearOfBirth)
            };
            string query = "UpdateAuthor";
            await ExecuteAsync(query, parameters, CommandType.StoredProcedure);
        }


        private Author ConvertToAuthor(DataTable dataTable)
        {
            Author author = null;
            if (dataTable.Rows.Count == 1)
            {
                author = new Author()
                {
                    IdAuthor = Convert.ToInt64(dataTable.Rows[0][0].ToString()),
                    FirstName = dataTable.Rows[0][1].ToString(),
                    SecondName = dataTable.Rows[0][2].ToString(),
                    YearOfBirth = Convert.ToInt16(dataTable.Rows[0][3].ToString())
                };
            }
            return author;
        }
    }
}