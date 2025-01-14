using HyperLiquid.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Models
{
    internal record HyperLiquidOrderResultInt
    {
        [JsonPropertyName("resting")]
        public HyperLiquidOrderResult? ResultResting { get; set; }
        [JsonPropertyName("filled")]
        public HyperLiquidOrderResult? ResultFilled { get; set; }
        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }

    public record HyperLiquidOrderResult
    {
        [JsonPropertyName("oid")]
        public long OrderId { get; set; }
        public OrderStatus Status { get; set; }
        [JsonPropertyName("totalSz")]
        public decimal? FilledQuantity { get; set; }
        [JsonPropertyName("avgPx")]
        public decimal? AveragePrice { get; set; }
    }
}
