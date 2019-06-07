using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace CarWashBooking.Extensions
{

    internal class OnLineUsersMiddelWare
    {
        private static ConcurrentBag<string> _bookingUsers;


        public OnLineUsersMiddelWare(RequestDelegate next)
        {
            if (_bookingUsers == null)
                _bookingUsers = new ConcurrentBag<string>();
            this.Next = next;
        }
        public RequestDelegate Next { get; private set; }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await HandleUserBooking(webSocket, CancellationToken.None);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await Next(context);
            }
        }


        private async Task HandleUserBooking(WebSocket socket, CancellationToken cancelToken)
        {
            WebSocketReceiveResult response;
            var message = new List<byte>();

            var buffer = new byte[4096];
            do
            {
                response = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancelToken);
                message.AddRange(new ArraySegment<byte>(buffer, 0, response.Count));
            } while (!response.EndOfMessage);

            var userName = System.Text.Encoding.Default.GetString(message.ToArray());


            if (!string.IsNullOrEmpty(userName))
            {
                if (!_bookingUsers.Contains(userName) && !string.IsNullOrEmpty(userName))
                    _bookingUsers.Add(userName);
            }

            while (!response.CloseStatus.HasValue)
            {
                string numberInfo = _bookingUsers.Count.ToString();
                var bufferInfo = System.Text.Encoding.UTF8.GetBytes(numberInfo);
                await socket.SendAsync(new ArraySegment<byte>(bufferInfo, 0, bufferInfo.Length), response.MessageType, response.EndOfMessage, CancellationToken.None);
                response = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await socket.CloseAsync(response.CloseStatus.Value, response.CloseStatusDescription, CancellationToken.None);
            if (_bookingUsers.Contains(userName))
                _bookingUsers.TryTake(out userName);
        }
    }
}