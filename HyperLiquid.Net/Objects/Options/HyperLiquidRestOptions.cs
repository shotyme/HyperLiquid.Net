using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Options;
using System;

namespace HyperLiquid.Net.Objects.Options
{
    /// <summary>
    /// Options for the HyperLiquidRestClient
    /// </summary>
    public class HyperLiquidRestOptions : RestExchangeOptions<HyperLiquidEnvironment, HyperLiquidCredentials>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static HyperLiquidRestOptions Default { get; set; } = new HyperLiquidRestOptions()
        {
            Environment = HyperLiquidEnvironment.Live
        };

        /// <summary>
        /// ctor
        /// </summary>
        public HyperLiquidRestOptions()
        {
            Default?.Set(this);
        }

        /// <summary>
        /// The builder fee percentage to apply to orders. Disabled by default in this fork; set together with <see cref="BuilderAddress"/> to collect the fee. Value can be between 0.001% and 0.1%.
        /// </summary>
        public decimal? BuilderFeePercentage { get; set; }

        /// <summary>
        /// Address of the builder
        /// </summary>
        public string BuilderAddress { get; set; } = string.Empty;

        /// <summary>
        /// Set this when the caller manages the builder fee approval itself, for example because the approval was signed by the master wallet
        /// outside of this library. The client then skips its own approval check and attaches <see cref="BuilderFeePercentage"/> to orders right away.
        /// Note that the exchange rejects an order whose builder fee exceeds the approved rate, so only set this for an approval you have verified.
        /// </summary>
        public bool BuilderFeeApproved { get; set; }

        /// <summary>
        /// If set requests will only be accepted by the server if they're received within (RequestTime + ExpiresAfter) timespan
        /// </summary>
        public TimeSpan? ExpiresAfter { get; set; }

        /// <summary>
        /// Spot API options
        /// </summary>
        public RestApiOptions SpotOptions { get; private set; } = new RestApiOptions();
        /// <summary>
        /// Futures API options
        /// </summary>
        public RestApiOptions FuturesOptions { get; private set; } = new RestApiOptions();

        internal HyperLiquidRestOptions Set(HyperLiquidRestOptions targetOptions)
        {
            targetOptions = base.Set<HyperLiquidRestOptions>(targetOptions);
            targetOptions.BuilderFeePercentage = BuilderFeePercentage;
            targetOptions.BuilderAddress = BuilderAddress;
            targetOptions.BuilderFeeApproved = BuilderFeeApproved;
            targetOptions.ExpiresAfter = ExpiresAfter;
            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);
            targetOptions.FuturesOptions = FuturesOptions.Set(targetOptions.FuturesOptions);
            return targetOptions;
        }
    }
}
