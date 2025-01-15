using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using HyperLiquid.Net.Interfaces;
using HyperLiquid.Net.Interfaces.Clients;
using HyperLiquid.Net.Objects.Options;
using HyperLiquid.Net.Enums;

namespace HyperLiquid.Net.SymbolOrderBooks
{
    /// <summary>
    /// HyperLiquid order book factory
    /// </summary>
    public class HyperLiquidOrderBookFactory : IHyperLiquidOrderBookFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public HyperLiquidOrderBookFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            Spot = new OrderBookFactory<HyperLiquidOrderBookOptions>((symbol, opts) => Create(SymbolType.Spot, symbol, opts), Create);
            Futures = new OrderBookFactory<HyperLiquidOrderBookOptions>((symbol, opts) => Create(SymbolType.Futures, symbol, opts), Create);
        }

        
         /// <inheritdoc />
        public IOrderBookFactory<HyperLiquidOrderBookOptions> Spot { get; }
        /// <inheritdoc />
        public IOrderBookFactory<HyperLiquidOrderBookOptions> Futures { get; }


        /// <inheritdoc />
        public ISymbolOrderBook Create(SharedSymbol symbol, Action<HyperLiquidOrderBookOptions>? options = null)
        {
            var symbolName = HyperLiquidExchange.FormatSymbol(symbol.BaseAsset, symbol.QuoteAsset, symbol.TradingMode, symbol.DeliverTime);
            return Create(symbol.TradingMode == TradingMode.Spot ? SymbolType.Spot : SymbolType.Futures, symbolName, options);
        }

        
         /// <inheritdoc />
        public ISymbolOrderBook Create(SymbolType symbolType, string symbol, Action<HyperLiquidOrderBookOptions>? options = null)
            => new HyperLiquidSymbolOrderBook(symbolType, symbol, options, 
                                                          _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                                          _serviceProvider.GetRequiredService<IHyperLiquidSocketClient>());


    }
}
