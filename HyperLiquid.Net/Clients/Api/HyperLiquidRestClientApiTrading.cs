using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using HyperLiquid.Net.Interfaces.Clients.Api;
using HyperLiquid.Net.Objects.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System;
using HyperLiquid.Net.Utils;
using HyperLiquid.Net.Enums;
using System.Linq;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace HyperLiquid.Net.Clients.Api
{
    /// <inheritdoc />
    internal class HyperLiquidRestClientApiTrading : IHyperLiquidRestClientApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly HyperLiquidRestClientApi _baseClient;
        private readonly ILogger _logger;

        internal HyperLiquidRestClientApiTrading(ILogger logger, HyperLiquidRestClientApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HyperLiquidOpenOrder>>> GetOpenOrdersAsync(string address, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "type", "openOrders" },
                { "user", address }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Post, "info", HyperLiquidExchange.RateLimiter.HyperLiquid, 1, false);
            var result = await _baseClient.SendAsync<IEnumerable<HyperLiquidOpenOrder>>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result;

            foreach (var order in result.Data) 
            {
                if (order.ExchangeSymbol.StartsWith("@"))
                {
                    var symbolName = await HyperLiquidUtils.GetSymbolNameFromExchangeNameAsync(order.ExchangeSymbol).ConfigureAwait(false);
                    if (symbolName == null)
                        continue;

                    order.Symbol = symbolName.Data;
                    order.SymbolType = SymbolType.Spot;
                }
                else
                {
                    order.Symbol = order.ExchangeSymbol;
                    order.SymbolType = SymbolType.Futures;
                }
            }

            return result;
        }

        #endregion

        #region Get Open Orders Extended

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HyperLiquidOrder>>> GetOpenOrdersExtendedAsync(string address, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "type", "frontendOpenOrders" },
                { "user", address }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Post, "info", HyperLiquidExchange.RateLimiter.HyperLiquid, 1, false);
            var result = await _baseClient.SendAsync<IEnumerable<HyperLiquidOrder>>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result;

            foreach (var order in result.Data)
            {
                if (order.ExchangeSymbol.StartsWith("@"))
                {
                    var symbolName = await HyperLiquidUtils.GetSymbolNameFromExchangeNameAsync(order.ExchangeSymbol).ConfigureAwait(false);
                    if (symbolName == null)
                        continue;

                    order.Symbol = symbolName.Data;
                    order.SymbolType = SymbolType.Spot;
                }
                else
                {
                    order.Symbol = order.ExchangeSymbol;
                    order.SymbolType = SymbolType.Futures;
                }
            }

            return result;
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HyperLiquidUserTrade>>> GetUserTradesAsync(string address, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "type", "userFills" },
                { "user", address }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Post, "info", HyperLiquidExchange.RateLimiter.HyperLiquid, 1, false);
            return await _baseClient.SendAsync<IEnumerable<HyperLiquidUserTrade>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get User Trades By Time

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HyperLiquidUserTrade>>> GetUserTradesByTimeAsync(
            string address,
            DateTime startTime,
            DateTime? endTime = null,
            bool? aggregateByTime = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "type", "userFillsByTime" },
                { "user", address }
            };
            parameters.AddMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("aggregateByTime", aggregateByTime);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "info", HyperLiquidExchange.RateLimiter.HyperLiquid, 1, false);
            return await _baseClient.SendAsync<IEnumerable<HyperLiquidUserTrade>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order

        /// <inheritdoc />
        public async Task<WebCallResult<HyperLiquidOrderStatus>> GetOrderAsync(string address, long? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "type", "orderStatus" },
                { "user", address }
            };

            parameters.AddOptional("oid", orderId);
            parameters.AddOptional("oid", clientOrderId);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "info", HyperLiquidExchange.RateLimiter.HyperLiquid, 1, false);
            var result = await _baseClient.SendAsync<HyperLiquidOrderStatusResult>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result.As<HyperLiquidOrderStatus>(default);

            if (result.Data.Status != "order")
                return result.AsError<HyperLiquidOrderStatus>(new ServerError(result.Data.Status));

            return result.As<HyperLiquidOrderStatus>(result.Data.Order);
        }

        #endregion

        #region Get Order History

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HyperLiquidOrderStatus>>> GetOrderHistoryAsync(string address, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "type", "historicalOrders" },
                { "user", address }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, "info", HyperLiquidExchange.RateLimiter.HyperLiquid, 1, false);
            var result = await _baseClient.SendAsync<IEnumerable<HyperLiquidOrderStatus>>(request, parameters, ct).ConfigureAwait(false);

            if (!result)
                return result;

            foreach (var order in result.Data)
            {
                if (order.Order.ExchangeSymbol.StartsWith("@"))
                {
                    var symbolName = await HyperLiquidUtils.GetSymbolNameFromExchangeNameAsync(order.Order.ExchangeSymbol).ConfigureAwait(false);
                    if (symbolName == null)
                        continue;

                    order.Order.Symbol = symbolName.Data;
                    order.Order.SymbolType = SymbolType.Spot;
                }
                else
                {
                    order.Order.Symbol = order.Order.ExchangeSymbol;
                    order.Order.SymbolType = SymbolType.Futures;
                }
            }

            return result;
        }

        #endregion

        #region Place Order

        public async Task<WebCallResult<HyperLiquidOrderResult>> PlaceOrderAsync(
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
            )
        {
            var result = await PlaceMultipleOrdersAsync([
                new HyperLiquidOrderRequest(symbolType, symbol, side, orderType, quantity, price, timeInForce, reduceOnly, triggerPrice, tpSlType, clientOrderId)
                ]).ConfigureAwait(false);

            if (!result)
                return result.As<HyperLiquidOrderResult>(default);

            var orderResult = result.Data.Single();
            if (!orderResult)
                return result.AsError<HyperLiquidOrderResult>(orderResult.Error!);

            return result.As(result.Data.Single().Data);
        }

        #endregion

        #region Place Multiple Orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<CallResult<HyperLiquidOrderResult>>>> PlaceMultipleOrdersAsync(
            IEnumerable<HyperLiquidOrderRequest> orders,
            CancellationToken ct = default)
        {
            var orderRequests = new List<ParameterCollection>();
            foreach(var order in orders)
            {
                var symbolId = await HyperLiquidUtils.GetSymbolIdFromNameAsync(order.SymbolType, order.Symbol).ConfigureAwait(false);
                if (!symbolId)
                    return new WebCallResult<IEnumerable<CallResult<HyperLiquidOrderResult>>>(symbolId.Error);

                var orderParameters = new ParameterCollection();
                orderParameters.Add("a", symbolId.Data);
                orderParameters.AddOrNull("b", order.Side == OrderSide.Buy);
                orderParameters.AddStringOrNull("p", order.Price);
                orderParameters.AddString("s", order.Quantity);
                orderParameters.Add("r", order.ReduceOnly ?? false);

                var orderTypeParameters = new ParameterCollection();
                if (order.OrderType == OrderType.Limit)
                {
                    var limitParameters = new ParameterCollection();
                    limitParameters.AddEnum("tif", order.TimeInForce);
                    orderTypeParameters.Add("limit", limitParameters);
                }
                else if(order.OrderType == OrderType.StopMarket || order.OrderType == OrderType.StopLimit)
                {
                    var triggerParameters = new ParameterCollection();
                    triggerParameters.Add("isMarket", order.TimeInForce);
                    triggerParameters.Add("triggerPx", order.TriggerPrice);
                    triggerParameters.AddEnum("tpsl", order.TpSlType);
                    orderTypeParameters.Add("trigger", triggerParameters);
                }

                orderParameters.Add("t", orderTypeParameters);

                orderParameters.AddOptional("c", order.ClientOrderId);

                orderRequests.Add(orderParameters);
            }

//#warning TODO builder
//            if (order.TpSlGrouping != null)
//                orderParameters.AddEnum("grouping", order.TpSlGrouping);
//            else
//                orderParameters.Add("grouping", "na");

            var parameters = new ParameterCollection()
            {
                {
                    "action", new ParameterCollection
                    {
                        { "type", "order" },
                        { "orders", orderRequests },
                        { "grouping", "na" }
                    }
                }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquid, 1, true);
            var intResult = await _baseClient.SendAuthAsync<HyperLiquidOrderResultIntWrapper>(request, parameters, ct).ConfigureAwait(false);
            if(!intResult)
                return intResult.As<IEnumerable<CallResult<HyperLiquidOrderResult>>>(default);

            var result = new List<CallResult<HyperLiquidOrderResult>>();
            foreach (var order in intResult.Data.Statuses)
            {
                if (order.Error != null)
                    result.Add(new CallResult<HyperLiquidOrderResult>(new ServerError(order.Error)));
                else if (order.ResultResting != null)
                    result.Add(new CallResult<HyperLiquidOrderResult>(order.ResultResting with { Status = OrderStatus.Open }));                
                else
                    result.Add(new CallResult<HyperLiquidOrderResult>(order.ResultFilled! with { Status = OrderStatus.Filled }));
            }

            return intResult.As<IEnumerable<CallResult<HyperLiquidOrderResult>>>(result);
        }

        #endregion

        #region Cancel Order

        public async Task<WebCallResult> CancelOrderAsync(SymbolType symbolType, string symbol, long orderId, CancellationToken ct = default)
        {
            var result = await CancelOrdersAsync([new HyperLiquidCancelRequest(symbolType, symbol, orderId)], ct).ConfigureAwait(false);
            if (!result)
                return result.AsDataless();

            var cancelResult = result.Data.Single();
            if (!cancelResult)
                return result.AsDatalessError(cancelResult.Error!);

            return result.AsDataless();
        }

        #endregion

        #region Cancel Orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<CallResult>>> CancelOrdersAsync(IEnumerable<HyperLiquidCancelRequest> requests, CancellationToken ct = default)
        {
            var orderRequests = new List<ParameterCollection>();
            foreach (var order in requests)
            {
                var symbolId = await HyperLiquidUtils.GetSymbolIdFromNameAsync(order.SymbolType, order.Symbol).ConfigureAwait(false);
                if (!symbolId)
                    return new WebCallResult<IEnumerable<CallResult>>(symbolId.Error);

                orderRequests.Add(new ParameterCollection
                    {
                        { "a", symbolId.Data },
                        { "o", order.OrderId }
                    }
                );
            }

            var parameters = new ParameterCollection()
            {
                { 
                    "action", new ParameterCollection
                    {
                        { "type", "cancel" },
                        { "cancels", orderRequests
                        }
                    }
                }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquid, 1, true);
            var resultInt = await _baseClient.SendAuthAsync<HyperLiquidCancelResult>(request, parameters, ct).ConfigureAwait(false);
            if (!resultInt)
                return resultInt.AsError<IEnumerable<CallResult>>(resultInt.Error!);

            var result = new List<CallResult>();
            foreach (var order in resultInt.Data.Statuses)
            {
                if (order.Equals("success"))
                    result.Add(new CallResult(null));
                else
                    result.Add(new CallResult(new ServerError(order)));
            }

            return resultInt.As<IEnumerable<CallResult>>(result);
        }

        #endregion


        #region Cancel Order By Client Order Id

        public async Task<WebCallResult> CancelOrderByClientOrderIdAsync(SymbolType symbolType, string symbol, string clientOrderId, CancellationToken ct = default)
        {
            var result = await CancelOrdersByClientOrderIdAsync([new HyperLiquidCancelByClientOrderIdRequest(symbolType, symbol, clientOrderId)], ct).ConfigureAwait(false);
            if (!result)
                return result.AsDataless();

            var cancelResult = result.Data.Single();
            if (!cancelResult)
                return result.AsDatalessError(cancelResult.Error!);

            return result.AsDataless();
        }

        #endregion

        #region Cancel Orders By Client Order Id

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<CallResult>>> CancelOrdersByClientOrderIdAsync(IEnumerable<HyperLiquidCancelByClientOrderIdRequest> requests, CancellationToken ct = default)
        {
            var orderRequests = new List<ParameterCollection>();
            foreach (var order in requests)
            {
                var symbolId = await HyperLiquidUtils.GetSymbolIdFromNameAsync(order.SymbolType, order.Symbol).ConfigureAwait(false);
                if (!symbolId)
                    return new WebCallResult<IEnumerable<CallResult>>(symbolId.Error);

                orderRequests.Add(new ParameterCollection
                    {
                        { "asset", symbolId.Data },
                        { "cloid", order.OrderId }
                    }
                );
            }

            var parameters = new ParameterCollection()
            {
                {
                    "action", new ParameterCollection
                    {
                        { "type", "cancelByCloid" },
                        { "cancels", orderRequests
                        }
                    }
                }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquid, 1, true);
            var resultInt = await _baseClient.SendAuthAsync<HyperLiquidCancelResult>(request, parameters, ct).ConfigureAwait(false);
            if (!resultInt)
                return resultInt.AsError<IEnumerable<CallResult>>(resultInt.Error!);

            var result = new List<CallResult>();
            foreach (var order in resultInt.Data.Statuses)
            {
                if (order.Equals("success"))
                    result.Add(new CallResult(null));
                else
                    result.Add(new CallResult(new ServerError(order)));
            }

            return resultInt.As<IEnumerable<CallResult>>(result);
        }

        #endregion

        #region Cancel after

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HyperLiquidOrderStatus>>> CancelAfterAsync(TimeSpan? timeout, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var actionParameters = new ParameterCollection()
            {
                { "type", "scheduleCancel" }
            };
            actionParameters.AddOptionalMilliseconds("time", timeout == null ? null : DateTime.UtcNow + timeout);
            parameters.Add("action", actionParameters);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquid, 1, true);
            var result = await _baseClient.SendAsync<IEnumerable<HyperLiquidOrderStatus>>(request, parameters, ct).ConfigureAwait(false);


            return result;
        }

        #endregion

    }
}
