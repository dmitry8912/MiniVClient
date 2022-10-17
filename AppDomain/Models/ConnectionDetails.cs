using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace MiniVClient.AppDomain.Models
{
  public class ConnectionDetails
  {
    [JsonPropertyName("host")]
    public string InternalHost { get; set; }

    [JsonPropertyName("ssh_username")]
    public string SshUsername { get; set; }

    [JsonPropertyName("ssh_password")]
    public string SshPassword { get; set; }

    [JsonPropertyName("external_port")]
    public int ExternalPort { get; set; }
  }
}
