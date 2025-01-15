using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using HyperLiquid.Net.Interfaces.Clients;
using HyperLiquid.Net.Objects.Options;
using HyperLiquid.Net.Interfaces.Clients.Api;
using HyperLiquid.Net.Clients.Api;
using CryptoExchange.Net.Objects.Options;

namespace HyperLiquid.Net.Clients
{
    /// <inheritdoc cref="IHyperLiquidSocketClient" />
    public class HyperLiquidSocketClient : BaseSocketClient, IHyperLiquidSocketClient
    {
        #region fields
        #endregion

        #region Api clients
                
         /// <inheritdoc />
        public IHyperLiquidSocketClientApi Api { get; }

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of HyperLiquidSocketClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public HyperLiquidSocketClient(Action<HyperLiquidSocketOptions>? optionsDelegate = null)
            : this(Options.Create(ApplyOptionsDelegate(optionsDelegate)), null)
        {
        }

        /// <summary>
        /// Create a new instance of HyperLiquidSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="options">Option configuration</param>
        public HyperLiquidSocketClient(IOptions<HyperLiquidSocketOptions> options, ILoggerFactory? loggerFactory = null) : base(loggerFactory, "HyperLiquid")
        {
            Initialize(options.Value);

            Api = AddApiClient(new HyperLiquidSocketClientApi(_logger, options.Value));
        }
        #endregion

        /// <inheritdoc />
        public void SetOptions(UpdateOptions options)
        {
            Api.SetOptions(options);
        }

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<HyperLiquidSocketOptions> optionsDelegate)
        {
            HyperLiquidSocketOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {
            Api.SetApiCredentials(credentials);
        }
    }
}
