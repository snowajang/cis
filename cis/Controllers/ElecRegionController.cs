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
    public class ElecRegionController : Controller
    {
        private const string ElecRegionJobId = "12b7c996-3d85-4834-93eb-fa79ef9a2b92";
        private const int ElecRegionServiceId = 129;

        private readonly ILogger<ElecRegionController> _logger;
        private readonly ApiService _apiService;

        public ElecRegionController(ILogger<ElecRegionController> logger, ApiService apiService)
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
                    jobID = ElecRegionJobId,
                    data = new[]
                    {
                        new
                        {
                            serviceID = ElecRegionServiceId,
                            query = new
                            {
                                personalID = pidText
                            }
                        }
                    }
                };

                var response = await _apiService.PostDataAsync<AggregateApiResponse>("/api/center/request/", request, lkToken);
                _logger.LogInformation("ElecRegion aggregate response status code: {StatusCode}", _apiService.statusCode);

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

                var elecRegionItem = AggregateResponseHelper.GetItem(response, ElecRegionServiceId);
                if (elecRegionItem == null)
                {
                    ViewData["error"] = "ไม่พบบริการข้อมูลไฟฟ้าส่วนภูมิภาคในผลลัพธ์ที่ได้รับ";
                    return View(s);
                }

                if (elecRegionItem.responseStatus != 200 && elecRegionItem.responseStatus != 404)
                {
                    ViewData["error"] = $"API service error: {elecRegionItem.responseStatus}";
                    return View(s);
                }

                if (elecRegionItem.responseStatus == 404)
                {
                    ViewData["error"] = "ไม่พบข้อมูลไฟฟ้าส่วนภูมิภาค";
                    return View(s);
                }

                var elecRegion = AggregateResponseHelper.DeserializeResponseData<ElecRegionService>(elecRegionItem.responseData);
                if (elecRegion == null)
                {
                    ViewData["error"] = "ไม่สามารถแปลงข้อมูลไฟฟ้าส่วนภูมิภาคจาก API ได้";
                    return View(s);
                }

                s.people.elecregion = elecRegion;

                _logger.LogInformation("ElecRegion search completed for PID: {PID}", pidText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ElecRegionController.Index failed");
                ViewData["error"] = ex.Message;
            }

            return View(s);
        }
    }
}
