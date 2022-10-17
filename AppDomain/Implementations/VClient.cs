using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Net.WebSockets;
namespace MiniVClient.AppDomain.Implementations
{
  public class VClient
  {
    public static void Connect(string shortLink)
    {
      var logger = Util.GetLogger();
      logger.Info("Connecting to {0}", shortLink);
      Uri uri = new Uri(shortLink);
      var host = uri.Host;
      var key = uri.PathAndQuery.Remove(0, 1);
      var wsScheme = uri.Scheme == "https" ? "wss" : "ws";
      var wsUri = String.Format("{0}://{1}/cjrpc/{2}", wsScheme, host, key);
      var wsClient = new ClientWebSocket();
      logger.Info("Requesting ssh params from websocket cinnection");
      var details = WebSocketWrapper.GetConnectionDetails(wsClient, wsUri).Result;
      logger.Info("Params received, waiting for container startup");
      Thread.Sleep(5000);
      var forwardedPort = SshTunnel.Create(host, details);
      logger.Info("SSH tunnel estabilished, forwarded port {0}", forwardedPort);
      Task.Run(() => { WebSocketWrapper.HandleEvents(wsClient); });
      logger.Info("Starting remote desktop connection");
      var rdcProcess = new Process();
      rdcProcess.StartInfo = new ProcessStartInfo("mstsc.exe", string.Format("/v:localhost:{0}", forwardedPort)); ;
      rdcProcess.Start();
      rdcProcess.WaitForExit();
      wsClient.Abort();
      logger.Info("Session closed");
    }
  }
}
