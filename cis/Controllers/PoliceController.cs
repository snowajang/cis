using System.Security.Claims;
using CISApps.Models;
using CISApps.Models.Linkage;
using CISApps.Models.Linkage.Police;
using cis.Models.Rest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cis.Controllers
{
    [Authorize]
    public class PoliceController : Controller
    {
        private const string PoliceJobId = "5de1a718-2dc6-4232-938c-d0cf58470350";
        private const int PoliceServiceId = 157;

        private readonly ILogger<PoliceController> _logger;
        private readonly ApiService _apiService;

        public PoliceController(ILogger<PoliceController> logger, ApiService apiService)
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

                if (!long.TryParse(pidText, out var pid) || pidText.Length != 13)
                {
                    ViewData["error"] = "เลขประจำตัวประชาชนไม่ถูกต้อง";
                    return View(s);
                }

                ViewData["spid"] = pidText;

                var lkToken = User.FindFirst("lktoken")?.Value;
                if (string.IsNullOrWhiteSpace(lkToken))
                {
                    ViewData["error"] = "ไม่พบ token สำหรับเรียกใช้งานระบบ กรุณาเข้าสู่ระบบใหม่";
                    return View(s);
                }

                var request = new
                {
                    jobID = PoliceJobId,
                    data = new[]
                    {
                        new
                        {
                            serviceID = PoliceServiceId,
                            query = new
                            {
                                personalID = pidText
                            }
                        }
                    }
                };

                var response = await _apiService.PostDataAsync<AggregateApiResponse>("/api/center/request/", request, lkToken);
                _logger.LogInformation("Police aggregate response status code: {StatusCode}", _apiService.statusCode);

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

                var policeItem = AggregateResponseHelper.GetItem(response, PoliceServiceId);
                if (policeItem == null)
                {
                    ViewData["error"] = "ไม่พบบริการข้อมูลหมายจับในผลลัพธ์ที่ได้รับ";
                    return View(s);
                }

                if (policeItem.responseStatus != 200 && policeItem.responseStatus != 404)
                {
                    ViewData["error"] = $"API service error: {policeItem.responseStatus}";
                    return View(s);
                }

                if (policeItem.responseStatus == 404)
                {
                    ViewData["error"] = "ไม่พบข้อมูลหมายจับ";
                    return View(s);
                }

                var police = AggregateResponseHelper.DeserializeResponseData<PoliceService>(policeItem.responseData);
                if (police == null)
                {
                    ViewData["error"] = "ไม่สามารถแปลงข้อมูลหมายจับจาก API ได้";
                    return View(s);
                }

                s.people.police = police;

                _logger.LogInformation("Police search completed for PID: {PID}", pidText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PoliceController.Index failed");
                ViewData["error"] = ex.Message;
            }

            return View(s);
        }
    }
}
