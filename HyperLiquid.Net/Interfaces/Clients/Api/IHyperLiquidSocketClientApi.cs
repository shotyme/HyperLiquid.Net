using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects.Sockets;
using HyperLiquid.Net.Objects.Models;
using System.Collections.Generic;
using HyperLiquid.Net.Enums;

namespace HyperLiquid.Net.Interfaces.Clients.Api
{
    /// <summary>
    /// HyperLiquid  streams
    /// </summary>
    public interface IHyperLiquidSocketClientApi : ISocketApiClient, IDisposable
    {
        /// <summary>
        /// Subscribe to mid price updates
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/websocket/subscriptions" /></para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToPriceUpdatesAsync(Action<DataEvent<Dictionary<string, decimal>>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Interval of the klines/candles
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/websocket/subscriptions" /></para>
        /// </summary>
        /// <param name="symbolType">Symbol type</param>
        /// <param name="symbol">Symbol name, for example "HYPE/USDT"</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="onMessage">The data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected and to unsubscribe</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(SymbolType symbolType, string symbol, KlineInterval interval, Action<DataEvent<HyperLiquidKline>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book updates
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/websocket/subscriptions" /></para>
        /// </summary>
        /// <param name="symbolType">Symbol type</param>
        /// <param name="symbol">Symbol name, for example "HYPE/USDT"</param>
        /// <param name="onMessage">The data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected and to unsubscribe</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(SymbolType symbolType, string symbol, Action<DataEvent<HyperLiquidOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to trade updates
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/websocket/subscriptions" /></para>
        /// </summary>
        /// <param name="symbolType">Symbol type</param>
        /// <param name="symbol">Symbol name, for example `HYPE/USDT`</param>
        /// <param name="onMessage">The data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected and to unsubscribe</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(SymbolType symbolType, string symbol, Action<DataEvent<IEnumerable<HyperLiquidTrade>>> onMessage, CancellationToken ct = default);

        Task<CallResult<UpdateSubscription>> SubscribeToNotificationUpdatesAsync(string? address, Action<DataEvent<IEnumerable<HyperLiquidTrade>>> onMessage, CancellationToken ct = default);

        Task<CallResult<UpdateSubscription>> SubscribeToUserUpdatesAsync(string? address, Action<DataEvent<IEnumerable<HyperLiquidTrade>>> onMessage, CancellationToken ct = default);

        Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(string? address, Action<DataEvent<IEnumerable<HyperLiquidOrderStatus>>> onMessage, CancellationToken ct = default);

        Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(string? address, Action<DataEvent<IEnumerable<HyperLiquidUserTrade>>> onMessage, CancellationToken ct = default);

        Task<CallResult<UpdateSubscription>> SubscribeToUserFundingUpdatesAsync(string? address, Action<DataEvent<IEnumerable<HyperLiquidUserFunding>>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Get the shared socket requests client. This interface is shared with other exhanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IHyperLiquidSocketClientApiShared SharedClient { get; }
    }
}
