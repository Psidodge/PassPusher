using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PassPusher
{
    public static class PasswordHasher
    {
        private const int saltLen = 10;


        public static byte[] GetHashedPassword(out byte[] salt, string password)
        {
            HashAlgorithm hashAlgorithm = new SHA512Managed();

            salt = GenerateSalt();
            byte[] saltedPass = Encoding.UTF8.GetBytes(password).Concat(salt).ToArray();
            return hashAlgorithm.ComputeHash(saltedPass);
        }

        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[saltLen];

            using (var rand = new RNGCryptoServiceProvider())
            {
                rand.GetNonZeroBytes(salt);
            }

            return salt;
        }
    }
}
