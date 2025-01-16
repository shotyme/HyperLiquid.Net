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
        Task<WebCallResult<Dictionary<string, decimal>>> GetPricesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get order book
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/info-endpoint#l2-book-snapshot" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example "HYPE/USDC" for spot, or "ETH" for futures</param>
        /// <param name="numberSignificantFigures">Asset name</param>
        /// <param name="mantissa">Mantissa</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<HyperLiquidOrderBook>> GetOrderBookAsync(string symbol, int? numberSignificantFigures = null, int? mantissa = null, CancellationToken ct = default);

        /// <summary>
        /// Get klines
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/info-endpoint#candle-snapshot" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example "HYPE/USDC" for spot, or "ETH" for futures</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="startTime">Data start time</param>
        /// <param name="endTime">Data end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<IEnumerable<HyperLiquidKline>>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime startTime, DateTime endTime, CancellationToken ct = default);

        /// <summary>
        /// Get spot exchange info
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/info-endpoint/spot#retrieve-spot-metadata" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<HyperLiquidSpotExchangeInfo>> GetSpotExchangeInfoAsync(CancellationToken ct = default);

        /// <summary>
        /// Get futures exchange info
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/info-endpoint/perpetuals" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<IEnumerable<HyperLiquidFuturesSymbol>>> GetFuturesExchangeInfoAsync(CancellationToken ct = default);

        /// <summary>
        /// Get spot exchange info and ticker info
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/info-endpoint/spot#retrieve-spot-asset-contexts" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<HyperLiquidExchangeInfoAndTickers>> GetSpotExchangeInfoAndTickersAsync(CancellationToken ct = default);

        /// <summary>
        /// Get information on an asset
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/info-endpoint/spot#retrieve-information-about-a-token" /></para>
        /// </summary>
        /// <param name="assetId">The asset id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<HyperLiquidAssetInfo>> GetAssetInfoAsync(string assetId, CancellationToken ct = default);

        /// <summary>
        /// Get futures exchange info and ticker info
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/info-endpoint/perpetuals#retrieve-perpetuals-asset-contexts-includes-mark-price-current-funding-open-interest-etc" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<HyperLiquidFuturesExchangeInfoAndTickers>> GetFuturesExchangeInfoAndTickersAsync(CancellationToken ct = default);

        /// <summary>
        /// Get funding rate history
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/info-endpoint/perpetuals#retrieve-historical-funding-rates" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example "ETH"</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<IEnumerable<HyperLiquidFundingRate>>> GetFundingRateHistoryAsync(string symbol, DateTime startTime, DateTime? endTime = null, CancellationToken ct = default);

        /// <summary>
        /// Get futures symbols at max open interest
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<IEnumerable<string>>> GetFuturesSymbolsAtMaxOpenInterestAsync(CancellationToken ct = default);
    }
}
