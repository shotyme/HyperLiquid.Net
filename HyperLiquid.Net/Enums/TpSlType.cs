using CryptoExchange.Net.Attributes;

namespace HyperLiquid.Net.Enums
{
    /// <summary>
    /// TakeProfit/StopLoss type
    /// </summary>
    public enum TpSlType
    {
        /// <summary>
        /// Take profit
        /// </summary>
        [Map("tp")]
        TakeProfit,
        /// <summary>
        /// Stop loss
        /// </summary>
        [Map("sl")]
        StopLoss
    }
}
