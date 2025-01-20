using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using HyperLiquid.Net.Objects.Internal;
using System;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Converters.MessageParsing;

namespace HyperLiquid.Net.Objects.Sockets
{
    internal class HyperLiquidQuery<T> : Query<HyperLiquidSocketUpdate<T>>
    {
        public override HashSet<string> ListenerIdentifiers { get; set; }

        private string? _errorString;

        public HyperLiquidQuery(HyperLiquidSocketRequest request, string listenId, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            ListenerIdentifiers = new HashSet<string> { listenId, "error" };
        }

        public override Type? GetMessageType(IMessageAccessor message)
        {
            if (message.GetValue<string>(MessagePath.Get().Property("channel"))?.Equals("error") == true)
            {
                // Save the error message
                _errorString = message.GetValue<string>(MessagePath.Get().Property("data"));
            }

            return base.GetMessageType(message);
        }

        public override CallResult<object> Deserialize(IMessageAccessor message, Type type)
        {
            if (_errorString != null)
            {
                // Error is set, the actual model doesn't matter
                return new CallResult<object>(new HyperLiquidSocketUpdate<T>());
            }

            return base.Deserialize(message, type);
        }

        public override CallResult<HyperLiquidSocketUpdate<T>> HandleMessage(SocketConnection connection, DataEvent<HyperLiquidSocketUpdate<T>> message)
        {
            if (_errorString != null && !_errorString.StartsWith("Already subscribed:")) // Allow duplicate subscriptions
            {
                var err = _errorString;
                _errorString = null;
                return message.ToCallResult<HyperLiquidSocketUpdate<T>>(new ServerError(err));
            }

            return message.ToCallResult();
        }
    }
}
