using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using CryptoExchange.Net.Objects;
using HyperLiquid.Net.Objects.Models;

namespace HyperLiquid.Net.Interfaces.Clients.Api
{
    /// <summary>
    /// HyperLiquid  account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
    /// </summary>
    public interface IHyperLiquidRestClientApiAccount
    {
        Task<WebCallResult<IEnumerable<HyperLiquidBalance>>> GetBalancesAsync(string address, CancellationToken ct = default);

        Task<WebCallResult<HyperLiquidRateLimit>> GetRateLimitsAsync(string address, CancellationToken ct = default);

        Task<WebCallResult<int>> GetApprovedBuilderFeeAsync(string address, string builderAddress, CancellationToken ct = default);
    }
}
