namespace HyperLiquid.Net
{
    /// <summary>
    /// Keys of the request definitions of the info endpoint (TvHub #723). All info reads post to the same path, so the default key
    /// of the definition cache (path, method, base address) gave every read type of one client class the weight of the first read
    /// in the process. The key now holds the info type, and it keeps the base address, so a testnet and a mainnet client never share
    /// a definition.
    /// </summary>
    internal static class HyperLiquidRequestKeys
    {
        /// <summary>Returns the definition key of an info read of the given type on the given base address.</summary>
        public static string Info(string type, string baseAddress) => "info:" + type + "@" + baseAddress;
    }
}
