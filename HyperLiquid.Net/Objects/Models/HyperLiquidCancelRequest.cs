using HyperLiquid.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperLiquid.Net.Objects.Models
{
    public record HyperLiquidCancelRequest
    {
        public SymbolType SymbolType { get; set; }
        public string Symbol { get; set; }
        public long OrderId { get; set; }

        public HyperLiquidCancelRequest(SymbolType symbolType, string symbol, long orderId)
        {
            SymbolType = symbolType;
            Symbol = symbol;
            OrderId = orderId;
        }
    }

    public record HyperLiquidCancelByClientOrderIdRequest
    {
        public SymbolType SymbolType { get; set; }
        public string Symbol { get; set; }
        public string OrderId { get; set; }

        public HyperLiquidCancelByClientOrderIdRequest(SymbolType symbolType, string symbol, string orderId)
        {
            SymbolType = symbolType;
            Symbol = symbol;
            OrderId = orderId;
        }
    }
}
