using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEncryption
{
    public static class CryptographerBuilder
    {
        private static Crypter Crypter = new Crypter();

        public static string Encrypt(string value)
        {
            return Crypter.Encrypt(value);
        }

        public static string? Decrypt(string value)
        {
            return Crypter.Decrypt(value);
        }
    }
}
