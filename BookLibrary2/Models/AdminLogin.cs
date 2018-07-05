using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BookLibrary2.Models
{
    public static class AdminLogin
    {
        public static async Task<bool> CheckLoginAdminAccess(string login)
        {
            bool isAdministrator = false;
            List<string> logs = await Logins();
            foreach (var admLogin in logs)
            {
                if (login == admLogin)
                {
                    isAdministrator = true;
                    break;
                }
            }
            return isAdministrator;
        }

        private static async Task<List<string>> Logins()
        {
            if (_adminLogins == null)
            {
                _adminLogins = await (new Administrator()).GetLogins();
            }
            return (new List<string>(_adminLogins));
        }
        private static List<string> _adminLogins;
    }
}