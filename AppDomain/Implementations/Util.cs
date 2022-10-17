using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using NLog;
using NLog.Targets;
namespace MiniVClient.AppDomain.Implementations
{
  public class Util
  {
    public static int GetFreePort(int startingPort = 12000)
    {
      List<int> intList = new List<int>();
      IPGlobalProperties globalProperties = IPGlobalProperties.GetIPGlobalProperties();
      TcpConnectionInformation[] activeTcpConnections = globalProperties.GetActiveTcpConnections();
      intList.AddRange(((IEnumerable<TcpConnectionInformation>)activeTcpConnections).Where<TcpConnectionInformation>((Func<TcpConnectionInformation, bool>)(n => n.LocalEndPoint.Port >= startingPort)).Select<TcpConnectionInformation, int>((Func<TcpConnectionInformation, int>)(n => n.LocalEndPoint.Port)));
      IPEndPoint[] activeTcpListeners = globalProperties.GetActiveTcpListeners();
      intList.AddRange(((IEnumerable<IPEndPoint>)activeTcpListeners).Where<IPEndPoint>((Func<IPEndPoint, bool>)(n => n.Port >= startingPort)).Select<IPEndPoint, int>((Func<IPEndPoint, int>)(n => n.Port)));
      IPEndPoint[] activeUdpListeners = globalProperties.GetActiveUdpListeners();
      intList.AddRange(((IEnumerable<IPEndPoint>)activeUdpListeners).Where<IPEndPoint>((Func<IPEndPoint, bool>)(n => n.Port >= startingPort)).Select<IPEndPoint, int>((Func<IPEndPoint, int>)(n => n.Port)));
      intList.Sort();
      for (int freePort = startingPort; freePort < (int)ushort.MaxValue; ++freePort)
      {
        if (!intList.Contains(freePort))
          return freePort;
      }
      return 0;
    }
    public static Logger GetLogger()
    {
      ConsoleTarget target = new ConsoleTarget();
      target.Layout = "${date:format=HH\\:MM\\:ss} ${logger} ${message}";
      NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Debug);

      Logger logger = LogManager.GetLogger("app");
      return logger;
    }
  }
}
