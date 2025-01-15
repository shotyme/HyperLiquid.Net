using CryptoExchange.Net.Objects;
using HyperLiquid.Net.Interfaces.Clients.Api;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using HyperLiquid.Net.Objects.Models;
using System;
using HyperLiquid.Net.Utils;
using HyperLiquid.Net.Enums;
using System.Globalization;

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
        public async Task<WebCallResult<IEnumerable<HyperLiquidBalance>>> GetBalancesAsync(string? address = null, CancellationToken ct = default)
        {
            if (address == null && _baseClient.AuthenticationProvider == null)
                throw new ArgumentNullException(nameof(address), "Address needs to be provided if API credentials not set");

            var parameters = new ParameterCollection()
            {
                { "type", "spotClearinghouseState" },
                { "user", address ?? _baseClient.AuthenticationProvider!.ApiKey }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Post, "info", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 2, false);
            var result = await _baseClient.SendAsync<HyperLiquidBalances>(request, parameters, ct).ConfigureAwait(false);
            return result.As<IEnumerable<HyperLiquidBalance>>(result.Data?.Balances);
        }

        #endregion

        #region Get Rate Limits

        /// <inheritdoc />
        public async Task<WebCallResult<HyperLiquidRateLimit>> GetRateLimitsAsync(string? address = null, CancellationToken ct = default)
        {
            if (address == null && _baseClient.AuthenticationProvider == null)
                throw new ArgumentNullException(nameof(address), "Address needs to be provided if API credentials not set");

            var parameters = new ParameterCollection()
            {
                { "type", "userRateLimit" },
                { "user", address ?? _baseClient.AuthenticationProvider!.ApiKey }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Post, "info", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 20, false);
            return await _baseClient.SendAsync<HyperLiquidRateLimit>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Approved Builder Fee

        /// <inheritdoc />
        public async Task<WebCallResult<int>> GetApprovedBuilderFeeAsync(string builderAddress, string? address = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "type", "maxBuilderFee" },
                { "user", address ?? _baseClient.AuthenticationProvider!.ApiKey },
                { "builder", builderAddress }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Post, "info", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 20, false);
            return await _baseClient.SendAsync<int>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Transfer USD

        /// <inheritdoc />
        public async Task<WebCallResult> TransferUsdAsync(string signatureChainId, string destinationAddress, decimal quantity, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var actionParameters = new ParameterCollection()
            {
                { "type", "usdSend" },
                { "hyperliquidChain", _baseClient.ClientOptions.Environment.Name == TradeEnvironmentNames.Testnet ? "Testnet" : "Mainnet" },
                { "signatureChainId", signatureChainId },
                { "destination", destinationAddress }
            };
            actionParameters.AddString("amount", quantity);
            actionParameters.AddMilliseconds("time", DateTime.UtcNow);
            parameters.Add("action", actionParameters);

#warning check signature

            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 1, true);
            var result = await _baseClient.SendAsync<HyperLiquidResponse>(request, parameters, ct).ConfigureAwait(false);
            return result.AsDataless();
        }

        #endregion

        #region Spot Transfer

        /// <inheritdoc />
        public async Task<WebCallResult> TransferSpotAsync(
            string signatureChainId,
            string destinationAddress,
            string asset,
            decimal quantity,
            CancellationToken ct = default)
        {
            var assetId = await HyperLiquidUtils.GetAssetNameAndIdAsync(asset).ConfigureAwait(false);
            if (!assetId)
                return new WebCallResult(assetId.Error!);

            var parameters = new ParameterCollection();
            var actionParameters = new ParameterCollection()
            {
                { "type", "spotSend" },
                { "hyperliquidChain", _baseClient.ClientOptions.Environment.Name == TradeEnvironmentNames.Testnet ? "Testnet" : "Mainnet" },
                { "signatureChainId", signatureChainId },
                { "destination", destinationAddress },
                { "token", assetId.Data }
            };
            actionParameters.AddString("amount", quantity);
            actionParameters.AddMilliseconds("time", DateTime.UtcNow);
            parameters.Add("action", actionParameters);

#warning check signature

            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 1, true);
            var result = await _baseClient.SendAsync<HyperLiquidResponse>(request, parameters, ct).ConfigureAwait(false);
            return result.AsDataless();
        }

        #endregion

        #region Withdraw

        /// <inheritdoc />
        public async Task<WebCallResult> WithdrawAsync(
            string signatureChainId,
            string destinationAddress,
            decimal quantity,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var actionParameters = new ParameterCollection()
            {
                { "type", "withdraw3" },
                { "hyperliquidChain", _baseClient.ClientOptions.Environment.Name == TradeEnvironmentNames.Testnet ? "Testnet" : "Mainnet" },
                { "signatureChainId", signatureChainId },
                { "destination", destinationAddress },
            };
            actionParameters.AddString("amount", quantity);
            actionParameters.AddMilliseconds("time", DateTime.UtcNow);
            parameters.Add("action", actionParameters);

#warning check signature

            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 1, true);
            var result = await _baseClient.SendAsync<HyperLiquidResponse>(request, parameters, ct).ConfigureAwait(false);
            return result.AsDataless();
        }

        #endregion

        #region Withdraw

        /// <inheritdoc />
        public async Task<WebCallResult> TransferInternalAsync(
            string signatureChainId,
            TransferDirection direction,
            decimal quantity,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var actionParameters = new ParameterCollection()
            {
                { "type", "usdClassTransfer" },
                { "hyperliquidChain", _baseClient.ClientOptions.Environment.Name == TradeEnvironmentNames.Testnet ? "Testnet" : "Mainnet" },
                { "signatureChainId", signatureChainId },
                { "toPerp", direction == TransferDirection.SpotToFutures },
            };
            actionParameters.AddString("amount", quantity);
            actionParameters.AddMilliseconds("nonce", DateTime.UtcNow);
            parameters.Add("action", actionParameters);

#warning check signature

            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 1, true);
            var result = await _baseClient.SendAsync<HyperLiquidResponse>(request, parameters, ct).ConfigureAwait(false);
            return result.AsDataless();
        }

        #endregion

        #region Deposit Into Staking

        /// <inheritdoc />
        public async Task<WebCallResult> DepositIntoStakingAsync(string signatureChainId, long wei, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "type", "cDeposit" },
                { "hyperliquidChain", _baseClient.ClientOptions.Environment.Name == TradeEnvironmentNames.Testnet ? "Testnet" : "Mainnet" },
                { "signatureChainId", signatureChainId },
                { "wei", wei }
            };
            parameters.AddMilliseconds("nonce", DateTime.UtcNow);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 1, false);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Withdrawa From Staking

        /// <inheritdoc />
        public async Task<WebCallResult> WithdrawFromStakingAsync(string signatureChainId, long wei, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "type", "cWithdraw" },
                { "hyperliquidChain", _baseClient.ClientOptions.Environment.Name == TradeEnvironmentNames.Testnet ? "Testnet" : "Mainnet" },
                { "signatureChainId", signatureChainId },
                { "wei", wei }
            };
            parameters.AddMilliseconds("nonce", DateTime.UtcNow);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 1, false);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Delegate Or Undelegate Stake

        /// <inheritdoc />
        public async Task<WebCallResult> DelegateOrUndelegateStakeFromValidatorAsync(DelegateDirection direction, string validator, long wei, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "type", "tokenDelegate" },
                { "validator", validator },
                { "isUndelegate", direction == DelegateDirection.Undelegate },
                { "wei", wei }
            };
            parameters.AddMilliseconds("nonce", DateTime.UtcNow);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 1, false);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Deposit Or Withdraw From Vault

        /// <inheritdoc />
        public async Task<WebCallResult> DepositOrWithdrawFromVaultAsync(DepositWithdrawDirection direction, string vaultAddress, long usd, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "type", "vaultTransfer" },
                { "vaultAddress", vaultAddress },
                { "isDeposit", direction == DepositWithdrawDirection.Deposit },
                { "usd", usd }
            };
            parameters.AddMilliseconds("nonce", DateTime.UtcNow);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 1, false);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Approve Builder Fee

        /// <inheritdoc />
        public async Task<WebCallResult> ApproveBuilderFeeAsync(string hyperliquidChain, string builderAddress, decimal maxFeePercentage, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "type", "approveBuilderFee" },
                { "hyperliquidChain", _baseClient.ClientOptions.Environment.Name == TradeEnvironmentNames.Testnet ? "Testnet" : "Mainnet" },
                { "signatureChainId", hyperliquidChain },
#warning signatureChainId is not something the user has to pass but something we use in the signing?
                { "maxFeeRate", $"{maxFeePercentage.ToString(CultureInfo.InvariantCulture)}%" },
                { "builder", builderAddress }
            };
            parameters.AddMilliseconds("nonce", DateTime.UtcNow);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "exchange", HyperLiquidExchange.RateLimiter.HyperLiquidRest, 1, false);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
