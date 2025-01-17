using HyperLiquid.Net.Objects.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Internal
{
    internal record HyperLiquidTwapTradeUpdate
    {
        [JsonPropertyName("twapSliceFills")]
        public IEnumerable<HyperLiquidTwapStatus> Trades { get; set; } = [];
        [JsonPropertyName("user")]
        public string User { get; set; } = string.Empty;
        [JsonPropertyName("isSnapshot")]
        public bool IsSnapshot { get; set; }
    }
}
