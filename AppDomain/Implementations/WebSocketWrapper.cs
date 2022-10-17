using System;
using System.Collections.Generic;
using System.Text;
using MiniVClient.AppDomain.Models;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
namespace MiniVClient.AppDomain.Implementations
{
  public class WebSocketWrapper
  {
    public static async Task<ConnectionDetails> GetConnectionDetails(ClientWebSocket client, string uri)
    {
      Uri serverUri = new Uri(uri);

      var cancellationToken = new CancellationTokenSource();
      cancellationToken.CancelAfter(30000);

      await client.ConnectAsync(serverUri, cancellationToken.Token);

      if(client.State != WebSocketState.Open)
      {
        throw new Exception();
      }

      WebSocketReceiveResult result;

      using (var ms = new MemoryStream())
      {
        do
        {
          var messageBuffer = WebSocket.CreateClientBuffer(1024, 16);
          result = await client.ReceiveAsync(messageBuffer, CancellationToken.None);
          ms.Write(messageBuffer.Array, messageBuffer.Offset, result.Count);
        }
        while (!result.EndOfMessage);
        var response = Encoding.UTF8.GetString(ms.ToArray());
        var deserializedResult = JsonSerializer.Deserialize<ConnectionDetails>(response);
        return deserializedResult;
      }
    }

    public static async Task HandleEvents(ClientWebSocket client)
    {
      WebSocketReceiveResult result;

      do
      {
        var messageBuffer = WebSocket.CreateClientBuffer(1024, 16);
        result = await client.ReceiveAsync(messageBuffer, CancellationToken.None);
      }
      while (true);
    }
  }
}
