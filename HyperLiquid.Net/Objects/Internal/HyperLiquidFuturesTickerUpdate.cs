using HyperLiquid.Net.Objects.Models;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Internal
{
    internal class HyperLiquidFuturesTickerUpdate
    {
        [JsonPropertyName("coin")]
        public string Symbol { get; set; } = string.Empty;
        [JsonPropertyName("ctx")]
        public HyperLiquidFuturesTicker Ticker { get; set; } = default!;
    }
}
