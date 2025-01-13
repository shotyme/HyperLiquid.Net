using HyperLiquid.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Models
{
    public record HyperLiquidOpenOrder
    {
        [JsonPropertyName("coin")]
        public string Asset { get; set; }

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
