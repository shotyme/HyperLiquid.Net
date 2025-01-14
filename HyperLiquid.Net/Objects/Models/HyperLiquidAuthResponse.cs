using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Models
{
    internal class HyperLiquidResponse<T>
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("response")]
        public HyperLiquidAuthResponse<T> Data { get; set; }
    }

    internal class HyperLiquidAuthResponse<T>
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}
