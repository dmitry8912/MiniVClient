using System;
using System.Text.Json;
using MiniVClient.AppDomain.Implementations;
namespace MiniVClient
{
  class Program
  {
    static void Main(string[] args)
    {
      VClient.Connect(MiniVClient.Properties.Resources.EndpointURL);
      Console.WriteLine("Press any key to exit...");
      Console.ReadLine();
    }
  }
}
