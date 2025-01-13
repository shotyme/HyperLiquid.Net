using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Models
{
    public record HyperLiquidRateLimit
    {
        [JsonPropertyName("cumVlm")]
        public decimal TotalVolume { get; set; }
        [JsonPropertyName("nRequestsUsed")]
        public long RequestsUsed { get; set; }
        [JsonPropertyName("nRequestsCap")]
        public long RequestsCap { get; set; }
    }
}
