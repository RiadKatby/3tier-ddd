using Mci.Security.Cryptography;
using System;
using System.Web;
using System.Configuration;

namespace RefactorName.WebApp.Infrastructure
{
    public class SessionBasedStringEncrypter : IEncryptString
    {
        private static string prefix;
        private static int hashIterationCounts;

        static SessionBasedStringEncrypter()
        {
            prefix = ConfigurationManager.AppSettings["EncryptionPrefix"];
            if (string.IsNullOrWhiteSpace(prefix))
                prefix = "encryptedHidden_";

            var hashIterationCountsConfig = ConfigurationManager.AppSettings["HashIterationCounts"];
            if (string.IsNullOrWhiteSpace(hashIterationCountsConfig))
                hashIterationCounts = 1;
            else
                hashIterationCounts = Int32.Parse(hashIterationCountsConfig);
        }
        private byte[] GetSesstionKey()
        {
            byte[] keyArray = null;
            var key = (string)HttpContext.Current.Session["EncryptionKey"] ?? string.Empty;
            if (string.IsNullOrEmpty(key))
            {
                keyArray = Mci.Security.Cryptography.Random.GenerateCrytpoRandomBytes(32);
                HttpContext.Current.Session["EncryptionKey"] = keyArray.ToHexString();
            }
            else
                keyArray = key.GetBytesFromHexString();

            return keyArray;
        }

        private byte[] GetSesstionIV()
        {
            byte[] ivArray = null;
            var iv = (string)HttpContext.Current.Session["EncryptionIV"] ?? string.Empty;
            if (string.IsNullOrEmpty(iv))
            {
                ivArray = Mci.Security.Cryptography.Random.GenerateCrytpoRandomBytes(16);
                HttpContext.Current.Session["EncryptionIV"] = ivArray.ToHexString();
            }
            else
                ivArray = iv.GetBytesFromHexString();

            return ivArray;
        }

        #region IEncryptionSettingsProvider Members
        public string Encrypt(string value)
        {
            //read the key from session
            var keyArray = GetSesstionKey();
            var ivArray = GetSesstionIV();
            var encryptedBytes = Mci.Security.Cryptography.Encryption.Encrypt(value, keyArray, ivArray);
            var encrypted = Convert.ToBase64String(encryptedBytes);
            return encrypted;
        }

        public string Decrypt(string value)
        {
            //read the key from session
            var keyArray = GetSesstionKey();
            var ivArray = GetSesstionIV();
            var bytes = Convert.FromBase64String(value);
            var decrypted = Mci.Security.Cryptography.Encryption.Decrypt(bytes, keyArray, ivArray);
            return decrypted;
        }

        public string Hash(string value)
        {
            byte[] sha512Hash = Hashing.GenerateHash(value, null, hashIterationCounts);
            return System.Text.Encoding.UTF8.GetString(sha512Hash);
        }

        public bool CompaireHash(string hashedText, string plainText)
        {
            plainText = Hash(plainText);
            return hashedText.Trim() == plainText.Trim();
        }

        public int HashIterationCounts
        {
            get { return hashIterationCounts; }
        }

        public string Prefix
        {
            get { return prefix; }
        }

        public bool IsEncryptionKeyExists
        {
            get
            {
                return (HttpContext.Current.Session["EncryptionKey"] != null
                         && HttpContext.Current.Session["EncryptionIV"] != null);
            }
        }
        #endregion
    }
}