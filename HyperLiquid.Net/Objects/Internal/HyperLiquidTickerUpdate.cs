using HyperLiquid.Net.Objects.Models;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Internal
{
    internal class HyperLiquidTickerUpdate
    {
        [JsonPropertyName("coin")]
        public string Symbol { get; set; } = string.Empty;
        [JsonPropertyName("ctx")]
        public HyperLiquidTicker Ticker { get; set; } = default!;
    }
}
