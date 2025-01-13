using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperLiquid.Net.Enums
{
    /// <summary>
    /// Order side
    /// </summary>
    public enum OrderSide
    {
        /// <summary>
        /// Buy
        /// </summary>
        [Map("B")]
        Buy,
        /// <summary>
        /// Sell
        /// </summary>
        [Map("A")]
        Sell
    }
}
