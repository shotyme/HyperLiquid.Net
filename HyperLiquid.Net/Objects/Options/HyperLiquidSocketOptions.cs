using CryptoExchange.Net.Objects.Options;

namespace HyperLiquid.Net.Objects.Options
{
    /// <summary>
    /// Options for the HyperLiquidSocketClient
    /// </summary>
    public class HyperLiquidSocketOptions : SocketExchangeOptions<HyperLiquidEnvironment>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static HyperLiquidSocketOptions Default { get; set; } = new HyperLiquidSocketOptions()
        {
            Environment = HyperLiquidEnvironment.Live,
            SocketSubscriptionsCombineTarget = 10
        };


        /// <summary>
        /// ctor
        /// </summary>
        public HyperLiquidSocketOptions()
        {
            Default?.Set(this);
        }


        
         /// <summary>
        ///  API options
        /// </summary>
        public SocketApiOptions Options { get; private set; } = new SocketApiOptions();


        internal HyperLiquidSocketOptions Set(HyperLiquidSocketOptions targetOptions)
        {
            targetOptions = base.Set<HyperLiquidSocketOptions>(targetOptions);
            
            targetOptions.Options = Options.Set(targetOptions.Options);

            return targetOptions;
        }
    }
}
