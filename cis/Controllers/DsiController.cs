using CISApps.Models;
using CISApps.Models.Api;
using CISApps.Models.Rest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace cis.Controllers
{
    public class DsiController : Controller
    {
		private readonly ILogger<DsiController> _logger;

		public DsiController(ILogger<DsiController> logger)
		{
			_logger = logger;
		}

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Searchs s, IFormCollection f)
        {
            ViewData["error"] = (f["error"].ToString() ?? "");
            try
            {
                s.setData(f);
                long pid = long.Parse(f["xyspid"].ToString() ?? "0L");
				var api = new Api();
                Error err = await Tools.SaveLog(api, $"ค้นหาทะเบียนการเดินทางด้วยเลข {pid.ToString("0 0000 00000 00 0")})");

            }
            catch (Exception ex)
            {
                ViewData["error"] = ex.Message;
            }
            return View(s);
        }
    }
}
