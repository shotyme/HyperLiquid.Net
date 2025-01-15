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
using System.Globalization;

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
        public async Task<WebCallResult<IEnumerable<HyperLiquidOpenOrder>>> GetOpenOrdersAsync(string? address = null, CancellationToken ct = default)
        {
            if (address == null && _baseClient.AuthenticationProvider == null)
                throw new ArgumentNullException(nameof(address), "Address needs to be provided if API credentials not set");

            var parameters = new ParameterCollection()
            {
                { "type", "openOrders" },
                { "user", address ?? _baseClient.AuthenticationProvider!.ApiKey  }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Post, "info", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 20, false);
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
        public async Task<WebCallResult<IEnumerable<HyperLiquidOrder>>> GetOpenOrdersExtendedAsync(string? address = null, CancellationToken ct = default)
        {
            if (address == null && _baseClient.AuthenticationProvider == null)
                throw new ArgumentNullException(nameof(address), "Address needs to be provided if API credentials not set");

            var parameters = new ParameterCollection()
            {
                { "type", "frontendOpenOrders" },
                { "user", address ?? _baseClient.AuthenticationProvider!.ApiKey }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Post, "info", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 20, false);
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
        public async Task<WebCallResult<IEnumerable<HyperLiquidUserTrade>>> GetUserTradesAsync(string? address = null, CancellationToken ct = default)
        {
            if (address == null && _baseClient.AuthenticationProvider == null)
                throw new ArgumentNullException(nameof(address), "Address needs to be provided if API credentials not set");

            var parameters = new ParameterCollection()
            {
                { "type", "userFills" },
                { "user", address ?? _baseClient.AuthenticationProvider!.ApiKey }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Post, "info", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 20, false);
            return await _baseClient.SendAsync<IEnumerable<HyperLiquidUserTrade>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get User Trades By Time

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HyperLiquidUserTrade>>> GetUserTradesByTimeAsync(
            DateTime startTime,
            DateTime? endTime = null,
            bool? aggregateByTime = null,
            string? address = null,
            CancellationToken ct = default)
        {
            if (address == null && _baseClient.AuthenticationProvider == null)
                throw new ArgumentNullException(nameof(address), "Address needs to be provided if API credentials not set");

            var parameters = new ParameterCollection()
            {
                { "type", "userFillsByTime" },
                { "user", address ?? _baseClient.AuthenticationProvider!.ApiKey }
            };
            parameters.AddMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("aggregateByTime", aggregateByTime);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "info", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 20, false);
            return await _baseClient.SendAsync<IEnumerable<HyperLiquidUserTrade>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order

        /// <inheritdoc />
        public async Task<WebCallResult<HyperLiquidOrderStatus>> GetOrderAsync(long? orderId = null, string? clientOrderId = null, string? address = null, CancellationToken ct = default)
        {
            if (address == null && _baseClient.AuthenticationProvider == null)
                throw new ArgumentNullException(nameof(address), "Address needs to be provided if API credentials not set");

            var parameters = new ParameterCollection()
            {
                { "type", "orderStatus" },
                { "user", address ?? _baseClient.AuthenticationProvider!.ApiKey }
            };

            parameters.AddOptional("oid", orderId);
            parameters.AddOptional("oid", clientOrderId);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "info", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 2, false);
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
        public async Task<WebCallResult<IEnumerable<HyperLiquidOrderStatus>>> GetOrderHistoryAsync(string? address = null, CancellationToken ct = default)
        {
            if (address == null && _baseClient.AuthenticationProvider == null)
                throw new ArgumentNullException(nameof(address), "Address needs to be provided if API credentials not set");

            var parameters = new ParameterCollection()
            {
                { "type", "historicalOrders" },
                { "user",  address ?? _baseClient.AuthenticationProvider!.ApiKey }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, "info", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 20, false);
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
            TpSlGrouping? tpSlGrouping = null,
            CancellationToken ct = default
            )
        {
            var result = await PlaceMultipleOrdersAsync([
                new HyperLiquidOrderRequest(symbolType, symbol, side, orderType, quantity, price, timeInForce, reduceOnly, triggerPrice, tpSlType, clientOrderId)
                ], ct).ConfigureAwait(false);

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
                    if (order.TriggerPrice == null)
                        throw new ArgumentNullException(nameof(order.TriggerPrice), "Trigger price should be provided for trigger orders");

                    var triggerParameters = new ParameterCollection();
                    triggerParameters.Add("isMarket", order.OrderType == OrderType.Market);
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

            var weight = 1 + (int)Math.Floor(orderRequests.Count / 40m);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 1, true);
            var intResult = await _baseClient.SendAuthAsync<HyperLiquidOrderResultIntWrapper>(request, parameters, ct, weight).ConfigureAwait(false);
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

            var weight = 1 + (int)Math.Floor(orderRequests.Count / 40m);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 1, true);
            var resultInt = await _baseClient.SendAuthAsync<HyperLiquidCancelResult>(request, parameters, ct, weight).ConfigureAwait(false);
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

            var weight = 1 + (int)Math.Floor(orderRequests.Count / 40m);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 1, true);
            var resultInt = await _baseClient.SendAuthAsync<HyperLiquidCancelResult>(request, parameters, ct, weight).ConfigureAwait(false);
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

            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 1, true);
            var result = await _baseClient.SendAsync<IEnumerable<HyperLiquidOrderStatus>>(request, parameters, ct).ConfigureAwait(false);


            return result;
        }

        #endregion

        #region Edit Order

        /// <inheritdoc />
        public async Task<WebCallResult> EditOrderAsync(
            SymbolType symbolType,
            string symbol,
            long? orderId,
            string? clientOrderId,
            OrderSide side,
            OrderType orderType,
            decimal quantity,
            decimal? price = null,
            TimeInForce? timeInForce = null,
            bool? reduceOnly = null,
            string? newClientOrderId = null,
            decimal? triggerPrice = null,
            TpSlType? tpSlType = null,
            CancellationToken ct = default)
        {
            var symbolId = await HyperLiquidUtils.GetSymbolIdFromNameAsync(symbolType, symbol).ConfigureAwait(false);
            if (!symbolId)
                return new WebCallResult(symbolId.Error!);

            if ((orderId == null) == (clientOrderId == null))
                throw new ArgumentException("Either orderId or clientOrderId should be provided");

            var parameters = new ParameterCollection();
            var actionParameters = new ParameterCollection()
            {
                { "type", "modify" }
            };
            actionParameters.AddOptional("oid", orderId);
            actionParameters.AddOptional("oid", clientOrderId);

            var orderParameters = new ParameterCollection();

            orderParameters.Add("a", symbolId.Data);
            orderParameters.AddOrNull("b", side == OrderSide.Buy);
            orderParameters.AddStringOrNull("p", price);
            orderParameters.AddString("s", quantity);
            orderParameters.Add("r", reduceOnly ?? false);

            var orderTypeParameters = new ParameterCollection();
            if (orderType == OrderType.Limit)
            {
                var limitParameters = new ParameterCollection();
                limitParameters.AddEnum("tif", timeInForce);
                orderTypeParameters.Add("limit", limitParameters);
            }
            else if (orderType == OrderType.StopMarket || orderType == OrderType.StopLimit)
            {
                if (triggerPrice == null)
                    throw new ArgumentNullException(nameof(triggerPrice), "Trigger price should be provided for trigger orders");

                var triggerParameters = new ParameterCollection();
                triggerParameters.Add("isMarket", orderType == OrderType.Market);
                triggerParameters.Add("triggerPx", triggerPrice);
                triggerParameters.AddEnum("tpsl", tpSlType);
                orderTypeParameters.Add("trigger", triggerParameters);
            }

            orderParameters.Add("t", orderTypeParameters);
            orderParameters.AddOptional("c", newClientOrderId);

            actionParameters.Add("order", orderParameters);
            parameters.Add("action", actionParameters);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 1, true);
            var result = await _baseClient.SendAsync<HyperLiquidResponse>(request, parameters, ct).ConfigureAwait(false);
            return result.AsDataless();
        }

        #endregion

        #region Edit Orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<CallResult<HyperLiquidOrderResult>>>> EditOrdersAsync(
            IEnumerable<HyperLiquidEditOrderRequest> requests,
            CancellationToken ct = default)
        {
            var orderRequests = new List<ParameterCollection>();
            foreach (var order in requests)
            {
                var symbolId = await HyperLiquidUtils.GetSymbolIdFromNameAsync(order.SymbolType, order.Symbol).ConfigureAwait(false);
                if (!symbolId)
                    return new WebCallResult<IEnumerable<CallResult<HyperLiquidOrderResult>>>(symbolId.Error!);

                var modifyParameters = new ParameterCollection();
                modifyParameters.AddOptional("oid", order.OrderId);
                modifyParameters.AddOptional("oid", order.ClientOrderId);
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
                else if (order.OrderType == OrderType.StopMarket || order.OrderType == OrderType.StopLimit)
                {
                    if (order.TriggerPrice == null)
                        throw new ArgumentNullException(nameof(order.TriggerPrice), "Trigger price should be provided for trigger orders");

                    var triggerParameters = new ParameterCollection();
                    triggerParameters.Add("isMarket", order.OrderType == OrderType.Market);
                    triggerParameters.Add("triggerPx", order.TriggerPrice);
                    triggerParameters.AddEnum("tpsl", order.TpSlType);
                    orderTypeParameters.Add("trigger", triggerParameters);
                }

                orderParameters.Add("t", orderTypeParameters);

                orderParameters.AddOptional("c", order.NewClientOrderId);

                modifyParameters.Add("order", orderParameters);
                orderRequests.Add(modifyParameters);
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
                        { "type", "batchModify" },
                        { "modifies", orderRequests }
                    }
                }
            };

            var weight = 1 + (int)Math.Floor(orderRequests.Count / 40m);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 1, true);
            var intResult = await _baseClient.SendAuthAsync<HyperLiquidOrderResultIntWrapper>(request, parameters, ct, weight).ConfigureAwait(false);
            if (!intResult)
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

        #region Set Leverage

        /// <inheritdoc />
        public async Task<WebCallResult> SetLeverageAsync(string symbol, int leverage, MarginType marginType, CancellationToken ct = default)
        {
            var symbolId = await HyperLiquidUtils.GetSymbolIdFromNameAsync(SymbolType.Futures, symbol).ConfigureAwait(false);
            if (!symbolId)
                return new WebCallResult(symbolId.Error!);

            var parameters = new ParameterCollection();
            var actionParameters = new ParameterCollection()
            {
                { "type", "updateLeverage" },
                { "asset", symbolId.Data }
            };
            actionParameters.Add("isCross", marginType == MarginType.Cross);
            actionParameters.Add("leverage", leverage);
            parameters.Add("action", actionParameters);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 1, true);
            var result = await _baseClient.SendAsync<HyperLiquidResponse>(request, parameters, ct).ConfigureAwait(false);
            return result.AsDataless();
        }

        #endregion

        #region Update Isolated Margin

        /// <inheritdoc />
        public async Task<WebCallResult> UpdateIsolatedMarginAsync(string symbol, decimal updateValue, CancellationToken ct = default)
        {
            var symbolId = await HyperLiquidUtils.GetSymbolIdFromNameAsync(SymbolType.Futures, symbol).ConfigureAwait(false);
            if (!symbolId)
                return new WebCallResult(symbolId.Error!);

            var parameters = new ParameterCollection();
            var actionParameters = new ParameterCollection()
            {
                { "type", "updateIsolatedMargin" },
                { "asset", symbolId.Data },
                { "isBuy", true }
            };
            actionParameters.Add("ntli", updateValue);
            parameters.Add("action", actionParameters);

#warning check
            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 1, true);
            var result = await _baseClient.SendAsync<HyperLiquidResponse>(request, parameters, ct).ConfigureAwait(false);
            return result.AsDataless();
        }

        #endregion

        #region Place TWAP order

        /// <inheritdoc />
        public async Task<WebCallResult<HyperLiquidTwapOrderResult>> PlaceTwapOrderAsync(SymbolType symbolType, string symbol, OrderSide orderSide, decimal quantity, bool reduceOnly, int minutes, bool randomize, CancellationToken ct = default)
        {
            var symbolId = await HyperLiquidUtils.GetSymbolIdFromNameAsync(symbolType, symbol).ConfigureAwait(false);
            if (!symbolId)
                return new WebCallResult<HyperLiquidTwapOrderResult>(symbolId.Error);

            var orderParameters = new ParameterCollection();
            orderParameters.Add("a", symbolId.Data);
            orderParameters.Add("b", orderSide == OrderSide.Buy);
            orderParameters.AddString("s", quantity);
            orderParameters.Add("r", reduceOnly);
            orderParameters.Add("m", minutes);
            orderParameters.Add("t", randomize);

            var parameters = new ParameterCollection()
            {
                { "type", "twapOrder" },
                { "twap", orderParameters }
            };
            parameters.AddMilliseconds("nonce", DateTime.UtcNow);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 1, false);
            var result = await _baseClient.SendAsync<HyperLiquidTwapOrderResultIntWrapper>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result.As<HyperLiquidTwapOrderResult>(default);

            if (result.Data.Status.Error != null)
                return result.AsError<HyperLiquidTwapOrderResult>(new ServerError(result.Data.Status.Error));

            return result.As(result.Data.Status.ResultRunning!);
        }

        #endregion

        #region Cancel Twap Order

        /// <inheritdoc />
        public async Task<WebCallResult<HyperLiquidTwapOrderResult>> CancelTwapOrderAsync(SymbolType symbolType, string symbol, long twapId, CancellationToken ct = default)
        {
            var symbolId = await HyperLiquidUtils.GetSymbolIdFromNameAsync(symbolType, symbol).ConfigureAwait(false);
            if (!symbolId)
                return new WebCallResult<HyperLiquidTwapOrderResult>(symbolId.Error);

            var parameters = new ParameterCollection()
            {
                { "type", "twapCancel" },
                { "a", symbolId.Data },
                { "t", twapId }
            };
            parameters.AddMilliseconds("nonce", DateTime.UtcNow);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 1, false);
            var result = await _baseClient.SendAsync<HyperLiquidTwapOrderResultIntWrapper>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result.As<HyperLiquidTwapOrderResult>(default);

            if (result.Data.Status.Error != null)
                return result.AsError<HyperLiquidTwapOrderResult>(new ServerError(result.Data.Status.Error));

            return result.As(result.Data.Status.ResultRunning!);
        }

        #endregion
    }
}
