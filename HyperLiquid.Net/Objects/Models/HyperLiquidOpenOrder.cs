using HyperLiquid.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Models
{
    public record HyperLiquidOpenOrder
    {
        [JsonIgnore]
        public string? Symbol { get; set; }
        [JsonIgnore]
        public SymbolType SymbolType { get; set; }
        [JsonPropertyName("coin")]
        public string ExchangeSymbol { get; set; }

        [JsonPropertyName("limitPx")]
        public decimal Price { get; set; }

        [JsonPropertyName("oid")]
        public long OrderId { get; set; }

        [JsonPropertyName("side")]
        public OrderSide OrderSide { get; set; }

        [JsonPropertyName("sz")]
        public decimal Quantity { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
