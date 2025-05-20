using CryptoExchange.Net.Attributes;

namespace HyperLiquid.Net.Enums
{
    /// <summary>
    /// Order status
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Filled
        /// </summary>
        [Map("filled")]
        Filled,
        /// <summary>
        /// Open
        /// </summary>
        [Map("open")]
        Open,
        /// <summary>
        /// Canceled
        /// </summary>
        [Map("canceled", "reduceOnlyCanceled")]
        Canceled,
        /// <summary>
        /// Trigger
        /// </summary>
        [Map("triggered")]
        Triggered,
        /// <summary>
        /// Rejected
        /// </summary>
        [Map("rejected")]
        Rejected,
        /// <summary>
        /// Margin canceled
        /// </summary>
        [Map("marginCanceled")]
        MarginCanceled,

        /// <summary>
        /// Waiting for main order to fill before placing this order
        /// </summary>
        WaitingFill,
        /// <summary>
        /// Waiting for trigger price to be reached before placing this order
        /// </summary>
        WaitingTrigger
    }
}
