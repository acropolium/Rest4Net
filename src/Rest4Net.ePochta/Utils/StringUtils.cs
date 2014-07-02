using System.Security.Cryptography;
using System.Text;

namespace Rest4Net.ePochta.Utils
{
    internal class StringUtils
    {
        public static string Md5(string input)
        {
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = MD5.Create().ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var t in hash)
                sb.Append(t.ToString("x2"));
            return sb.ToString();
        }
    }
}
