using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RefactorName.WebApp.Infrastructure
{
    /// <summary>
    /// Define <see cref="IEncryptString"/> implementation that depend on configuration based hashing key.
    /// </summary>
    public class ConfigurationBasedStringEncrypter : IEncryptString
    {
        public static readonly ICryptoTransform encrypter;
        public static readonly ICryptoTransform decrypter;
        private static string prefix;

        static ConfigurationBasedStringEncrypter()
        {
            // read settings from configuration
            string key = ConfigurationManager.AppSettings["EncryptionKey"];

            string useHashingString = ConfigurationManager.AppSettings["UseHashingForEncryption"];
            bool useHashing = true;
            if (string.Compare(useHashingString, "false", true) == 0)
                useHashing = false;

            string prefixCon = ConfigurationManager.AppSettings["EncryptionPrefix"];
            if (string.IsNullOrWhiteSpace(prefixCon))
                prefix = "encryptedHidden_";
            else
                prefix = prefixCon;

            byte[] keyArray = null;

            // compute the array value of key.
            if (useHashing)
                using (MD5CryptoServiceProvider hashMd5 = new MD5CryptoServiceProvider())
                    keyArray = hashMd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            // create the encrypter and decrypter objects
            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            })
            {
                encrypter = tdes.CreateEncryptor();
                decrypter = tdes.CreateDecryptor();
            }
        }

        #region IEncryptString Members

        /// <summary>
        /// Encrypt and Encode the value.
        /// </summary>
        /// <param name="value">string value that need to be encrypted and encoded.</param>
        /// <returns>Encrypted and decoded string.</returns>
        public string Encrypt(string value)
        {
            byte[] bytes = UTF8Encoding.UTF8.GetBytes(value);
            byte[] encryptedBytes = encrypter.TransformFinalBlock(bytes, 0, bytes.Length);
            string encrypted = Convert.ToBase64String(encryptedBytes);
            string encoded = HttpUtility.UrlEncode(encrypted);
            return encoded;
        }

        /// <summary>
        /// Decrypt and decode the value.
        /// </summary>
        /// <param name="value">string value that need to be decrypted and decoded.</param>
        /// <returns>Decrypted and decoded string.</returns>
        public string Decrypt(string value)
        {
            string decrypted = null;
            try
            {
                string decoded = HttpUtility.UrlDecode(value);
                byte[] bytes = Convert.FromBase64String(decoded);
                byte[] decryptedBytes = decrypter.TransformFinalBlock(bytes, 0, bytes.Length);
                decrypted = UTF8Encoding.UTF8.GetString(decryptedBytes);
            }
            catch { }

            return decrypted;
        }

        /// <summary>
        /// Gets prefix value that inserted in front of HTML controls to state it's value is encrypted.
        /// </summary>
        public string Prefix => prefix;

        #endregion
    }
}