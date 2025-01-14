using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Converters
{
    internal class CancelResultConverter : JsonConverter<IEnumerable<string>>
    {
        public override IEnumerable<string>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
#warning check, seems weird

            var resultList = new List<string>();
            reader.Read(); // Read start array
            while (reader.TokenType == JsonTokenType.StartObject || reader.TokenType == JsonTokenType.String)
            {
                if (reader.TokenType == JsonTokenType.String)
                {
                    resultList.Add(JsonSerializer.Deserialize<string>(ref reader, options));
                    reader.Read();
                    continue;
                }

                var result = JsonSerializer.Deserialize<ErrorMessage>(ref reader, options);
                resultList.Add(result.Error);
                reader.Read(); // Read end array
            }

            return resultList;
        }

        public override void Write(Utf8JsonWriter writer, IEnumerable<string> value, JsonSerializerOptions options)
        {

        }

        class ErrorMessage
        {
            [JsonPropertyName("error")]
            public string Error { get; set; }
        }
    }
}
