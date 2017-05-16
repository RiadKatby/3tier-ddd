using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    /// <summary>
    /// Represents all application specific settings that may be used though out all application layers.
    /// </summary>
    public interface ISettingsProvider
    {
        /// <summary>
        /// Gets database specifics implementation for IRepository interfaces.
        /// </summary>
        string DbProvider { get; }

        /// <summary>
        /// Gets web-service specifics implementation for IRepository interfaces.
        /// </summary>
        string WebSvcProviderName { get; }

        /// <summary>
        /// Gets caching provider implementation assembly for ICachingProvider interface.
        /// </summary>
        string CacheProvider { get; }

        /// <summary>
        /// Gets redis server host name or ip address.
        /// </summary>
        string RedisServerHost { get; }

        /// <summary>
        /// Gets redis server host port.
        /// </summary>
        int RedisServerPort { get; }

        /// <summary>
        /// Gets Redis server password.
        /// </summary>
        string RedisServerPassword { get; }

        /// <summary>
        /// Gets value determines wether SSL is used to connect redis server or not.
        /// </summary>
        bool RedisServerSSL { get; }

        /// <summary>
        /// Gets or sets whether bundling and minification of bundle references is enabled.
        /// </summary>
        bool EnableOptimizations { get; }

        /// <summary>
        /// Gets Danger Snackbar Message life time timeout.
        /// </summary>
        int SnackbarDangerMessageTimeout { get; }

        /// <summary>
        /// Gets Success Snackbar Message life time timeout.
        /// </summary>
        int SnackbarSuccessMessageTimeout { get; }

        /// <summary>
        /// Gets Info Snackbar Message life time timeout.
        /// </summary>
        int SnackbarInfoMessageTimeout { get; }

        /// <summary>
        /// Gets Warning Snackbar Message life time timeout.
        /// </summary>
        int SnackbarWarningMessageTimeout { get; }

        string EncryptionKey { get; }

        string EncryptionIV { get; }

        string EncryptionPrefix { get; }

        int HashIterationCounts { get; }
    }
}
