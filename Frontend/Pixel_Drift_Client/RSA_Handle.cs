using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pixel_Drift
{
    public static class RSA_Handle
    {
        public static void Generate_Keys(out string Public_Key, out string Private_Key)
        {
            using (var RSA = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    Private_Key = RSA.ToXmlString(true);
                    Public_Key = RSA.ToXmlString(false);
                }
                finally
                {
                    RSA.PersistKeyInCsp = false;
                }
            }
        }

        public static string Encrypt(string PlainText, string Public_Key)
        {
            using (var RSA = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    RSA.FromXmlString(Public_Key);
                    byte[] Data = Encoding.UTF8.GetBytes(PlainText);
                    byte[] Encrypted_Data = RSA.Encrypt(Data, false);
                    return Convert.ToBase64String(Encrypted_Data);
                }
                catch
                {
                    return null;
                }
            }
        }

        public static string Decrypt(string CipherText, string Private_Key)
        {
            using (var RSA = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    RSA.FromXmlString(Private_Key);
                    byte[] Data = Convert.FromBase64String(CipherText);
                    byte[] Decrypted_Data = RSA.Decrypt(Data, false);
                    return Encoding.UTF8.GetString(Decrypted_Data);
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}