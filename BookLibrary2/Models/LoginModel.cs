using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookLibrary2.Models
{
    public class LoginModel
    {
        [Display(Name = "Login")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Inadmissible e-mail")]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Inadmissible password lenght ")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}