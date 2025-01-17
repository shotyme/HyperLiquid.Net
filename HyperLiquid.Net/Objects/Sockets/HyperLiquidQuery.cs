using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using HyperLiquid.Net.Objects.Internal;

namespace HyperLiquid.Net.Objects.Sockets
{
    internal class HyperLiquidQuery<T> : Query<HyperLiquidSocketUpdate<T>>
    {
        public override HashSet<string> ListenerIdentifiers { get; set; }

        public HyperLiquidQuery(HyperLiquidSocketRequest request, string listenId, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            ListenerIdentifiers = new HashSet<string> { listenId };
        }

        public override CallResult<HyperLiquidSocketUpdate<T>> HandleMessage(SocketConnection connection, DataEvent<HyperLiquidSocketUpdate<T>> message)
        {
            return message.ToCallResult();
        }
    }
}
