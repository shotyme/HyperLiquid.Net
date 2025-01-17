using CryptoExchange.Net.Sockets;
using System;
using System.Collections.Generic;
using HyperLiquid.Net.Objects.Internal;

namespace HyperLiquid.Net.Objects.Sockets
{
    internal class HyperLiquidPingQuery : Query<HyperLiquidPong>
    {
        public override HashSet<string> ListenerIdentifiers { get; set; } = new HashSet<string> { "pong" };

        public HyperLiquidPingQuery() : base(new HyperLiquidPing(), false)
        {
            RequestTimeout = TimeSpan.FromSeconds(5);
        }
    }
}
