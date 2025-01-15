using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Internal
{
    internal class HyperLiquidPong
    {
        [JsonPropertyName("channel")]
        public string Channel { get; set; } = string.Empty;
    }
}
