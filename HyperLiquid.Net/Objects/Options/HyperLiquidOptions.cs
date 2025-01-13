using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperLiquid.Net.Objects.Options
{
    /// <summary>
    /// HyperLiquid options
    /// </summary>
    public class HyperLiquidOptions
    {
        /// <summary>
        /// Rest client options
        /// </summary>
        public HyperLiquidRestOptions Rest { get; set; } = new HyperLiquidRestOptions();

        /// <summary>
        /// Socket client options
        /// </summary>
        public HyperLiquidSocketOptions Socket { get; set; } = new HyperLiquidSocketOptions();

        /// <summary>
        /// Trade environment. Contains info about URL's to use to connect to the API. Use `HyperLiquidEnvironment` to swap environment, for example `Environment = HyperLiquidEnvironment.Live`
        /// </summary>
        public HyperLiquidEnvironment? Environment { get; set; }

        /// <summary>
        /// The api credentials used for signing requests.
        /// </summary>
        public ApiCredentials? ApiCredentials { get; set; }

        /// <summary>
        /// The DI service lifetime for the IHyperLiquidSocketClient
        /// </summary>
        public ServiceLifetime? SocketClientLifeTime { get; set; }
    }
}
