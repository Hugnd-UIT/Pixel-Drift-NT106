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
        public static string Generate_Key()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] keyBytes = new byte[32];
                rng.GetBytes(keyBytes);
                return BitConverter.ToString(keyBytes).Replace("-", "").ToLower().Substring(0, 32);
            }
        }

        public static string Encrypt(string PlainText, string Key)
        {
            try
            {
                using (Aes AES = Aes.Create())
                {
                    AES.Key = Encoding.UTF8.GetBytes(Key);
                    AES.GenerateIV();
                    byte[] IV = AES.IV;

                    ICryptoTransform Encryptor = AES.CreateEncryptor(AES.Key, IV);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        ms.Write(IV, 0, IV.Length);
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

                    byte[] IV = new byte[16];
                    byte[] Cipher = new byte[Buffer.Length - 16];

                    Array.Copy(Buffer, 0, IV, 0, 16);
                    Array.Copy(Buffer, 16, Cipher, 0, Cipher.Length);

                    AES.IV = IV;
                    ICryptoTransform Decryptor = AES.CreateDecryptor(AES.Key, AES.IV);

                    using (MemoryStream ms = new MemoryStream(Cipher))
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