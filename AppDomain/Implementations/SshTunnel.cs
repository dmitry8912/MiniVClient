using System;
using System.Collections.Generic;
using System.Text;
using MiniVClient.AppDomain.Models;
using Renci.SshNet;
using MiniVClient.AppDomain.Implementations;

namespace MiniVClient.AppDomain.Implementations
{
  public class SshTunnel
  {
    private static SshClient client;
    private static ForwardedPortLocal forwardedPort;
    public static int Create(string sshHost, ConnectionDetails details)
    {
      client = new SshClient(new ConnectionInfo(sshHost, details.ExternalPort, details.SshUsername, new AuthenticationMethod[1]
      {
        new PasswordAuthenticationMethod(details.SshUsername, details.SshPassword)
      }));
      client.Connect();
      int freePort = Util.GetFreePort(new Random().Next(12000, 13000));
      forwardedPort = new ForwardedPortLocal("localhost", Convert.ToUInt32(freePort), details.InternalHost, 3389U);
      client.AddForwardedPort(forwardedPort);
      forwardedPort.Start();
      return freePort;
    }
  }
}
