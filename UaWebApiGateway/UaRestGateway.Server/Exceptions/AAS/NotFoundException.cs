
using System;

namespace UaRestGateway.Server.Exceptions.AAS
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {

        }
    }
}
