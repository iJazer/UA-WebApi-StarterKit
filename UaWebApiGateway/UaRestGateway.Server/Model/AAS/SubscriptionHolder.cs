using Opc.Ua.Client;
using System.Net.WebSockets;

namespace UaRestGateway.Server.Model.AAS
{
    internal class SubscriptionHolder
    {
        internal MonitoredItem Item { get; set; }
        internal WebSocket Socket { get; set; }

        internal string SessionId { get; set; }
    }
}
