using CryptoExchange.Net.Objects;
using HyperLiquid.Net.Clients.Api;
using HyperLiquid.Net.Interfaces.Clients.Api;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using HyperLiquid.Net.Objects.Models;

namespace HyperLiquid.Net.Clients.Api
{
    /// <inheritdoc />
    internal class HyperLiquidRestClientApiAccount : IHyperLiquidRestClientApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly HyperLiquidRestClientApi _baseClient;

        internal HyperLiquidRestClientApiAccount(HyperLiquidRestClientApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Balances

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HyperLiquidBalance>>> GetBalancesAsync(string address, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "type", "spotClearinghouseState" },
                { "user", address }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Post, "info", HyperLiquidExchange.RateLimiter.HyperLiquid, 1, false);
            var result = await _baseClient.SendAsync<HyperLiquidBalances>(request, parameters, ct).ConfigureAwait(false);
            return result.As<IEnumerable<HyperLiquidBalance>>(result.Data?.Balances);
        }

        #endregion

        #region Get Rate Limits

        /// <inheritdoc />
        public async Task<WebCallResult<HyperLiquidRateLimit>> GetRateLimitsAsync(string address, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "type", "userRateLimit" },
                { "user", address }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Post, "info", HyperLiquidExchange.RateLimiter.HyperLiquid, 1, false);
            return await _baseClient.SendAsync<HyperLiquidRateLimit>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Approved Builder Fee

        /// <inheritdoc />
        public async Task<WebCallResult<int>> GetApprovedBuilderFeeAsync(string address, string builderAddress, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "type", "maxBuilderFee" },
                { "user", address },
                { "builder", builderAddress }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Post, "info", HyperLiquidExchange.RateLimiter.HyperLiquid, 1, false);
            return await _baseClient.SendAsync<int>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
