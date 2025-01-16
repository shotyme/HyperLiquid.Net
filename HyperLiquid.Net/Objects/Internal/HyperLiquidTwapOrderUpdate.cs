using HyperLiquid.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Internal
{
    internal record HyperLiquidTwapOrderUpdate
    {
        [JsonPropertyName("history")]
        public IEnumerable<HyperLiquidTwapOrderStatus> History { get; set; } = [];
        [JsonPropertyName("user")]
        public string User { get; set; } = string.Empty;
        [JsonPropertyName("isSnapshot")]
        public bool IsSnapshot { get; set; }
    }
}
