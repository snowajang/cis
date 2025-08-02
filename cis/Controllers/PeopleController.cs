using Microsoft.AspNetCore.Mvc;
using CISApps.Models;

namespace cis.Controllers
{ 
    public class PeopleController : Controller
    {
        private readonly ILogger<PeopleController> _logger;

        public PeopleController(ILogger<PeopleController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PID()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PID(IFormCollection f)
        {
            return View();
        }

        public IActionResult NAME()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NAME(IFormCollection f)
        {
            return View();
        }

        public IActionResult OLDNAME()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OLDNAME(IFormCollection f)
        {
            return View();
        }
        
        public IActionResult HOUSE()
        {
            return View();
        }

        [HttpPost]
        public IActionResult HOUSE(IFormCollection f)
        {
            return View();
        }
        
        public IActionResult ALIEN()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ALIEN(IFormCollection f)
        {
            return View();
        }
        
    }

}
