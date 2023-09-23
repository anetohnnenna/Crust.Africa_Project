using System.Text;

namespace UserAuthentication.Services
{
    public class Encryption
    {
        public static string key = "anms@@xcfgnjik@";

        public string ConvertToEncrypt(string password)
        {
            if (string.IsNullOrEmpty(password)) return "";
            password += key;
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(passwordBytes);
        }


        public string ConvertToDecrypt(string base64password)
        {
            if (string.IsNullOrEmpty(base64password)) return "";
            var base64EncodeBytes = Convert.FromBase64String(base64password);
            var result = Encoding.UTF8.GetString(base64EncodeBytes);
            result = result.Substring(0, result.Length - key.Length);
            return result;

        }
    }
}
