using CISApps.Models;
using CISApps.Models.Api;
using CISApps.Models.Rest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace cis.Controllers
{
	public class NshoController : Controller
    {
		private readonly ILogger<NshoController> _logger;

		public NshoController(ILogger<NshoController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			Searchs s = new Searchs();
			return View(s);
		}

		[HttpPost]
		public async Task<IActionResult> Index(Searchs s, IFormCollection f)
		{
			s.errorMessage = (f["error"].ToString() ?? "");
			try
			{
				s.setData(f);
                long pid = long.Parse(f["xyspid"].ToString() ?? "0L");
				var api = new Api();
                Error err = await Tools.SaveLog(api, $"ค้นหาทะเบียนการเดินทางด้วยเลข {pid.ToString("0 0000 00000 00 0")})");

            }
            catch (Exception ex)
			{
				s.errorMessage = ex.Message;
			}
			return View(s);
		}
	}
}
