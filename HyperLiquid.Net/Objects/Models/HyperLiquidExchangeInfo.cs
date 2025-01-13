using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Models
{
    public record HyperLiquidExchangeInfo
    {
        [JsonPropertyName("tokens")]
        public IEnumerable<HyperLiquidAsset> Assets { get; set; }
        [JsonInclude, JsonPropertyName("universe")]
        private IEnumerable<HyperLiquidSymbolReference> SymbolsInt { get; set; }

        private IEnumerable<HyperLiquidSymbol> _symbols;
        [JsonIgnore]
        public IEnumerable<HyperLiquidSymbol> Symbols
        {
            get
            {
                if (_symbols == null)
                {
                    _symbols = SymbolsInt.Select(x =>                    
                        new HyperLiquidSymbol
                        {
                            Index = x.Index,
                            IsCanonical = x.IsCanonical,
                            Name = x.Name,
                            BaseAsset = Assets.ElementAt(x.BaseAssetIndex),
                            QuoteAsset = Assets.ElementAt(x.QuoteAssetIndex),
                        }
                    ).ToList();
                }

                return _symbols;
            }
        }
    }

    public record HyperLiquidSymbol
    {
        public string Name { get; set; }
        public HyperLiquidAsset BaseAsset { get; set; }
        public HyperLiquidAsset QuoteAsset { get; set; }
        public int Index { get; set; }
        public bool IsCanonical { get; set; }
    }

    public record HyperLiquidAsset
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("szDecimals")]
        public int QuantityDecimals { get; set; }
        [JsonPropertyName("weiDecimals")]
        public int PriceDecimals { get; set; }
        [JsonPropertyName("index")]
        public int Index { get; set; }
        [JsonPropertyName("tokenId")]
        public string AssetId { get; set; }
        [JsonPropertyName("isCanonical")]
        public bool IsCanonical { get; set; }
        [JsonPropertyName("evmContract")]
        public string EvmContract { get; set; }
        [JsonPropertyName("fullName")]
        public string FullName { get; set; }
    }

    public record HyperLiquidSymbolReference
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonInclude, JsonPropertyName("tokens")]
        private int[] Assets { get; set; }
        [JsonIgnore]
        public int BaseAssetIndex => Assets[0];
        [JsonIgnore]
        public int QuoteAssetIndex => Assets[1];

        [JsonPropertyName("index")]
        public int Index { get; set; }
        [JsonPropertyName("isCanonical")]
        public bool IsCanonical { get; set; }
    }
}
