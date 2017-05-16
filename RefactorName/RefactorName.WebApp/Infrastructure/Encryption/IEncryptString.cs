using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.WebApp.Infrastructure
{
    /// <summary>
    /// Represents Encryption, Decryption and Hashing mechanism of strings.
    /// </summary>
    public interface IEncryptString
    {
        /// <summary>
        /// Encrypt and Encode the value.
        /// </summary>
        /// <param name="value">string value that need to be encrypted and encoded.</param>
        /// <returns>Encrypted and decoded string.</returns>
        string Encrypt(string value);

        /// <summary>
        /// Decrypt and decode the value.
        /// </summary>
        /// <param name="value">string value that need to be decrypted and decoded.</param>
        /// <returns>Decrypted and decoded string.</returns>
        string Decrypt(string value);

        /// <summary>
        /// Hashing the specified string.
        /// </summary>
        /// <param name="value">value to be hashed.</param>
        /// <returns>Hashed string.</returns>
        string Hash(string value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hashedText"></param>
        /// <param name="plainText"></param>
        /// <returns></returns>
        bool CompaireHash(string hashedText, string plainText);

        /// <summary>
        /// Gets prefix value that inserted in front of HTML controls to state it's value is encrypted.
        /// </summary>
        string Prefix { get; }

        /// <summary>
        /// 
        /// </summary>
        int HashIterationCounts { get; }

        bool IsEncryptionKeyExists { get; }
    }
}
