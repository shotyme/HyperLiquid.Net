using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using HyperLiquid.Net.Enums;
using HyperLiquid.Net.Objects.Models;

namespace HyperLiquid.Net.Interfaces.Clients.Api
{
    /// <summary>
    /// HyperLiquid  exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IHyperLiquidRestClientApiExchangeData
    {
        /// <summary>
        /// Get mid prices for all assets
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/info-endpoint#retrieve-mids-for-all-actively-traded-coins" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<Dictionary<string, decimal>>> GetMidsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get order book
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/info-endpoint#l2-book-snapshot" /></para>
        /// </summary>
        /// <param name="asset">Asset name</param>
        /// <param name="numberSignificantFigures">Asset name</param>
        /// <param name="mantissa">Mantissa</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HyperLiquidOrderBook>> GetOrderBookAsync(string asset, int? numberSignificantFigures = null, int? mantissa = null, CancellationToken ct = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="interval"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<HyperLiquidKline>>> GetKlinesAsync(string asset, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

        Task<WebCallResult<HyperLiquidExchangeInfo>> GetSpotExchangeInfoAsync(CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<HyperLiquidFuturesSymbol>>> GetFuturesExchangeInfoAsync(CancellationToken ct = default);
        Task<WebCallResult<HyperLiquidExchangeInfoAndTickers>> GetSpotExchangeInfoAndTickersAsync(CancellationToken ct = default);

        Task<WebCallResult<HyperLiquidAssetInfo>> GetAssetInfoAsync(string assetId, CancellationToken ct = default);
    }
}
