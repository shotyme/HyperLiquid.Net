using CryptoExchange.Net.Objects;
using HyperLiquid.Net.Clients;
using HyperLiquid.Net.Enums;
using HyperLiquid.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HyperLiquid.Net.Utils
{
    public static class HyperLiquidUtils
    {
        private static IEnumerable<HyperLiquidSymbol>? _spotSymbolInfo;
        private static IEnumerable<HyperLiquidFuturesSymbol>? _futuresSymbolInfo;

        private static DateTime _lastSpotUpdateTime;
        private static DateTime _lastFuturesUpdateTime;

        private static readonly SemaphoreSlim _semaphoreSpot = new SemaphoreSlim(1, 1);
        private static readonly SemaphoreSlim _semaphoreFutures = new SemaphoreSlim(1, 1);

        public static async Task<CallResult> UpdateFuturesSymbolInfoAsync()
        {
            await _semaphoreFutures.WaitAsync().ConfigureAwait(false);

            try
            {
                if (DateTime.UtcNow - _lastFuturesUpdateTime < TimeSpan.FromHours(1))
                    return new CallResult(null);

                var symbolInfo = await new HyperLiquidRestClient().Api.ExchangeData.GetFuturesExchangeInfoAsync().ConfigureAwait(false);
                if (!symbolInfo)
                    return symbolInfo.AsDataless();

                _futuresSymbolInfo = symbolInfo.Data;
                _lastFuturesUpdateTime = DateTime.UtcNow;
                return new CallResult(null);
            }
            finally
            {
                _semaphoreFutures.Release();
            }
        }

        public static async Task<CallResult> UpdateSpotSymbolInfoAsync()
        {
            await _semaphoreSpot.WaitAsync().ConfigureAwait(false);
            try
            {
                if (DateTime.UtcNow - _lastSpotUpdateTime < TimeSpan.FromHours(1))
                    return new CallResult(null);

                var symbolInfo = await new HyperLiquidRestClient().Api.ExchangeData.GetSpotExchangeInfoAsync().ConfigureAwait(false);
                if (!symbolInfo)
                    return symbolInfo.AsDataless();

                _spotSymbolInfo = symbolInfo.Data.Symbols;
                _lastSpotUpdateTime = DateTime.UtcNow;
                return new CallResult(null);
            }
            finally
            {
                _semaphoreSpot.Release();
            }
        }

        public static async Task<CallResult<int>> GetSymbolIdFromNameAsync(SymbolType type, string symbolName)
        {
            if (type == SymbolType.Spot)
            {
                var update = await UpdateSpotSymbolInfoAsync().ConfigureAwait(false);
                if (!update)
                    return new CallResult<int>(update.Error!);

                var symbol = _spotSymbolInfo.SingleOrDefault(x => x.Name == symbolName);
                if (symbol == null)
                    return new CallResult<int>(new ServerError("Symbol not found"));

                return new CallResult<int>(symbol.Index + 10000);
            }
            else
            {
                var update = await UpdateFuturesSymbolInfoAsync().ConfigureAwait(false);
                if (!update)
                    return new CallResult<int>(update.Error!);

                var symbol = _futuresSymbolInfo.SingleOrDefault(x => x.Name == symbolName);
                if (symbol == null)
                    return new CallResult<int>(new ServerError("Symbol not found"));

                return new CallResult<int>(symbol.Index);
            }
        }

        public static async Task<CallResult<string>> GetSymbolNameFromExchangeNameAsync(string id)
        {
            var symbol = _spotSymbolInfo.SingleOrDefault(x => x.ExchangeName == id);
            if (symbol == null)
                return new CallResult<string>(new ServerError("Symbol not found"));

            return new CallResult<string>(symbol.Name);
        }
    }
}
