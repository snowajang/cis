using System.Security.Claims;
using CISApps.Models;
using CISApps.Models.Linkage;
using CISApps.Models.Linkage.Nsho;
using cis.Models.Rest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cis.Controllers
{
    [Authorize]
    public class NshoController : Controller
    {
        private const string NshoJobId = "c0906a0b-1906-4d00-985a-9c421562b34e";
        private const int NshoServiceId = 98;

        private readonly ILogger<NshoController> _logger;
        private readonly ApiService _apiService;

        public NshoController(ILogger<NshoController> logger, ApiService apiService)
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
                    jobID = NshoJobId,
                    data = new[]
                    {
                        new
                        {
                            serviceID = NshoServiceId,
                            query = new
                            {
                                personalID = pidText
                            }
                        }
                    }
                };

                var response = await _apiService.PostDataAsync<AggregateApiResponse>("/api/center/request/", request, lkToken);
                _logger.LogInformation("Nsho aggregate response status code: {StatusCode}", _apiService.statusCode);

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

                var nshoItem = AggregateResponseHelper.GetItem(response, NshoServiceId);
                if (nshoItem == null)
                {
                    ViewData["error"] = "ไม่พบบริการข้อมูลสิทธิรักษาพยาบาลในผลลัพธ์ที่ได้รับ";
                    return View(s);
                }

                if (nshoItem.responseStatus != 200 && nshoItem.responseStatus != 404)
                {
                    ViewData["error"] = $"API service error: {nshoItem.responseStatus}";
                    return View(s);
                }

                if (nshoItem.responseStatus == 404)
                {
                    ViewData["error"] = "ไม่พบข้อมูลสิทธิรักษาพยาบาล";
                    return View(s);
                }

                var nsho = AggregateResponseHelper.DeserializeResponseData<NshoService>(nshoItem.responseData);
                if (nsho == null)
                {
                    ViewData["error"] = "ไม่สามารถแปลงข้อมูลสิทธิรักษาพยาบาลจาก API ได้";
                    return View(s);
                }

                s.people.nsho = nsho;

                _logger.LogInformation("Nsho search completed for PID: {PID}", pidText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "NshoController.Index failed");
                ViewData["error"] = ex.Message;
            }

            return View(s);
        }
    }
}
