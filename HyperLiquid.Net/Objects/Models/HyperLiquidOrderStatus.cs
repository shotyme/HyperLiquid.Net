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
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("order")]
        public HyperLiquidOrderStatus? Order { get; set; }
    }

    /// <summary>
    /// Order status
    /// </summary>
    public record HyperLiquidOrderStatus
    {
        /// <summary>
        /// Status timestamp
        /// </summary>
        [JsonPropertyName("statusTimestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        [JsonPropertyName("status")]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// Order info
        /// </summary>
        [JsonPropertyName("order")]
        public HyperLiquidOrder Order { get; set; } = default!;

    }
}
