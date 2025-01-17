using CryptoExchange.Net.Attributes;

namespace HyperLiquid.Net.Enums
{
    /// <summary>
    /// Order type
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// Limit
        /// </summary>
        [Map("Limit")]
        Limit,
        /// <summary>
        /// Market
        /// </summary>
        [Map("Market")]
        Market,
        /// <summary>
        /// Stop Market
        /// </summary>
        [Map("Stop Market")]
        StopMarket,
        /// <summary>
        /// Stop Limit
        /// </summary>
        [Map("Stop Limit")]
        StopLimit
    }
}
