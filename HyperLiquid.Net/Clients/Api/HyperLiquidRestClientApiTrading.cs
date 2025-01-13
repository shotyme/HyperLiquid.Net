using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using HyperLiquid.Net.Interfaces.Clients.Api;
using HyperLiquid.Net.Objects.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System;

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

        #region Cancel Order

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HyperLiquidOrderStatus>>> CancelOrderAsync(string symbol, long orderId, CancellationToken ct = default)
        {
            var symbolId = 1; // TODO
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
                                    { "a", symbolId },
                                    { "o", orderId }
                                }
                            }
                        }
                    }
                }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquid, 1, true);
            return await _baseClient.SendAsync<IEnumerable<HyperLiquidOrderStatus>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
