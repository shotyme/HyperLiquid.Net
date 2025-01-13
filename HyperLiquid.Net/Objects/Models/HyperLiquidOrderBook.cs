using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Models
{
    public record HyperLiquidOrderBook
    {
        [JsonPropertyName("coin")]
        public string Asset { get; set; }
        [JsonPropertyName("time")]
        public DateTime Timespan { get; set; }

        [JsonPropertyName("levels")]
        public HyperLiquidOrderBookLevels Levels { get; set; } = null!;
    }

    [JsonConverter(typeof(ArrayConverter))]
    public record HyperLiquidOrderBookLevels
    {
        [ArrayProperty(0), JsonConversion]
        public IEnumerable<HyperLiquidOrderBookEntry> Bids { get; set; } = [];
        [ArrayProperty(1), JsonConversion]
        public IEnumerable<HyperLiquidOrderBookEntry> Asks { get; set; } = [];
    }

    public record HyperLiquidOrderBookEntry
    {
        [JsonPropertyName("px")]
        public decimal Price { get; set; }
        [JsonPropertyName("sz")]
        public decimal Quantity { get; set; }
        [JsonPropertyName("n")]
        public int Orders { get; set; }
    }
}
