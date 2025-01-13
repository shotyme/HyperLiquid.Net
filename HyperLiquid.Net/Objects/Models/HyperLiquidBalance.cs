using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Models
{
    internal class HyperLiquidBalances
    {
        [JsonPropertyName("balances")]
        public IEnumerable<HyperLiquidBalance> Balances { get; set; }
    }

    public record HyperLiquidBalance
    {
        [JsonPropertyName("coin")]
        public string Asset { get; set; }
        [JsonPropertyName("token")]
        public int Token { get; set; }
        [JsonPropertyName("hold")]
        public decimal Hold { get; set; }
        [JsonPropertyName("total")]
        public decimal Total { get; set; }
        [JsonPropertyName("entryNtl")]
        public decimal EntryNotional { get; set; }
    }
}
