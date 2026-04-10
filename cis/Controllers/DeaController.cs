using System.Security.Claims;
using CISApps.Models;
using CISApps.Models.Linkage;
using CISApps.Models.Linkage.Dea;
using cis.Models.Rest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cis.Controllers
{
    [Authorize]
    public class DeaController : Controller
    {
        private const string DeaJobId = "c2d982d4-258f-40e2-af90-e58575a1fd11";
        private const int DeaServiceId = 167;

        private readonly ILogger<DeaController> _logger;
        private readonly ApiService _apiService;

        public DeaController(ILogger<DeaController> logger, ApiService apiService)
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
                    jobID = DeaJobId,
                    data = new[]
                    {
                        new
                        {
                            serviceID = DeaServiceId,
                            query = new
                            {
                                personalID = pidText
                            }
                        }
                    }
                };

                var response = await _apiService.PostDataAsync<AggregateApiResponse>("/api/center/request/", request, lkToken);
                _logger.LogInformation("Dea aggregate response status code: {StatusCode}", _apiService.statusCode);

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

                var deaItem = AggregateResponseHelper.GetItem(response, DeaServiceId);
                if (deaItem == null)
                {
                    ViewData["error"] = "ไม่พบบริการข้อมูล ปปง. ในผลลัพธ์ที่ได้รับ";
                    return View(s);
                }

                if (deaItem.responseStatus != 200 && deaItem.responseStatus != 404)
                {
                    ViewData["error"] = $"API service error: {deaItem.responseStatus}";
                    return View(s);
                }

                if (deaItem.responseStatus == 404)
                {
                    ViewData["error"] = "ไม่พบข้อมูล ปปง.";
                    return View(s);
                }

                var dea = AggregateResponseHelper.DeserializeResponseData<DeaService>(deaItem.responseData);
                if (dea == null)
                {
                    ViewData["error"] = "ไม่สามารถแปลงข้อมูล ปปง. จาก API ได้";
                    return View(s);
                }

                s.people.dea = dea;
                _logger.LogInformation("Dea search completed for PID: {PID}", pidText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeaController.Index failed");
                ViewData["error"] = ex.Message;
            }

            return View(s);
        }
    }
}
