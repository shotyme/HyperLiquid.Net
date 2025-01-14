using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Models
{
    public record HyperLiquidFuturesExchangeInfo
    {
        [JsonPropertyName("universe")]
        public IEnumerable<HyperLiquidFuturesSymbol> Symbols { get; set; }

    }


    public record HyperLiquidFuturesSymbol
    {
        [JsonPropertyName("name")]
        public string ExchangeName { get; set; }
        [JsonIgnore]
        public string Name => ExchangeName + "/USDT";
        [JsonPropertyName("szDecimals")]
        public int QuantityDecimals { get; set; }
        [JsonPropertyName("maxLeverage")]
        public int MaxLeverage { get; set; }

        [JsonIgnore]
        public int Index { get; set; }
    }
}
