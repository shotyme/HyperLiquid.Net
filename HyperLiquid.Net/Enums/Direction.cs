using CryptoExchange.Net.Attributes;

namespace HyperLiquid.Net.Enums
{
    /// <summary>
    /// Direction
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// Open long
        /// </summary>
        [Map("Open Long")]
        OpenLong,
        /// <summary>
        /// Close long
        /// </summary>
        [Map("Close Long")]
        CloseLong,
        /// <summary>
        /// Open short
        /// </summary>
        [Map("Open Short")]
        OpenShort,
        /// <summary>
        /// Close short
        /// </summary>
        [Map("Close Short")]
        CloseShort

#warning spot?
    }
}
