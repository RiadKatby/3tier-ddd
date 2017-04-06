using Mci.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace RefactorName.Web.Infrastructure.Encryption
{
    public class StringEncrypter
    {
        public static IEncryptString Obj { get; private set; }

        static StringEncrypter()
        {
            string EncrypterClassName = ConfigurationManager.AppSettings["StringEncrypter"].ToString();
            if (string.IsNullOrEmpty(EncrypterClassName))
                throw new InvalidOperationException("[StringEncrypter] appSettings key is not defined or has no value.");
            Type type = Type.GetType(EncrypterClassName);
            Obj = (IEncryptString)Activator.CreateInstance(type);
        }

    }
}