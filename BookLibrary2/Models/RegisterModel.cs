using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BookLibrary2.Controllers;

namespace BookLibrary2.Models
{
    public class RegisterModel : DbContext
    {
        [Required]
        [Display(Name = "First name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Inadmissible name lenght ")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Second name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Inadmissible name lenght ")]
        public string SecondName { get; set; }

        [Required]
        [Display(Name = "e-mail")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Inadmissible e-mail")]
        public string Email { get; set; }



        public async Task<bool> CheckEmail(string email)
        {
            SqlParameter outPar;
            var parameters = AddParam(email, out outPar);
            string query = "CheckEmail";
            await ExecuteAsync(query, parameters, CommandType.StoredProcedure);
            bool isAdmin = await AdminLogin.CheckLoginAdminAccess(email);
            bool permiss = (bool) outPar.Value && !isAdmin;
            return permiss;
        }


        private static List<SqlParameter> AddParam(string value, out SqlParameter outPar)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@email", value)
            };
            outPar = new SqlParameter
            {
                ParameterName = "@result",
                Direction = ParameterDirection.Output,
                DbType = DbType.Boolean
            };
            parameters.Add(outPar);
            return parameters;
        }
    }
}