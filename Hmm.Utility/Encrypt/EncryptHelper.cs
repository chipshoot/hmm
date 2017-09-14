using System;
using System.Security.Cryptography;
using System.Text;

namespace Hmm.Utility.Encrypt
{
    public static class EncryptHelper
    {
        public static string GenerateSalt()
        {
            var buf = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(buf);
            return Convert.ToBase64String(buf);
        }

        public static string EncodePassword(string pass, string salt, bool isClearText)
        {
            // MembershipPasswordFormat.Clear
            if (isClearText)
            {
                return pass;
            }

            var bIn = Encoding.Unicode.GetBytes(pass);
            var bSalt = Convert.FromBase64String(salt);
            var bAll = new byte[bSalt.Length + bIn.Length];

            Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
            Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);

            // MembershipPasswordFormat.Hashed
            var s = SHA256.Create("SHA256");
            var bRet = s.ComputeHash(bAll);

            return Convert.ToBase64String(bRet);
        }
    }
}