using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Models
{
    /// <summary>
    /// Futures exchange info
    /// </summary>
    public record HyperLiquidFuturesExchangeInfo
    {
        /// <summary>
        /// Symbols
        /// </summary>
        [JsonPropertyName("universe")]
        public IEnumerable<HyperLiquidFuturesSymbol> Symbols { get; set; } = [];
    }

    /// <summary>
    /// Futures symbol info
    /// </summary>
    public record HyperLiquidFuturesSymbol
    {
        /// <summary>
        /// Symbol name as returned by the API
        /// </summary>
        [JsonPropertyName("name")]
        public string ExchangeName { get; set; } = string.Empty;
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonIgnore]
        public string Name => ExchangeName + "/USD";
        /// <summary>
        /// Decimal places for quantities
        /// </summary>
        [JsonPropertyName("szDecimals")]
        public int QuantityDecimals { get; set; }
        /// <summary>
        /// Max leverage
        /// </summary>
        [JsonPropertyName("maxLeverage")]
        public int MaxLeverage { get; set; }
        /// <summary>
        /// Index
        /// </summary>
        [JsonIgnore]
        public int Index { get; set; }
    }
}
