using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Models
{
    internal record HyperLiquidTwapOrderResultIntWrapper
    {
        [JsonPropertyName("status")]
        public HyperLiquidTwapOrderResultInt Status { get; set; } = default!;
    }

    internal record HyperLiquidTwapOrderResultInt
    {
        [JsonPropertyName("running")]
        public HyperLiquidTwapOrderResult? ResultRunning { get; set; }
        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }

    /// <summary>
    /// Order result
    /// </summary>
    public record HyperLiquidTwapOrderResult
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("twapId")]
        public long TwapId { get; set; }
    }
}
