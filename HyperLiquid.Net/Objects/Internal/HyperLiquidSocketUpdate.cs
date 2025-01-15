using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Internal
{
    internal class HyperLiquidSocketUpdate<T>
    {
        [JsonPropertyName("channel")]
        public string Channel { get; set; }
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}
