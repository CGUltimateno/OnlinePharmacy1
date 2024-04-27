using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace OnlinePharmacy.Helpers
{
    public class TripleDesEncryptionHelper
    {
        private const string Key = "VptzbIEE5yXFHR0kSe3lN1s="; // 24 bytes

        public static string Encrypt(string plainText)
        {
            using var tripleDes = CreateTripleDes();
            ICryptoTransform encryptor = tripleDes.CreateEncryptor();

            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedData = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);

            return Convert.ToBase64String(encryptedData);
        }

        public static string Decrypt(string encryptedText)
        {
            byte[] encryptedData = Convert.FromBase64String(encryptedText);

            using var tripleDes = CreateTripleDes();
            ICryptoTransform decryptor = tripleDes.CreateDecryptor();

            byte[] decryptedData = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

            return Encoding.UTF8.GetString(decryptedData);
        }

        private static TripleDESCryptoServiceProvider CreateTripleDes()
        {
            var tripleDes = new TripleDESCryptoServiceProvider
            {
                Key = Encoding.UTF8.GetBytes(Key),
                Mode = CipherMode.ECB, // Electronic Codebook (ECB) mode
                Padding = PaddingMode.PKCS7 // PKCS7 padding
            };

            return tripleDes;
        }
    }
}
