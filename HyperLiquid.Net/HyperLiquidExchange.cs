using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Filters;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.RateLimiting;
using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.SharedApis;

namespace HyperLiquid.Net
{
    /// <summary>
    /// HyperLiquid exchange information and configuration
    /// </summary>
    public static class HyperLiquidExchange
    {
        /// <summary>
        /// Exchange name
        /// </summary>
        public static string ExchangeName => "HyperLiquid";

        /// <summary>
        /// Url to the main website
        /// </summary>
        public static string Url { get; } = "https://www.XXX.com";

        /// <summary>
        /// Urls to the API documentation
        /// </summary>
        public static string[] ApiDocsUrl { get; } = new[] {
            "XXX"
            };

        /// <summary>
        /// Type of exchange
        /// </summary>
        public static ExchangeType Type { get; } = ExchangeType.DEX;

        /// <summary>
        /// Format a base and quote asset to an HyperLiquid recognized symbol 
        /// </summary>
        /// <param name="baseAsset">Base asset</param>
        /// <param name="quoteAsset">Quote asset</param>
        /// <param name="tradingMode">Trading mode</param>
        /// <param name="deliverTime">Delivery time for delivery futures</param>
        /// <returns></returns>
        public static string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Rate limiter configuration for the HyperLiquid API
        /// </summary>
        public static HyperLiquidRateLimiters RateLimiter { get; } = new HyperLiquidRateLimiters();
    }

    /// <summary>
    /// Rate limiter configuration for the HyperLiquid API
    /// </summary>
    public class HyperLiquidRateLimiters
    {
        /// <summary>
        /// Event for when a rate limit is triggered
        /// </summary>
        public event Action<RateLimitEvent> RateLimitTriggered;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal HyperLiquidRateLimiters()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Initialize();
        }

        private void Initialize()
        {
            HyperLiquid = new RateLimitGate("HyperLiquid");
            HyperLiquid.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
        }


        internal IRateLimitGate HyperLiquid { get; private set; }

    }
}
