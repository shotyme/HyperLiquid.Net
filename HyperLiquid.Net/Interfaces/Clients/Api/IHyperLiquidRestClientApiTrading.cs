using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using CryptoExchange.Net.Objects;
using HyperLiquid.Net.Objects.Models;
using System;
using HyperLiquid.Net.Enums;

namespace HyperLiquid.Net.Interfaces.Clients.Api
{
    /// <summary>
    /// HyperLiquid  trading endpoints, placing and managing orders.
    /// </summary>
    public interface IHyperLiquidRestClientApiTrading
    {
        Task<WebCallResult<IEnumerable<HyperLiquidOpenOrder>>> GetOpenOrdersAsync(string address, CancellationToken ct = default);

        Task<WebCallResult<IEnumerable<HyperLiquidOrder>>> GetOpenOrdersExtendedAsync(string address, CancellationToken ct = default);

        Task<WebCallResult<IEnumerable<HyperLiquidUserTrade>>> GetUserTradesByTimeAsync(
            string address,
            DateTime startTime,
            DateTime? endTime = null,
            bool? aggregateByTime = null,
            CancellationToken ct = default);

        Task<WebCallResult<HyperLiquidOrderStatus>> GetOrderAsync(string address, long? orderId = null, string? clientOrderId = null, CancellationToken ct = default);

        Task<WebCallResult<IEnumerable<HyperLiquidOrderStatus>>> GetOrderHistoryAsync(string address, CancellationToken ct = default);

        Task<WebCallResult> CancelOrderAsync(SymbolType symbolType, string symbol, long orderId, CancellationToken ct = default);

        Task<WebCallResult<IEnumerable<CallResult>>> CancelOrdersAsync(IEnumerable<HyperLiquidCancelRequest> requests, CancellationToken ct = default);

        Task<WebCallResult> CancelOrderByClientOrderIdAsync(SymbolType symbolType, string symbol, string clientOrderId, CancellationToken ct = default);

        Task<WebCallResult<IEnumerable<CallResult>>> CancelOrdersByClientOrderIdAsync(IEnumerable<HyperLiquidCancelByClientOrderIdRequest> requests, CancellationToken ct = default);

        Task<WebCallResult<IEnumerable<CallResult<HyperLiquidOrderResult>>>> PlaceMultipleOrdersAsync(
            IEnumerable<HyperLiquidOrderRequest> orders,
            CancellationToken ct = default);

        Task<WebCallResult<HyperLiquidOrderResult>> PlaceOrderAsync(
            SymbolType symbolType,
            string symbol,
            OrderSide side,
            OrderType orderType,
            decimal quantity,
            decimal? price = null,
            TimeInForce? timeInForce = null,
            bool? reduceOnly = null,
            string? clientOrderId = null,
            decimal? triggerPrice = null,
            TpSlType? tpSlType = null,
            TpSlGrouping? tpSlGrouping = null
            );

        Task<WebCallResult<IEnumerable<HyperLiquidOrderStatus>>> CancelAfterAsync(TimeSpan? timeout, CancellationToken ct = default);
    }
}
