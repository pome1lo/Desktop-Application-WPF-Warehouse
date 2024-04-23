using System;
using System.Security.Cryptography;
using System.Text;

namespace DataEncryption
{
    public class Crypter
    {
        private string Key { get; } = "<RSAKeyValue><Modulus>q4Y9Y7WFChl64pI4jJluzF6C/93My2XCUYLO76hwJv9PbGyjMT9dmUw/m5C289Y0j/XaLI0jgO4I6NX1VOmybKnO+rZ95NppH54VT6VfATjdrym+6CpcoergFOsRb08leqINVCTl+yodfq6eHr1mQ9y/YdF6t6FOX/q1tihcxSU=</Modulus><Exponent>AQAB</Exponent><P>45YekSe+h43s5y8QG8JkOjX6sOgZ7IYhqgg5hoXC5riTrIN2IAJdh6fp2WvetIADw/KlvkK4XRSKZg5Blpp7Yw==</P><Q>wPBS7pTJjUYpXB92gFbuV8w1HYr7H37vAslOsge9IJd3rUdzj6mEKuH29EnUc19HdkiB/hZuV0OP53ZpJrXX1w==</Q><DP>PQgaBGU7JBD8cfbeBAO6ax3kr6JeqV5DEt0HyDqAzOy8tWu/ts/Lk0CFZsgVviQCXn7o0cAEvvluL/Yswp2E7w==</DP><DQ>l8Cuyh7XBLpJr77DeyBk6UOiB3GYIWa6YWuq7RZvKGJabD1F5JpFbWE711r2siQf1iYjsJE+Cn8Ggdy9yge/Ew==</DQ><InverseQ>2o6HxugEikRddLxVV58JZUdDN7ErPZmxEXBk2g7XqkTQ1yov8dq/KtLXUMfLbl6cWfe4GIdpefvDaVhAkS/Q2A==</InverseQ><D>da2EwrrPys0ObRHKsFO4G4igMbFXhxiKh+fJ18zlHSw+rnGeSPRjYABbB3zyuDn3F+mhxL0UZalp/WyFg7tOCHS4fkNNyDtac++OwHCXGSsrHqZhUDfZ9jDwRIFZWTocadvWjTleIgjTAxdB/kW5My40r7C9tbONhuI66s2H3UU=</D></RSAKeyValue>";
        private RSACryptoServiceProvider rsa { get; } = new();
        public string? Decrypt(string value)
        {
            rsa.FromXmlString(Key);
            return ToString(rsa.Decrypt(Convert.FromBase64String(value), true));
        }

        public string Encrypt(string value)
        {
            rsa.FromXmlString(Key);
            return Convert.ToBase64String(rsa.Encrypt(ToByte(value), true));
        }

        private byte[] ToByte(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        private string ToString(byte[] value)
        {
            return Encoding.UTF8.GetString(value);
        }
    }
}
