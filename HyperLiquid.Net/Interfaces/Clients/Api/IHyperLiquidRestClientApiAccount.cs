using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using CryptoExchange.Net.Objects;
using HyperLiquid.Net.Objects.Models;
using HyperLiquid.Net.Enums;

namespace HyperLiquid.Net.Interfaces.Clients.Api
{
    /// <summary>
    /// HyperLiquid  account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
    /// </summary>
    public interface IHyperLiquidRestClientApiAccount
    {
        /// <summary>
        /// Get user asset balances
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/info-endpoint/spot#retrieve-a-users-token-balances" /></para>
        /// </summary>
        /// <param name="address">Address to request balances for. If not provided will use the address provided in the API credentials</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<IEnumerable<HyperLiquidBalance>>> GetBalancesAsync(string? address = null, CancellationToken ct = default);

        /// <summary>
        /// Get user rate limits
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/info-endpoint#query-user-rate-limits" /></para>
        /// </summary>
        /// <param name="address">Address to request rate limits for. If not provided will use the address provided in the API credentials</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<HyperLiquidRateLimit>> GetRateLimitsAsync(string? address = null, CancellationToken ct = default);

        /// <summary>
        /// Get the approved builder fee
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/info-endpoint#check-builder-fee-approval" /></para>
        /// </summary>
        /// <param name="builderAddress">The address of the builder</param>
        /// <param name="address">Address to request approved builder fee for. If not provided will use the address provided in the API credentials</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<int>> GetApprovedBuilderFeeAsync(string builderAddress, string? address = null, CancellationToken ct = default);

        /// <summary>
        /// Send usd to another address. This transfer does not touch the EVM bridge.
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/exchange-endpoint#l1-usdc-transfer" /></para>
        /// </summary>
        /// <param name="signatureChainId">The id of the chain used when signing in hexadecimal format; e.g. "0xa4b1" for Arbitrum</param>
        /// <param name="destinationAddress">Address in 42-character hexadecimal format; e.g. 0x0000000000000000000000000000000000000000</param>
        /// <param name="quantity">Quantity of USD to send</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> TransferUsdAsync(string signatureChainId, string destinationAddress, decimal quantity, CancellationToken ct = default);

        /// <summary>
        /// Send spot assets to another address. This transfer does not touch the EVM bridge.
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/exchange-endpoint#l1-spot-transfer" /></para>
        /// </summary>
        /// <param name="signatureChainId">The id of the chain used when signing in hexadecimal format; e.g. "0xa4b1" for Arbitrum</param>
        /// <param name="destinationAddress">Address in 42-character hexadecimal format; e.g. 0x0000000000000000000000000000000000000000</param>
        /// <param name="asset">Asset name</param>
        /// <param name="quantity">Quantity to send</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> TransferSpotAsync(
            string signatureChainId,
            string destinationAddress,
            string asset,
            decimal quantity,
            CancellationToken ct = default);

        /// <summary>
        /// Initiate the withdrawal flow. After making this request, the L1 validators will sign and send the withdrawal request to the bridge contract. There is a $1 fee for withdrawing at the time of this writing and withdrawals take approximately 5 minutes to finalize.
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/exchange-endpoint#initiate-a-withdrawal-request" /></para>
        /// </summary>
        /// <param name="signatureChainId">The id of the chain used when signing in hexadecimal format; e.g. "0xa4b1" for Arbitrum</param>
        /// <param name="destinationAddress">Address in 42-character hexadecimal format; e.g. 0x0000000000000000000000000000000000000000</param>
        /// <param name="quantity">Quantity of USD to send</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> WithdrawAsync(
            string signatureChainId,
            string destinationAddress,
            decimal quantity,
            CancellationToken ct = default);

        /// <summary>
        /// Transfer USD between Spot and Futures account
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/exchange-endpoint#transfer-from-spot-account-to-perp-account-and-vice-versa" /></para>
        /// </summary>
        /// <param name="signatureChainId">The id of the chain used when signing in hexadecimal format; e.g. "0xa4b1" for Arbitrum</param>
        /// <param name="direction">Transfer direction</param>
        /// <param name="quantity">Quantity of USD to send</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> TransferInternalAsync(
            string signatureChainId,
            TransferDirection direction,
            decimal quantity,
            CancellationToken ct = default);

        /// <summary>
        /// Deposit into staking
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/exchange-endpoint#deposit-into-staking" /></para>
        /// </summary>
        /// <param name="signatureChainId"></param>
        /// <param name="wei"></param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> DepositIntoStakingAsync(string signatureChainId, long wei, CancellationToken ct = default);

        /// <summary>
        /// 
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/exchange-endpoint#withdraw-from-staking" /></para>
        /// </summary>
        /// <param name="signatureChainId"></param>
        /// <param name="wei"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult> WithdrawFromStakingAsync(string signatureChainId, long wei, CancellationToken ct = default);

        /// <summary>
        /// 
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/exchange-endpoint#delegate-or-undelegate-stake-from-validator" /></para>
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="validator"></param>
        /// <param name="wei"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult> DelegateOrUndelegateStakeFromValidatorAsync(DelegateDirection direction, string validator, long wei, CancellationToken ct = default);

        /// <summary>
        /// 
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/exchange-endpoint#deposit-or-withdraw-from-a-vault" /></para>
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="vaultAddress"></param>
        /// <param name="usd"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult> DepositOrWithdrawFromVaultAsync(DepositWithdrawDirection direction, string vaultAddress, long usd, CancellationToken ct = default);
    }
}
