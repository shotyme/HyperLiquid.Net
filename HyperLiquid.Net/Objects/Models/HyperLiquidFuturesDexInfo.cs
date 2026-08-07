using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace HyperLiquid.Net.Objects.Models
{
    /// <summary>
    /// Perp dex
    /// </summary>
    [SerializationModel]
    public record HyperLiquidFuturesDexInfo
    {
        /// <summary>
        /// Symbols
        /// </summary>
        public HyperLiquidFuturesSymbol[] Symbols { get; set; } = [];
        /// <summary>
        /// Dex name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Index of the DEX (only filled for GetExchangeInfoAllDexesAsync)
        /// </summary>
        public int? Index { get; set; }
        /// <summary>
        /// Collateral token index
        /// </summary>
        public int CollateralTokenIndex { get; set; }
        /// <summary>
        /// Collateral token name
        /// </summary>
        public string? CollateralToken { get; set; }
    }
}
