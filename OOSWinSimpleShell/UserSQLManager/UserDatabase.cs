using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Cryptography;

namespace OOSWinSimpleShell.UserSQLManager
{
    public class UserDatabase
    {
        private readonly UsersContext context;
        public UserDatabase(string pathToDatabase)
        {
            this.context = new UsersContext(pathToDatabase);
        }

        public static byte[] Pepper { get; } = new byte[] { 25, 09, 19, 96 };

        public User GetUser(string username)
        {
            return this.context.Users.Where(u => u.Username == username).SingleOrDefault();
        }

        public void AddUser(string username, string password)
        {
            using var hasher = SHA1.Create();
            byte[] salt = new byte[16];
            new Random().NextBytes(salt);
            byte[] passBytes = Encoding.ASCII.GetBytes(password);
            byte[] passHash = hasher.ComputeHash(Pepper.Concat(salt).Concat(passBytes).ToArray());
            string dateTime = DateTime.Now.ToString("dddd, MMM dd yyyy, hh:mm:ss");

            User toAdd = new User
            {
                Username = username,
                Salt = salt,
                PassHash = passHash,
                LastLogin = dateTime,
            };

            this.context.Users.Add(toAdd);
            this.context.SaveChanges();
        }

        public void RemoveUser(string username)
        {
            this.context.Users.Remove(GetUser(username));
            this.context.SaveChanges();
        }

        public void UpdateLoginTime(User user, string lastLogin)
        {            
            user.LastLogin = lastLogin;
            this.context.SaveChanges();
        }
    }
}
