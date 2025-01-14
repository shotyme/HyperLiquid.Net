using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces.CommonClients;
using HyperLiquid.Net.Interfaces.Clients.Api;
using HyperLiquid.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Converters.MessageParsing;
using HyperLiquid.Net.Objects.Models;

namespace HyperLiquid.Net.Clients.Api
{
    /// <inheritdoc cref="IHyperLiquidRestClientApi" />
    internal partial class HyperLiquidRestClientApi : RestApiClient, IHyperLiquidRestClientApi
    {
        #region fields 
        internal static TimeSyncState _timeSyncState = new TimeSyncState(" Api");
        #endregion

        #region Api clients
        /// <inheritdoc />
        public IHyperLiquidRestClientApiAccount Account { get; }
        /// <inheritdoc />
        public IHyperLiquidRestClientApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IHyperLiquidRestClientApiTrading Trading { get; }
        /// <inheritdoc />
        public string ExchangeName => "HyperLiquid";
        #endregion

        #region constructor/destructor
        internal HyperLiquidRestClientApi(ILogger logger, HttpClient? httpClient, HyperLiquidRestOptions options)
            : base(logger, httpClient, options.Environment.RestClientAddress, options, options.Options)
        {
            Account = new HyperLiquidRestClientApiAccount(this);
            ExchangeData = new HyperLiquidRestClientApiExchangeData(logger, this);
            Trading = new HyperLiquidRestClientApiTrading(logger, this);
        }
        #endregion

        /// <inheritdoc />
        protected override IStreamMessageAccessor CreateAccessor() => new SystemTextJsonStreamMessageAccessor();
        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer();

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new HyperLiquidAuthenticationProvider(credentials);

        internal Task<WebCallResult> SendAsync(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
            => SendToAddressAsync(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult> SendToAddressAsync(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);

            // Optional response checking

            return result;
        }

        internal Task<WebCallResult<T>> SendAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
            => SendToAddressAsync<T>(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult<T>> SendToAddressAsync<T>(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<T>(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);

            // Optional response checking

            return result;
        }

        internal async Task<WebCallResult<T>> SendAuthAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await SendToAddressAsync<HyperLiquidResponse<T>>(BaseAddress, definition, parameters, cancellationToken, weight).ConfigureAwait(false);
            if (!result)
                return result.As<T>(default);

            if (!result.Data.Status.Equals("ok"))
                return result.AsError<T>(new ServerError(result.Data.Status));

            return result.As<T>(result.Data.Data.Data);
        }


        protected override Error? TryParseError(IEnumerable<KeyValuePair<string, IEnumerable<string>>> responseHeaders, IMessageAccessor accessor)
        {
            var status = accessor.GetValue<string?>(MessagePath.Get().Property("status"));
            if (status == "err")
                return new ServerError(accessor.GetValue<string>(MessagePath.Get().Property("response"))!);

            return null;
        }

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync() => throw new NotImplementedException();

        /// <inheritdoc />
        public override TimeSyncInfo? GetTimeSyncInfo() => null;

        /// <inheritdoc />
        public override TimeSpan? GetTimeOffset() => _timeSyncState.TimeOffset;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null) 
            => HyperLiquidExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

        /// <inheritdoc />
        public IHyperLiquidRestClientApiShared SharedClient => this;

    }
}
