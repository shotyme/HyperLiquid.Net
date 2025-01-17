using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HyperLiquid.Net.Clients.SpotApi;
using CryptoExchange.Net.Objects;
using HyperLiquid.Net.Objects.Models;

namespace HyperLiquid.Net.Clients.BaseApi
{
    internal class HyperLiquidRestClientAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly HyperLiquidRestClientApi _baseClient;

        internal HyperLiquidRestClientAccount(HyperLiquidRestClientApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Trading Fee

        /// <inheritdoc />
        public async Task<WebCallResult<HyperLiquidFeeInfo>> GetFeeInfoAsync(string? address = null, CancellationToken ct = default)
        {
            if (address == null && _baseClient.AuthenticationProvider == null)
                throw new ArgumentNullException(nameof(address), "Address needs to be provided if API credentials not set");

            var parameters = new ParameterCollection()
            {
                { "type", "userFees" },
                { "user", address ?? _baseClient.AuthenticationProvider!.ApiKey }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Post, "info", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 20, false);
            return await _baseClient.SendAsync<HyperLiquidFeeInfo>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
