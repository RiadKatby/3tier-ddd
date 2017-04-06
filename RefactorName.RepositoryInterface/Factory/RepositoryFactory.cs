using RefactorName.Core;
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
        private static Assembly DbProviderAssembly;
        private static Assembly WebSvcProviderAssembly;
        private static Assembly ConfigProviderAssembly;
        private static Assembly CacheProviderAssembly;

        private static IGenericRepository repository;
        private static IGenericQueryRepository queryRepository;

        private static void Initialize()
        {
            if (Settings.Provider == null)
            {
                string configProvider = ConfigurationManager.AppSettings["ConfigProvider"];
                Settings.Provider = Create<ISettingsProvider>(configProvider, ref ConfigProviderAssembly, "SettingsProvider");
            }
        }

        private static T Create<T>(string providerName, ref Assembly providerAssembly, string name, params object[] args)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name), "Repository class name must not be null.");

            if (string.IsNullOrEmpty(providerName))
                throw new InvalidOperationException("[DbProvider] appSettings key is not defined or has no value.");

            if (providerAssembly == null)
                providerAssembly = Assembly.Load(providerName);

            return (T)providerAssembly.CreateInstance(providerName + "." + name, false, BindingFlags.Default, null, args, null, null);
        }

        /// <summary>
        /// Build Database Repository instance.
        /// </summary>
        /// <typeparam name="T">Type of Repository Interface to build.</typeparam>
        /// <param name="name">repository concreate class name.</param>
        /// <returns>New Instance of T type.</returns>
        public static T CreateDbRepository<T>(string name)
        {
            Initialize();

            return Create<T>(Settings.Provider.DbProvider, ref DbProviderAssembly, name);
        }

        /// <summary>
        /// Build Web-Service Repository instance.
        /// </summary>
        /// <typeparam name="T">Type of Repository Interface to build.</typeparam>
        /// <param name="name">repository concreate class name.</param>
        /// <returns>New Instance of T Type.</returns>
        public static T CreateWebSvc<T>(string name)
        {
            Initialize();

            return Create<T>(Settings.Provider.WebSvcProviderName, ref WebSvcProviderAssembly, name);
        }

        public static ICachingProvider CreateCacheProvider()
        {
            Initialize();

            object[] args = null;

            if (Settings.Provider.CacheProvider.Contains("Redis"))
            {
                args = new object[] {
                    Settings.Provider.RedisServerHost,
                    Settings.Provider.RedisServerPort,
                    Settings.Provider.RedisServerPassword,
                    Settings.Provider.RedisServerSSL
                };
            }
            else if ("NoCachingProvider".Equals(Settings.Provider.CacheProvider))
                return new NoCachingProvider();

            return Create<ICachingProvider>(Settings.Provider.CacheProvider, ref CacheProviderAssembly, "CachingProvider", args);
        }

        public static IGenericRepository CreateRepository()
        {
            if (repository == null)
                repository = CreateDbRepository<IGenericRepository>("GenericRepository");

            return repository;
        }

        public static IGenericQueryRepository CreateQueryRepository()
        {
            if (queryRepository == null)
                queryRepository = CreateDbRepository<IGenericQueryRepository>("GenericQueryRepository");

            return queryRepository;
        }

        public static IUnitOfWork CreateUnitOfWork()
        {
            return CreateDbRepository<IUnitOfWork>("UnitOfWork");
        }
    }
}