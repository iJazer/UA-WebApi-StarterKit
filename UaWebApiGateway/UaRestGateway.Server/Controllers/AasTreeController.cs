using Microsoft.AspNetCore.Mvc;
using AasCore.Aas3_0;
using System.Collections.Generic;
using UaRestGateway.Server.Service.AAS;

namespace UaRestGateway.Server.Controllers
{
    

    [ApiController]
    [Route("api/aas")]
    public class AasController : ControllerBase
    {
        private readonly IAasTreeService _aasTreeService;
        private readonly IAASCommunicationService _aasCommunicationService;

        public AasController(IAasTreeService aasTreeService, IAASCommunicationService aasCommunicationService)
        {
            _aasTreeService = aasTreeService;
            _aasCommunicationService = aasCommunicationService;
        }

        [HttpGet("tree")]
        public IActionResult GetAasTree()
        {
            // Load AAS from database, file, or service
            AssetAdministrationShell aas = (AssetAdministrationShell)_aasCommunicationService.AssetAdministrationShells.First(); // You need to implement this

            var tree = _aasTreeService.ConvertAasToTree(aas);
            return Ok(tree);
        }
    }

}
