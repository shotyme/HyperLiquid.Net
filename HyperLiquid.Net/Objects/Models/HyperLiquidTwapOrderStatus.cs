using HyperLiquid.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Models
{
    /// <summary>
    /// TWAP order status
    /// </summary>
    public record HyperLiquidTwapOrderStatus
    {
        /// <summary>
        /// TWAP status
        /// </summary>
        [JsonPropertyName("state")]
        public HyperLiquidTwapStatus TwapInfo { get; set; } = default!;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Statu
        /// </summary>
        [JsonPropertyName("status")]
        public HyperLiquidTwapOrderStatusDesc Status { get; set; } = default!;
    }

    /// <summary>
    /// TWAP order status
    /// </summary>
    public record HyperLiquidTwapOrderStatusDesc
    {
        /// <summary>
        /// Order status
        /// </summary>
        [JsonPropertyName("status")]
        public TwapStatus Status { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}
