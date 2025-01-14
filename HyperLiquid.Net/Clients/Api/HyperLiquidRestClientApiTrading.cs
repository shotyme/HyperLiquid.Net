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
            return await _baseClient.SendAsync<IEnumerable<HyperLiquidOpenOrder>>(request, parameters, ct).ConfigureAwait(false);
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
            return await _baseClient.SendAsync<IEnumerable<HyperLiquidOrder>>(request, parameters, ct).ConfigureAwait(false);
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

        #region Get Closed Orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HyperLiquidOrderStatus>>> GetClosedOrdersAsync(string address, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "type", "historicalOrders" },
                { "user", address }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, "info", HyperLiquidExchange.RateLimiter.HyperLiquid, 1, false);
            return await _baseClient.SendAsync<IEnumerable<HyperLiquidOrderStatus>>(request, parameters, ct).ConfigureAwait(false);
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
                var symbolId = await HyperLiquidUtils.GetSymbolIdFromName(order.SymbolType, order.Symbol).ConfigureAwait(false);
                if (!symbolId)
                    return new WebCallResult<IEnumerable<CallResult<HyperLiquidOrderResult>>>(symbolId.Error);

                var orderParameters = new ParameterCollection();
                orderParameters.Add("a", symbolId.Data);
                orderParameters.AddString("s", order.Quantity);

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

                orderParameters.AddOptional("b", order.Side == OrderSide.Buy);
                orderParameters.AddOptionalString("p", order.Price);
                orderParameters.AddOptional("r", order.ReduceOnly);
                orderParameters.AddOptional("c", order.ClientOrderId);
#warning TODO builder
                if (order.TpSlGrouping != null)
                    orderParameters.AddEnum("grouping", order.TpSlGrouping);
                else
                    orderParameters.Add("grouping", "na");

                orderRequests.Add(orderParameters);
            }

            var parameters = new ParameterCollection()
            {
                {
                    "action", new ParameterCollection
                    {
                        { "type", "order" },
                        { "orders", orderRequests }
                    }
                }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquid, 1, true);
            var intResult = await _baseClient.SendAuthAsync<IEnumerable<HyperLiquidOrderResultInt>>(request, parameters, ct).ConfigureAwait(false);
            if(!intResult)
                return intResult.As<IEnumerable<CallResult<HyperLiquidOrderResult>>>(default);

            var result = new List<CallResult<HyperLiquidOrderResult>>();
            foreach (var order in intResult.Data)
            {
                if (order.Error != null)
                    result.Add(new CallResult<HyperLiquidOrderResult>(new ServerError(order.Error)));
                else
                    result.Add(new CallResult<HyperLiquidOrderResult>(order.ResultResting ?? order.ResultFilled!));                
            }

            return intResult.As<IEnumerable<CallResult<HyperLiquidOrderResult>>>(result);
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<WebCallResult> CancelOrderAsync(SymbolType symbolType, string symbol, long orderId, CancellationToken ct = default)
        {
            var symbolId = await HyperLiquidUtils.GetSymbolIdFromName(symbolType, symbol).ConfigureAwait(false);
            if (!symbolId)
                return new WebCallResult(symbolId.Error!);

            var parameters = new ParameterCollection()
            {
                { 
                    "action", new ParameterCollection
                    {
                        { "type", "cancel" },
                        { "cancels",
                            new []  
                            {
                                new ParameterCollection
                                {
                                    { "a", symbolId.Data },
                                    { "o", orderId }
                                }
                            }
                        }
                    }
                }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquid, 1, true);
            var result = await _baseClient.SendAuthAsync<HyperLiquidOrderStatusResult>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result.AsDatalessError(result.Error!);

#warning check responses

            return result.AsDataless();
        }

        #endregion
    }
}
