using Opc.Ua;

namespace UA_RESTServer.Models
{
    public class WritePayload
    {
        public string ExpandedNodeId { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        public string OPCUADataType { get; set; } = string.Empty;
    }
}
