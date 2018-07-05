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
    [Authorize]
    public class User : DbContext
    {
        public long IdUser { get; set; }

        [Display(Name = "First name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Inadmissible name lenght ")]
        public string FirstName { get; set; }

        [Display(Name = "Second name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Inadmissible name lenght ")]
        public string SecondName { get; set; }

        [Display(Name = "e-mail")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Inadmissible e-mail")]
        public string Email { get; set; }


      
        public async Task<DataTable> GetAllAsync()
        {
            string query = "GetAllUsers";
            return await GetDataAsync(query, null, CommandType.StoredProcedure);
        }

    
        public async Task CreateAsync()
        {
            string query = "AddUser";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@firstName", FirstName),
                new SqlParameter("@secondName", SecondName),
                new SqlParameter("@email", Email)
            };
            await ExecuteAsync(query, parameters, CommandType.StoredProcedure);
        }

   
        public async Task<User> GetByIdAsync(long idUser)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@idUser", idUser)
            };
            string query = "GetUserById";
            return ConvertToUser(await GetDataAsync(query, parameters, CommandType.StoredProcedure));
        }


        public async Task<User> GetByEmailAsync(string email)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@email", email)
            };
            string query = "GetUserByEmail";
            return ConvertToUser(await GetDataAsync(query, parameters, CommandType.StoredProcedure));
        }

        public async Task UpdateAsync()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@idUser", IdUser),
                new SqlParameter("@firstName", FirstName),
                new SqlParameter("@secondName", SecondName),
                new SqlParameter("@email", Email)
            };
            string query = "UpdateUser";
            await ExecuteAsync(query, parameters, CommandType.StoredProcedure);
        }


        public async Task<DataTable> GetAbsentBooksAsync(long idUser)
        {
            DataTable table = new DataTable();
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@idUser", idUser)
            };
            string query = "GetAbsentUserBooks";
            return await GetDataAsync(query, parameters, CommandType.StoredProcedure);
        }

        public async Task ReceiveTheBook(long idUser, long idBook)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@idUser", idUser),
                new SqlParameter("@idBook", idBook),               
            };
            string query = "ReceiveBook";
            await ExecuteAsync(query, parameters, CommandType.StoredProcedure);
        }

        public async Task<DataTable> GetUserBooks(long idUser)
        {
            DataTable table = new DataTable();
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@idUser", idUser)
            };
            string query = "GetUserBooks";
            return await GetDataAsync(query, parameters, CommandType.StoredProcedure);
        }

        public async Task Return(long idUser, long idBook)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@idUser", idUser),
                new SqlParameter("@idBook", idBook),
            };
            string query = "ReturnBook";
            await ExecuteAsync(query, parameters, CommandType.StoredProcedure);
        }

        private User ConvertToUser(DataTable dataTable)
        {
            User user = null;
            if (dataTable.Rows.Count == 1)
            {
                user = new User()
                {
                    IdUser = Convert.ToInt64(dataTable.Rows[0][0].ToString()),
                    FirstName = dataTable.Rows[0][1].ToString(),
                    SecondName = dataTable.Rows[0][2].ToString(),
                    Email = dataTable.Rows[0][3].ToString()
                };
            }

            return user;
        }
    }
}