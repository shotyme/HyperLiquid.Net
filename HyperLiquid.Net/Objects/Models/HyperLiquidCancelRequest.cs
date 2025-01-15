using HyperLiquid.Net.Enums;

namespace HyperLiquid.Net.Objects.Models
{
    /// <summary>
    /// Cancel request
    /// </summary>
    public record HyperLiquidCancelRequest
    {
        /// <summary>
        /// Type of the symbol
        /// </summary>
        public SymbolType SymbolType { get; set; }
        /// <summary>
        /// Symbol name
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Order id
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public HyperLiquidCancelRequest(SymbolType symbolType, string symbol, long orderId)
        {
            SymbolType = symbolType;
            Symbol = symbol;
            OrderId = orderId;
        }
    }

    /// <summary>
    /// Cancel request
    /// </summary>
    public record HyperLiquidCancelByClientOrderIdRequest
    {
        /// <summary>
        /// Type of the symbol
        /// </summary>
        public SymbolType SymbolType { get; set; }
        /// <summary>
        /// Symbol name
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Client order id
        /// </summary>
        public string OrderId { get; set; } = string.Empty;

        /// <summary>
        /// ctor
        /// </summary>
        public HyperLiquidCancelByClientOrderIdRequest(SymbolType symbolType, string symbol, string orderId)
        {
            SymbolType = symbolType;
            Symbol = symbol;
            OrderId = orderId;
        }
    }
}
