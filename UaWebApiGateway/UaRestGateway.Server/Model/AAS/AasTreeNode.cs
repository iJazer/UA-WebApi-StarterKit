namespace UaRestGateway.Server.Model.AAS
{
    public class AasTreeNode
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Link { get; set; } // Optional: Direct link to AAS elements
        public List<AasTreeNode>? Children { get; set; } = new List<AasTreeNode>();
    }
}
