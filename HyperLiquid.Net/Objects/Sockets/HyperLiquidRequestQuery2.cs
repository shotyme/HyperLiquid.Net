using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using HyperLiquid.Net.Objects.Internal;
using System;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using CryptoExchange.Net;
using HyperLiquid.Net.Clients.BaseApi;
using CryptoExchange.Net.Objects.Errors;
using System.Text.Json.Serialization;
using HyperLiquid.Net.Objects.Models;

namespace HyperLiquid.Net.Objects.Sockets
{
    internal record HyperLiquidSocketMessage2<T>
    {
        [JsonPropertyName("channel")]
        public string Channel { get; set; } = string.Empty;
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;
    }

    internal record HyperLiquidSocketEnvelope2<T>
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("response")]
        public T Response { get; set; } = default!;
    }

    internal record HyperLiquidSocketResponse2<T>
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("payload")]
        public T Payload { get; set; } = default!;
    }

    internal record HyperLiquidSocketPayload2<T>
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("response")]
        public T Response { get; set; } = default!;
    }


    internal class HyperLiquidRequestQuery2<TResponse> : Query<TResponse>
    {
        public HyperLiquidRequestQuery2(
            HyperLiquidSocketClientApi client,
            string method,
            string type,
            Parameters request,
            int weight = 1) 
            : base(
                new HyperLiquidRequest
                {
                    Id = ExchangeHelpers.NextId(), 
                    Method = method, 
                    Request = new HyperLiquidRequestWrapper { Type = type, Payload = request } 
                }, 
                true,
                weight)
        {
            client.AuthenticationProvider!.ProcessRequest(client, request);

            MessageRouter = MessageRouter.Create([
                MessageRoute.CreateForQuery<HyperLiquidSocketMessage2<HyperLiquidSocketEnvelope2<HyperLiquidSocketResponse2<HyperLiquidSocketPayload2<TResponse>>>>, TResponse>(((HyperLiquidRequest)Request).Id.ToString(), HandleMessage),
            ]);            
        }

        public CallResult<TResponse> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, HyperLiquidSocketMessage2<HyperLiquidSocketEnvelope2<HyperLiquidSocketResponse2<HyperLiquidSocketPayload2<TResponse>>>> message)
        {
            if (!message.Data.Response.Payload.Status.Equals("ok"))
            {
                if (message.Data.Response.Payload.Status.Equals("err")
                    && message.Data.Response.Payload.Response is HyperLiquidDefault hlDefault)
                {
                    return CallResult<TResponse>.Fail(new ServerError(ErrorInfo.Unknown with { Message = hlDefault.Type }), originalData);
                }

                return CallResult<TResponse>.Fail(new ServerError(ErrorInfo.Unknown with { Message = message.Data.Response.Payload.Status }), originalData);
            }
            return CallResult<TResponse>.Ok(message.Data.Response.Payload.Response, originalData);
        }

    }
}
