using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Internal
{
    internal class HyperLiquidMidsUpdate
    {
        [JsonPropertyName("mids")]
        public Dictionary<string, decimal> Mids { get; set; } = new Dictionary<string, decimal>();
    }
}
