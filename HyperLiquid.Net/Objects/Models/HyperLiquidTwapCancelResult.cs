using HyperLiquid.Net.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Models
{
    internal class HyperLiquidTwapCancelResult
    {
        [JsonPropertyName("status")]
        [JsonConverter(typeof(TwapCancelResultConverter))]
        public string Status { get; set; } = string.Empty;
    }
}