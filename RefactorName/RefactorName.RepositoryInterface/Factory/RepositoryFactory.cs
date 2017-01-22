using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.RepositoryInterface
{
    public class RepositoryFactory
    {
        private static string DbProviderName;
        private static string WebSvcProviderName;

        private static Assembly DbProviderAssembly;
        private static Assembly WebSvcProviderAssembly;

        private static IGenericRepository repository;
        private static IGenericQueryRepository queryRepository;

        static RepositoryFactory()
        {
            DbProviderName = ConfigurationManager.AppSettings["DbProvider"];
            WebSvcProviderName = ConfigurationManager.AppSettings["WebSvcProvider"];
        }

        public static T Create<T>(string providerName, ref Assembly providerAssembly, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "Repository class name must not be null.");

            if (string.IsNullOrEmpty(providerName))
                throw new InvalidOperationException("[DbProvider] appSettings key is not defined or has no value.");

            if (providerAssembly == null)
                providerAssembly = Assembly.Load(providerName);

            return (T)providerAssembly.CreateInstance(providerName + "." + name);
        }

        public static T Create<T>(string name)
        {
            return Create<T>(DbProviderName, ref DbProviderAssembly, name);
        }

        public static T CreateWebSvc<T>(string name)
        {
            return Create<T>(WebSvcProviderName, ref WebSvcProviderAssembly, name);
        }

        public static IGenericRepository CreateRepository()
        {
            if (repository == null)
                repository = Create<IGenericRepository>("GenericRepository");

            return repository;
        }

        public static IGenericQueryRepository CreateQueryRepository()
        {
            if (queryRepository == null)
                queryRepository = Create<IGenericQueryRepository>("GenericQueryRepository");

            return queryRepository;
        }

        public static IUnitOfWork CreateUnitOfWork()
        {
            return Create<IUnitOfWork>("UnitOfWork");
        }
    }
}
