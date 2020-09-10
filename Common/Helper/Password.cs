using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public class Password
    {
        public static string AddStringPass = "$56aG@hj";
 

        public static string HashPassword(string strword)
        {
            if (string.IsNullOrEmpty(strword))
            {
               throw new Exception("String word not null");
            }
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(strword + AddStringPass);
            var hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
        public static bool CheckPassword(string password, string storedHash)
        {
            return HashPassword(password + AddStringPass) == storedHash;
        }
    }
}
