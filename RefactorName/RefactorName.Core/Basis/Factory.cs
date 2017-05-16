using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Basis
{
    /// <summary>
    /// Provides API and capabilities of creating objects.
    /// </summary>
    public static class Factory
    {
        private static Assembly DbProviderAssembly;
        private static Assembly WebSvcProviderAssembly;
        private static Assembly ConfigProviderAssembly;
        private static Assembly CacheProviderAssembly;

        /// <summary>
        /// Initialize the required values that are needed to Factory work.
        /// </summary>
        public static void Initialize()
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
        /// <param name="name">repository concrete class name.</param>
        /// <returns>New Instance of T type.</returns>
        public static T CreateDbRepository<T>(string name)
        {
            if (Settings.Provider == null)
                throw new InvalidOperationException("Settings.Provider is not initialized, you have to invoke Initialize() method first.");

            return Create<T>(Settings.Provider.DbProvider, ref DbProviderAssembly, name);
        }

        /// <summary>
        /// Build Web-Service Repository instance.
        /// </summary>
        /// <typeparam name="T">Type of Repository Interface to build.</typeparam>
        /// <param name="name">repository concrete class name.</param>
        /// <returns>New Instance of T Type.</returns>
        public static T CreateWebSvc<T>(string name)
        {
            if (Settings.Provider == null)
                throw new InvalidOperationException("Settings.Provider is not initialized, you have to invoke Initialize() method first.");

            return Create<T>(Settings.Provider.WebSvcProviderName, ref WebSvcProviderAssembly, name);
        }

        /// <summary>
        /// Build Caching-Provider instance.
        /// </summary>
        /// <typeparam name="T">Type of Caching Provider interface to build.</typeparam>
        /// <param name="name">caching provider concrete class name.</param>
        /// <param name="args">array of arguments that are required to instantiate the provider.</param>
        /// <returns>New instance of T type.</returns>
        public static T CreateCacheProvider<T>(string name, params object[] args)
        {
            if (Settings.Provider == null)
                throw new InvalidOperationException("Settings.Provider is not initialized, you have to invoke Initialize() method first.");

            return Create<T>(Settings.Provider.CacheProvider, ref CacheProviderAssembly, name, args);
        }
    }
}
