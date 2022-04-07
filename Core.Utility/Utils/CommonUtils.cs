using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility.Utils
{
    public static class CommonUtils
    {
        public static DateTime GetDefaultDateTime()
        {
            return DateTime.Now;
        }

        public static DateTime GetUtcDateTime()
        {
            return DateTime.UtcNow;
        }

        public static DateTime GetParseDate(string date)
        {
            return DateTime.ParseExact(date.Substring(0, 10), "dd-MM-yyyy", CultureInfo.InvariantCulture);
        }

        public static string GetFormatedDate(DateTime date)
        {
            return date.ToString("dd-MM-yyyy");
        }

        public static string DescriptionAttr<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }

        public static string Encryption(string plaintext, string secretKey)
        {
            string res = "";
            try
            {

                byte[] src = Encoding.UTF8.GetBytes(plaintext);
                using (var aes = new AesCryptoServiceProvider())
                {
                    aes.BlockSize = 128;
                    aes.KeySize = 128;
                    byte[] IV = new byte[aes.BlockSize / 8];
                    aes.IV = IV;
                    aes.Key = Encoding.UTF8.GetBytes(secretKey);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.Zeros;
                    // decryption
                    using (ICryptoTransform encrypt = aes.CreateEncryptor(aes.Key, aes.IV))
                    {
                        byte[] encryptedText = encrypt.TransformFinalBlock(src, 0, src.Length);
                        res = Convert.ToBase64String(encryptedText, 0, encryptedText.Length);
                        aes.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                res = ex.ToString();
            }
            return res;


        }



        public static string Decryption(string encryptedText, string secretKey)
        {
            string res = "";
            try
            {
                byte[] src = Convert.FromBase64String(encryptedText); ;
                using (var aes = new AesCryptoServiceProvider())
                {
                    aes.BlockSize = 128;
                    aes.KeySize = 128;
                    byte[] IV = new byte[aes.BlockSize / 8];
                    aes.IV = IV;
                    aes.Key = Encoding.UTF8.GetBytes(secretKey);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.Zeros;
                    // decryption
                    using (ICryptoTransform decrypt = aes.CreateDecryptor(aes.Key, aes.IV))
                    {
                        byte[] decryptedText = decrypt.TransformFinalBlock(src, 0, src.Length);
                        res = Encoding.UTF8.GetString(decryptedText).Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                res = ex.ToString();
            }

            return res;
        }

    }
}
