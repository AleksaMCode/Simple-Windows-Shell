using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOSWinSimpleShell.UserSQLManager;

namespace OOSWinSimpleShell
{
    public class LoginControl
    {
        public static bool Login(string username, string password, string loginTime)
        {
            UserDatabase db = new UserDatabase(@"C:\Users\Aleksa\source\repos\OOSWinSimpleShell\OOSWinSimpleShell\Users.db");
            var user = db.GetUser(username);

            if (user != null && user.IsPasswordValid(password))
            {
                Console.WriteLine("\nLast login: " + user.LastLogin);
                user.LastLogin = loginTime;
                db.UpdateLoginTime(user, loginTime);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
