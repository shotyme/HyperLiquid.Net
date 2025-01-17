using CryptoExchange.Net.Objects.Options;

namespace HyperLiquid.Net.Objects.Options
{
    /// <summary>
    /// Options for the HyperLiquidRestClient
    /// </summary>
    public class HyperLiquidRestOptions : RestExchangeOptions<HyperLiquidEnvironment>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static HyperLiquidRestOptions Default { get; set; } = new HyperLiquidRestOptions()
        {
            Environment = HyperLiquidEnvironment.Live,
            AutoTimestamp = true
        };

        /// <summary>
        /// ctor
        /// </summary>
        public HyperLiquidRestOptions()
        {
            Default?.Set(this);
        }
        
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
            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);
            targetOptions.FuturesOptions = FuturesOptions.Set(targetOptions.FuturesOptions);
            return targetOptions;
        }
    }
}
