namespace HyperLiquid.Net.Objects
{
    /// <summary>
    /// Api addresses
    /// </summary>
    public class HyperLiquidApiAddresses
    {
        /// <summary>
        /// The address used by the HyperLiquidRestClient for the API
        /// </summary>
        public string RestClientAddress { get; set; } = "";
        /// <summary>
        /// The address used by the HyperLiquidSocketClient for the websocket API
        /// </summary>
        public string SocketClientAddress { get; set; } = "";

        /// <summary>
        /// The default addresses to connect to the HyperLiquid API
        /// </summary>
        public static HyperLiquidApiAddresses Default = new HyperLiquidApiAddresses
        {
            RestClientAddress = "https://api.hyperliquid.xyz",
            SocketClientAddress = "wss://api.hyperliquid.xyz"
        };

        /// <summary>
        /// The addresses to connect to the HyperLiquid Testnet API
        /// </summary>
        public static HyperLiquidApiAddresses Testnet = new HyperLiquidApiAddresses
        {
            RestClientAddress = "https://api.hyperliquid-testnet.xyz",
            SocketClientAddress = "wss://api.hyperliquid-testnet.xyz"
        };
    }
}
