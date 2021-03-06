﻿using System.Text;
using System.Security.Cryptography;
using System.Linq;
using System;

namespace OOSWinSimpleShell.UserSQLManager
{    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] Salt { get; set; }
        public byte[] PassHash { get; set; }
        public string LastLogin { get; set; }

        public bool IsPasswordValid(string password)
        {
            using var hasher = SHA1.Create();
            byte[] passBytes = Encoding.ASCII.GetBytes(password);
            var currentHash = hasher.ComputeHash(UserDatabase.Pepper.Concat(this.Salt).Concat(passBytes).ToArray());

            return currentHash.SequenceEqual(this.PassHash);
        }
    }
}
