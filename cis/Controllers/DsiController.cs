using System.Security.Claims;
using CISApps.Models;
using CISApps.Models.Linkage;
using CISApps.Models.Linkage.Dsi;
using cis.Models.Rest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cis.Controllers
{
    [Authorize]
    public class DsiController : Controller
    {
        private const string DsiJobId = "b99be68c-81f9-421d-a932-5920aa8f0719";
        private const int DsiServiceId = 176;

        private readonly ILogger<DsiController> _logger;
        private readonly ApiService _apiService;

        public DsiController(ILogger<DsiController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View(new Searchs());
        }

        [HttpPost]
        public async Task<IActionResult> Index(Searchs s, IFormCollection f)
        {
            s ??= new Searchs();
            s.people ??= new People();
            ViewData["error"] = f["error"].ToString() ?? "";

            try
            {
                var pidText = f["spid"].ToString();
                ViewData["spid"] = pidText;
                if (string.IsNullOrWhiteSpace(pidText))
                {
                    ViewData["error"] = "กรุณากรอกเลขประจำตัวประชาชน";
                    return View(s);
                }

                if (!long.TryParse(pidText, out _) || pidText.Length != 13)
                {
                    ViewData["error"] = "เลขประจำตัวประชาชนไม่ถูกต้อง";
                    return View(s);
                }

                var lkToken = User.FindFirst("lktoken")?.Value;
                if (string.IsNullOrWhiteSpace(lkToken))
                {
                    ViewData["error"] = "ไม่พบ token สำหรับเรียกใช้งานระบบ กรุณาเข้าสู่ระบบใหม่";
                    return View(s);
                }

                var request = new
                {
                    jobID = DsiJobId,
                    data = new[]
                    {
                        new
                        {
                            serviceID = DsiServiceId,
                            query = new
                            {
                                personalID = pidText
                            }
                        }
                    }
                };

                var response = await _apiService.PostDataAsync<AggregateApiResponse>("/api/center/request/", request, lkToken);
                _logger.LogInformation("Dsi aggregate response status code: {StatusCode}", _apiService.statusCode);

                if (_apiService.statusCode != 200 && _apiService.statusCode != 404)
                {
                    ViewData["error"] = $"API error: {_apiService.statusCode} {response?.errorMessage ?? "Unknown error"}";
                    return View(s);
                }

                if (!AggregateResponseHelper.HasData(response))
                {
                    ViewData["error"] = string.IsNullOrWhiteSpace(ViewData["error"]?.ToString())
                        ? "ไม่พบข้อมูลจาก API"
                        : ViewData["error"]?.ToString();
                    return View(s);
                }

                var dsiItem = AggregateResponseHelper.GetItem(response, DsiServiceId);
                if (dsiItem == null)
                {
                    ViewData["error"] = "ไม่พบบริการข้อมูล DSI ในผลลัพธ์ที่ได้รับ";
                    return View(s);
                }

                if (dsiItem.responseStatus != 200 && dsiItem.responseStatus != 404)
                {
                    ViewData["error"] = $"API service error: {dsiItem.responseStatus}";
                    return View(s);
                }

                if (dsiItem.responseStatus == 404)
                {
                    ViewData["error"] = "ไม่พบข้อมูล DSI";
                    return View(s);
                }

                var dsi = AggregateResponseHelper.DeserializeResponseData<DsiService>(dsiItem.responseData);
                if (dsi == null)
                {
                    ViewData["error"] = "ไม่สามารถแปลงข้อมูล DSI จาก API ได้";
                    return View(s);
                }

                s.people.dsi = dsi;

                _logger.LogInformation("Dsi search completed for PID: {PID}", pidText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DsiController.Index failed");
                ViewData["error"] = ex.Message;
            }

            return View(s);
        }
    }
}
