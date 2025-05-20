using HyperLiquid.Net.Enums;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using HyperLiquid.Net.Converters;

namespace HyperLiquid.Net.Objects.Models
{
    internal record HyperLiquidOrderResultIntWrapper
    {
        [JsonPropertyName("statuses")]
        [JsonConverter(typeof(OrderResultConverter))]
        public HyperLiquidOrderResultInt[] Statuses { get; set; } = [];
    }

    internal record HyperLiquidOrderResultInt
    {
        [JsonPropertyName("resting")]
        public HyperLiquidOrderResult? ResultResting { get; set; }
        [JsonPropertyName("filled")]
        public HyperLiquidOrderResult? ResultFilled { get; set; }
        public HyperLiquidOrderResult? WaitingForTrigger { get; set; }
        public HyperLiquidOrderResult? WaitingForFill { get; set; }
        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }

    /// <summary>
    /// Order result
    /// </summary>
    public record HyperLiquidOrderResult
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("oid")]
        public long OrderId { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        public OrderStatus Status { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("totalSz")]
        public decimal? FilledQuantity { get; set; }
        /// <summary>
        /// Average fill price
        /// </summary>
        [JsonPropertyName("avgPx")]
        public decimal? AveragePrice { get; set; }
    }
}
