using HyperLiquid.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperLiquid.Net.Objects.Models
{
    public record HyperLiquidOrderRequest
    {
        public SymbolType SymbolType { get; set; }
        public string Symbol { get; set; }
        public OrderSide Side { get; set; }
        public OrderType OrderType { get; set; }
        public TimeInForce? TimeInForce { get; set; }
        public decimal? Price { get; set; }
        public decimal Quantity { get; set; }
        public bool? ReduceOnly { get; set; }
        public string? ClientOrderId { get; set; }
        public decimal? TriggerPrice { get; set; }
        public TpSlType? TpSlType { get; set; }
        public TpSlGrouping? TpSlGrouping { get; set; }

        public HyperLiquidOrderRequest(
            SymbolType symbolType,
            string symbol,
            OrderSide side,
            OrderType orderType,
            decimal quantity,
            decimal? price = null,
            TimeInForce? timeInForce = null,
            bool? reduceOnly = null,
            decimal? triggerPrice = null,
            TpSlType? tpSlType = null,
            string? clientOrderId = null
            )
        {
            SymbolType = symbolType;
            Symbol = symbol;
            Side = side;
            OrderType = orderType;
            Quantity = quantity;
            Price = price;
            ReduceOnly = reduceOnly;
            TriggerPrice = triggerPrice;
            TpSlType = tpSlType;
            ClientOrderId = clientOrderId;
            TimeInForce = timeInForce;
        }
    }
}
