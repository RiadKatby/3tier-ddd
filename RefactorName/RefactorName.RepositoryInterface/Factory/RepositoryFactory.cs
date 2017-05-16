using RefactorName.Core;
using RefactorName.Core.Basis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.RepositoryInterface
{
    /// <summary>
    /// Encapsulate <see cref="Factory"/> generic class and specialize instantiation of Repositories, and Providers.
    /// </summary>
    public class RepositoryFactory
    {
        /// <summary>
        /// Instantiate new instance of <see cref="ICachingProvider"/>.
        /// </summary>
        /// <returns>New Instance of <see cref="ICachingProvider"/>.</returns>
        public static ICachingProvider CreateCacheProvider()
        {
            if ("NoCachingProvider".Equals(Settings.Provider.CacheProvider))
                return new NoCachingProvider();

            if (Settings.Provider.CacheProvider.Contains("Redis"))
            {
                var args = new object[] {
                    Settings.Provider.RedisServerHost,
                    Settings.Provider.RedisServerPort,
                    Settings.Provider.RedisServerPassword,
                    Settings.Provider.RedisServerSSL
                };

                return Factory.CreateCacheProvider<ICachingProvider>("CachingProvider", args);
            }

            /// In case of InMemory Caching not arguments required.
            return Factory.CreateCacheProvider<ICachingProvider>("CachingProvider");
        }

        /// <summary>
        /// Create new instance of <see cref="IGenericRepository"/> interface.
        /// </summary>
        /// <returns>New Instance of <see cref="IGenericRepository"/>.</returns>
        public static IGenericRepository CreateRepository()
        {
            return Factory.CreateDbRepository<IGenericRepository>("GenericRepository");
        }

        /// <summary>
        /// Create new instance of <see cref="IGenericQueryRepository"/> interface.
        /// </summary>
        /// <returns>New instance of <see cref="IGenericQueryRepository"/>.</returns>
        public static IGenericQueryRepository CreateQueryRepository()
        {
            return Factory.CreateDbRepository<IGenericQueryRepository>("GenericQueryRepository");
        }

        /// <summary>
        /// Create new instance of <see cref="IUnitOfWork"/> interface.
        /// </summary>
        /// <returns>New instance of <see cref="IUnitOfWork"/>.</returns>
        public static IUnitOfWork CreateUnitOfWork()
        {
            return Factory.CreateDbRepository<IUnitOfWork>("UnitOfWork");
        }
    }
}