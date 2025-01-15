using CryptoExchange.Net.Interfaces;
using System;

namespace HyperLiquid.Net.Interfaces.Clients.Api
{
    /// <summary>
    /// HyperLiquid  API endpoints
    /// </summary>
    public interface IHyperLiquidRestClientApi : IRestApiClient, IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        public IHyperLiquidRestClientApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        public IHyperLiquidRestClientApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        public IHyperLiquidRestClientApiTrading Trading { get; }

        /// <summary>
        /// Get the shared rest requests client. This interface is shared with other exhanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IHyperLiquidRestClientApiShared SharedClient { get; }
    }
}
