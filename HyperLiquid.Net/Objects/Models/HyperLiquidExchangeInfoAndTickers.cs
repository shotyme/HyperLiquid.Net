using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Models
{
    [JsonConverter(typeof(ArrayConverter))]
    public record HyperLiquidExchangeInfoAndTickers
    {
        [ArrayProperty(0), JsonConversion]
        public HyperLiquidExchangeInfo ExchangeInfo { get; set; }

        [ArrayProperty(1), JsonConversion]
        public IEnumerable<HyperLiquidTicker> Tickers { get; set; }
    }

    public record HyperLiquidTicker
    {
        [JsonPropertyName("prevDayPx")]
        public decimal PreviousDayPrice { get; set; }
        [JsonPropertyName("dayNtlVlm")]
        public decimal DayNotionalVlm { get; set; }
        [JsonPropertyName("markPx")]
        public decimal MarkPrice { get; set; }
        [JsonPropertyName("midPx")]
        public decimal? MidPrice { get; set; }
        [JsonPropertyName("circulatingSupply")]
        public decimal CirculatingSupply { get; set; }
        [JsonPropertyName("totalSupply")]
        public decimal TotalSupply { get; set; }
        [JsonPropertyName("dayBaseVlm")]
        public decimal DayBaseVlm { get; set; }
        [JsonPropertyName("coin")]
        public string Symbol { get; set; }
    }
}
