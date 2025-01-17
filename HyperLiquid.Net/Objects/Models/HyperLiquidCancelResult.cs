using HyperLiquid.Net.Converters;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Models
{
    /// <summary>
    /// Cancel result
    /// </summary>
    internal record HyperLiquidCancelResult
    {
        [JsonPropertyName("statuses")]
        [JsonConverter(typeof(CancelResultConverter))]
        public IEnumerable<string> Statuses { get; set; } = [];
    }
}
