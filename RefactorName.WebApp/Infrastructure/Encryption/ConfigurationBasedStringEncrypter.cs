using Mci.Security.Cryptography;
using RefactorName.Core;
using System;
using System.Configuration;

namespace RefactorName.WebApp.Infrastructure
{
    public class ConfigurationBasedStringEncrypter : IEncryptString
    {
        private static string prefix;
        private static int hashIterationCounts;
        private static byte[] keyArray, ivArray;
        static ConfigurationBasedStringEncrypter()
        {
            //read settings from configuration     
            var key = Settings.Provider.EncryptionKey;
            var iv = Settings.Provider.EncryptionIV;

            if (string.IsNullOrEmpty(key))
                throw new InvalidOperationException("[EncryptionKey] appSettings key is not defined or has no value.");

            if (string.IsNullOrEmpty(iv))
                throw new InvalidOperationException("[EncryptionIV] appSettings key is not defined or has no value.");

            prefix = Settings.Provider.EncryptionPrefix;
            if (string.IsNullOrWhiteSpace(prefix))
                prefix = "encryptedHidden_";

            hashIterationCounts = Settings.Provider.HashIterationCounts;

            keyArray = key.GetBytesFromHexString();
            ivArray = iv.GetBytesFromHexString();
        }

        #region IEncryptionSettingsProvider Members
        public string Encrypt(string value)
        {
            var encryptedBytes = Mci.Security.Cryptography.Encryption.Encrypt(value, keyArray, ivArray);
            var encrypted = Convert.ToBase64String(encryptedBytes);
            return encrypted;
        }
        public string Decrypt(string value)
        {
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
                return !(string.IsNullOrEmpty(ConfigurationManager.AppSettings["EncryptionKey"])
                         && string.IsNullOrEmpty(ConfigurationManager.AppSettings["EncryptionIV"]));
            }
        }
        #endregion
    }
}