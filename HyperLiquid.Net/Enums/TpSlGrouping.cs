using CryptoExchange.Net.Attributes;

namespace HyperLiquid.Net.Enums
{
    /// <summary>
    /// TakeProfit/StopLoss grouping
    /// </summary>
    public enum TpSlGrouping
    {
        /// <summary>
        /// Normal TakeProfit/StopLoss
        /// </summary>
        [Map("normalTpsl")]
        NormalTpSl,
        /// <summary>
        /// Position TakeProfit/StopLoss
        /// </summary>
        [Map("positionTpsl")]
        PositionTpSl
    }
}
