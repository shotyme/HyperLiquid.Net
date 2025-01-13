using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Models
{
    public record HyperLiquidKline
    {
        [JsonPropertyName("t")]
        public DateTime OpenTime { get; set; }
        [JsonPropertyName("T")]
        public DateTime CloseTime { get; set; }
        [JsonPropertyName("c")]
        public decimal ClosePrice { get; set; }

        [JsonPropertyName("h")]
        public decimal HighPrice { get; set; }
        [JsonPropertyName("l")]
        public decimal LowPrice { get; set; }
        [JsonPropertyName("o")]
        public decimal OpenPrice { get; set; }
        [JsonPropertyName("v")]
        public decimal Volume { get; set; }
        [JsonPropertyName("n")]
        public int TradeCount { get; set; }
        [JsonPropertyName("s")]
        public string Asset { get; set; }
    }
}