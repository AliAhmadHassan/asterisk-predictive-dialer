using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Silver.BLL
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    /// Data: 25/04/2013
    ///
    /// Criptografar e descriptografar strings a partir de uma chave privada
    /// </summary>
    public class Criptografia
    {
        private static byte[] bIV = { 0x50, 0x08, 0xF1, 0xDD, 0xDE, 0x3C, 0xF2, 0x18, 0x44, 0x74, 0x19, 0x2C, 0x53, 0x49, 0xAB, 0xBC };
        private const string cryptoKey = "Q3JpcHRvZ3JhZmlhcyBjb20gUmluamRhZWwgLyBBRVM=";

        public static string Criptografar(string text)
        {
            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    var bText = new UTF8Encoding().GetBytes(text);
                    var bKey = Convert.FromBase64String(cryptoKey);

                    Rijndael rijndael = new RijndaelManaged() { KeySize = 256 };

                    var mStream = new MemoryStream();

                    var encryptor = new CryptoStream(mStream, rijndael.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write);

                    encryptor.Write(bText, 0, bText.Length);

                    encryptor.FlushFinalBlock();

                    return Convert.ToBase64String(mStream.ToArray());
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao criptografar", ex);
            }
        }

        public static string Descriptografar(string text)
        {
            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    var bText = Convert.FromBase64String(text);
                    var bKey = Convert.FromBase64String(cryptoKey);

                    Rijndael rijndael = new RijndaelManaged() { KeySize = 256 };

                    var mStream = new MemoryStream();
                    var decryptor = new CryptoStream(mStream, rijndael.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write);

                    decryptor.Write(bText, 0, bText.Length);
                    decryptor.FlushFinalBlock();

                    var utf8 = new UTF8Encoding();
                    return utf8.GetString(mStream.ToArray());
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao descriptografar", ex);
            }
        }
    }
}
