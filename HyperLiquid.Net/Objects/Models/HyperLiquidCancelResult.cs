using HyperLiquid.Net.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Models
{
    public record HyperLiquidCancelResult
    {
        [JsonPropertyName("statuses")]
        [JsonConverter(typeof(CancelResultConverter))]
        public IEnumerable<string> Statuses { get; set; }
    }
}
