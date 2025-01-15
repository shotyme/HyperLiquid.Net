using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using System;
using HyperLiquid.Net.Objects.Options;
using HyperLiquid.Net.Enums;

namespace HyperLiquid.Net.Interfaces
{
    /// <summary>
    /// HyperLiquid local order book factory
    /// </summary>
    public interface IHyperLiquidOrderBookFactory
    {
        /// <summary>
        ///  order book Spot factory methods
        /// </summary>
        IOrderBookFactory<HyperLiquidOrderBookOptions> Spot { get; }
        /// <summary>
        ///  order book Futures factory methods
        /// </summary>
        IOrderBookFactory<HyperLiquidOrderBookOptions> Futures { get; }

        /// <summary>
        /// Create a SymbolOrderBook for the symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="options">Book options</param>
        /// <returns></returns>
        ISymbolOrderBook Create(SharedSymbol symbol, Action<HyperLiquidOrderBookOptions>? options = null);
        
        /// <summary>
        /// Create a new  local order book instance
        /// </summary>
        ISymbolOrderBook Create(SymbolType symbolType, string symbol, Action<HyperLiquidOrderBookOptions>? options = null);
    }
}