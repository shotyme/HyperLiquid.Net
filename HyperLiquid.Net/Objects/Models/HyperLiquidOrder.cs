using HyperLiquid.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Models
{
    public record HyperLiquidOrder
    {
        [JsonIgnore]
        public string? Symbol { get; set; }
        [JsonIgnore]
        public SymbolType SymbolType { get; set; }
        [JsonPropertyName("coin")]
        public string ExchangeSymbol { get; set; }

        [JsonPropertyName("isPositionTpsl")]
        public bool IsPositionTpSl { get; set; }

        [JsonPropertyName("isTrigger")]
        public bool IsTrigger { get; set; }

        [JsonPropertyName("limitPx")]
        public decimal Price { get; set; }

        [JsonPropertyName("oid")]
        public long OrderId { get; set; }

        [JsonPropertyName("side")]
        public OrderSide OrderSide { get; set; }

        [JsonPropertyName("type")]
        public OrderType OrderType { get; set; }

        [JsonPropertyName("origSz")]
        public decimal OriginalQuantity { get; set; }

        [JsonPropertyName("reduceOnly")]
        public bool ReduceOnly { get; set; }

        [JsonPropertyName("sz")]
        public decimal Quantity { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("triggerCondition")]
        public TriggerCondition TriggerCondition { get; set; }

        [JsonPropertyName("triggerPx")]
        public decimal? TriggerPrice { get; set; }
    }
}
