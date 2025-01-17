using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Internal
{
    internal class HyperLiquidPing
    {
        [JsonPropertyName("method")]
        public string Method { get; set; } = "ping";
    }
}
