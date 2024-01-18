using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UaRestGateway.Server.Service;

namespace UaRestGateway.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly IUACommunicationService _mySingletonService;

        public HomeController(IUACommunicationService communicationService)
        {
            _mySingletonService = communicationService;
        }

        [HttpGet]
        // GET: HomeController
        public ActionResult Index()
        {
            return Ok("Hello World!");
        }

        // POST: HomeController/Create
        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
