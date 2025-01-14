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
        private static IEnumerable<HyperLiquidSymbol>? _futuresSymbolInfo;

        private static DateTime _lastUpdateTime;

        private static readonly SemaphoreSlim _semaphoreSpot = new SemaphoreSlim(1, 1);
        private static readonly SemaphoreSlim _semaphoreFutures = new SemaphoreSlim(1, 1);

        //public static async Task<CallResult> UpdateFuturesSymbolInfoAsync()
        //{
        //    await _semaphoreFutures.WaitAsync().ConfigureAwait(false);
        //    if (DateTime.UtcNow - _lastUpdateTime < TimeSpan.FromHours(1))
        //        return new CallResult(null);

        //    try
        //    {
        //        var symbolInfo = await new HyperLiquidRestClient().Api.ExchangeData.Get().ConfigureAwait(false);
        //        if (!symbolInfo)
        //            return symbolInfo.AsDataless();

        //        _symbolInfo = symbolInfo.Data.Symbols;
        //        _lastUpdateTime = DateTime.UtcNow;
        //        return new CallResult(null);
        //    }
        //    finally
        //    {
        //        _semaphoreFutures.Release();
        //    }
        //}

        public static async Task<CallResult> UpdateSpotSymbolInfoAsync()
        {
            await _semaphoreSpot.WaitAsync().ConfigureAwait(false);
            if (DateTime.UtcNow - _lastUpdateTime < TimeSpan.FromHours(1))
                return new CallResult(null);

            try
            {
                var symbolInfo = await new HyperLiquidRestClient().Api.ExchangeData.GetSpotExchangeInfoAsync().ConfigureAwait(false);
                if (!symbolInfo)
                    return symbolInfo.AsDataless();

                _spotSymbolInfo = symbolInfo.Data.Symbols;
                _lastUpdateTime = DateTime.UtcNow;
                return new CallResult(null);
            }
            finally
            {
                _semaphoreSpot.Release();
            }
        }

        public static async Task<CallResult<int>> GetSymbolIdFromName(SymbolType type, string symbolName)
        {
            if (type == SymbolType.Spot)
            {
                var update = await UpdateSpotSymbolInfoAsync().ConfigureAwait(false);
                if (!update)
                    return new CallResult<int>(update.Error!);
            }
            else
            {

            }

            var symbol = _spotSymbolInfo.SingleOrDefault(x => x.Name == symbolName);
            if (symbol == null)
                return new CallResult<int>(new ServerError("Symbol not found"));

            return new CallResult<int>(symbol.Index + (type == SymbolType.Spot ? 10000 : 0));
        }
    }
}
