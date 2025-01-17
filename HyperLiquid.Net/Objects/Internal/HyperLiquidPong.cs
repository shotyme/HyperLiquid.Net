using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Internal
{
    internal class HyperLiquidPong
    {
        [JsonPropertyName("channel")]
        public string Channel { get; set; } = string.Empty;
    }
}
