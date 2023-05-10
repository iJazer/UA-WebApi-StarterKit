using Opc.Ua;

namespace UA_RESTServer.Models
{
    public class CallPayload
    {
        public string ExpandedNodeId { get; set; } = string.Empty;

        public List<Variant> InputArguments { get; set; } = new();
    }
}
