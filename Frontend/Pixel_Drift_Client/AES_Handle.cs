using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pixel_Drift
{
    public static class AES_Handle
    {
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("PixelDrift2025!!");

        public static string Generate_Key()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 32);
        }

        public static string Encrypt(string PlainText, string Key)
        {
            try
            {
                using (Aes AES = Aes.Create())
                {
                    AES.Key = Encoding.UTF8.GetBytes(Key);
                    AES.IV = IV;
                    ICryptoTransform Encryptor = AES.CreateEncryptor(AES.Key, AES.IV);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, Encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                sw.Write(PlainText);
                            }
                            return Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public static string Decrypt(string CipherText, string Key)
        {
            try
            {
                byte[] Buffer = Convert.FromBase64String(CipherText);
                using (Aes AES = Aes.Create())
                {
                    AES.Key = Encoding.UTF8.GetBytes(Key);
                    AES.IV = IV;

                    ICryptoTransform Decryptor = AES.CreateDecryptor(AES.Key, AES.IV);
                    using (MemoryStream ms = new MemoryStream(Buffer))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, Decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}