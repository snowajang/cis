using System.Security.Claims;
using CISApps.Models;
using CISApps.Models.Linkage;
using CISApps.Models.Linkage.Electric;
using cis.Models.Rest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cis.Controllers
{
    [Authorize]
    public class ElecCenterController : Controller
    {
        private const string ElecCenterJobId = "866399a1-8f5b-423b-8a19-aabdc6a1914b";
        private const int ElecCenterServiceId = 156;

        private readonly ILogger<ElecCenterController> _logger;
        private readonly ApiService _apiService;

        public ElecCenterController(ILogger<ElecCenterController> logger, ApiService apiService)
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

                ViewData["spid"] = pidText;

                var lkToken = User.FindFirst("lktoken")?.Value;
                if (string.IsNullOrWhiteSpace(lkToken))
                {
                    ViewData["error"] = "ไม่พบ token สำหรับเรียกใช้งานระบบ กรุณาเข้าสู่ระบบใหม่";
                    return View(s);
                }

                var request = new
                {
                    jobID = ElecCenterJobId,
                    data = new[]
                    {
                        new
                        {
                            serviceID = ElecCenterServiceId,
                            query = new
                            {
                                personalID = pidText
                            }
                        }
                    }
                };

                var response = await _apiService.PostDataAsync<AggregateApiResponse>("/api/center/request/", request, lkToken);
                _logger.LogInformation("ElecCenter aggregate response status code: {StatusCode}", _apiService.statusCode);

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

                var elecCenterItem = AggregateResponseHelper.GetItem(response, ElecCenterServiceId);
                if (elecCenterItem == null)
                {
                    ViewData["error"] = "ไม่พบบริการข้อมูลไฟฟ้านครหลวงในผลลัพธ์ที่ได้รับ";
                    return View(s);
                }

                if (elecCenterItem.responseStatus != 200 && elecCenterItem.responseStatus != 404)
                {
                    ViewData["error"] = $"API service error: {elecCenterItem.responseStatus}";
                    return View(s);
                }

                if (elecCenterItem.responseStatus == 404)
                {
                    ViewData["error"] = "ไม่พบข้อมูลไฟฟ้านครหลวง";
                    return View(s);
                }

                var elecCenter = AggregateResponseHelper.DeserializeResponseData<ElecCenterService>(elecCenterItem.responseData);
                if (elecCenter == null)
                {
                    ViewData["error"] = "ไม่สามารถแปลงข้อมูลไฟฟ้านครหลวงจาก API ได้";
                    return View(s);
                }

                s.people.eleccenter = elecCenter;

                _logger.LogInformation("ElecCenter search completed for PID: {PID}", pidText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ElecCenterController.Index failed");
                ViewData["error"] = ex.Message;
            }

            return View(s);
        }
    }
}
