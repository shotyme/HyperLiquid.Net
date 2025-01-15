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
        ///  API options
        /// </summary>
        public RestApiOptions Options { get; private set; } = new RestApiOptions();

        internal HyperLiquidRestOptions Set(HyperLiquidRestOptions targetOptions)
        {
            targetOptions = base.Set<HyperLiquidRestOptions>(targetOptions);            
            targetOptions.Options = Options.Set(targetOptions.Options);
            return targetOptions;
        }
    }
}
