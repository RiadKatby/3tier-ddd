using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Web.Infrastructure.Encryption
{
    public interface IEncryptString
    {
        string Encrypt(string value);
        string Decrypt(string value);
        string Hash(string value);
        bool CompaireHash(string hashedText, string plainText);
        string Prefix { get; }
        int HashIterationCounts { get; }
    }
}
