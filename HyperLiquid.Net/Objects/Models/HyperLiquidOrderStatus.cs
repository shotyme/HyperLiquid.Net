using HyperLiquid.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Models
{
    internal record HyperLiquidOrderStatusResult
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("order")]
        public HyperLiquidOrderStatus? Order { get; set; }
    }

    public record HyperLiquidOrderStatus
    {
        [JsonPropertyName("statusTimestamp")]
        public DateTime Timestamp { get; set; }
        [JsonPropertyName("status")]
        public OrderStatus Status { get; set; }
        [JsonPropertyName("order")]
        public HyperLiquidOrder Order { get; set; }

    }
}
