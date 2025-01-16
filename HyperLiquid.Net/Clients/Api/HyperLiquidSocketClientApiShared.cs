using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Text;
using HyperLiquid.Net.Interfaces.Clients.Api;

namespace HyperLiquid.Net.Clients.Api
{
    internal partial class HyperLiquidSocketClientApi : IHyperLiquidSocketClientApiShared
    {
        public string Exchange => "HyperLiquid";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot, TradingMode.PerpetualLinear };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();
    }
}
