using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp.Infrastructure.Encryption
{
    public class StringEncrypter
    {
        public static IEncryptString UrlEncrypter { get; private set; }

        public static IEncryptString ControlsEncrypter { get; private set; }

        static StringEncrypter()
        {
            UrlEncrypter = new ConfigurationBasedStringEncrypter();

            ControlsEncrypter = new SessionBasedStringEncrypter();

            //string EncrypterClassName = ConfigurationManager.AppSettings["StringEncrypter"].ToString();
            //if (string.IsNullOrEmpty(EncrypterClassName))
            //    throw new InvalidOperationException("[StringEncrypter] appSettings key is not defined or has no value.");
            //Type type = Type.GetType(EncrypterClassName);
            //UrlEncrypter = (IEncryptString)Activator.CreateInstance(type);
        }

    }
}