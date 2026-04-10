using System.Security.Claims;
using CISApps.Models;
using CISApps.Models.Linkage;
using CISApps.Models.Linkage.Gun;
using cis.Models.Rest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cis.Controllers
{
    [Authorize]
    public class GunController : Controller
    {
        private const string GunCardJobId = "9270731e-ddab-4968-8788-d56b752db44c";
        private const int GunCardServiceId = 104;

        private const string GunNumberJobId = "f76718ed-c211-4aab-ba4c-65ae6433fcb8";
        private const int GunNumberServiceId = 155;

        private readonly ILogger<GunController> _logger;
        private readonly ApiService _apiService;

        public GunController(ILogger<GunController> logger, ApiService apiService)
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
                var pidText = f["xyspid"].ToString();
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
                    jobID = GunCardJobId,
                    data = new[]
                    {
                        new
                        {
                            serviceID = GunCardServiceId,
                            query = new
                            {
                                personalID = pidText
                            }
                        }
                    }
                };

                var response = await _apiService.PostDataAsync<AggregateApiResponse>("/api/center/request/", request, lkToken);
                _logger.LogInformation("GunController.Index aggregate response status code: {StatusCode}", _apiService.statusCode);

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

                var gunCardItem = AggregateResponseHelper.GetItem(response, GunCardServiceId);
                if (gunCardItem == null)
                {
                    ViewData["error"] = "ไม่พบบริการข้อมูลใบอนุญาต ป.4 ในผลลัพธ์ที่ได้รับ";
                    return View(s);
                }

                if (gunCardItem.responseStatus != 200 && gunCardItem.responseStatus != 404)
                {
                    ViewData["error"] = $"API service error: {gunCardItem.responseStatus}";
                    return View(s);
                }

                if (gunCardItem.responseStatus == 404)
                {
                    ViewData["error"] = "ไม่พบข้อมูลใบอนุญาต ป.4";
                    return View(s);
                }

                var gunCard = AggregateResponseHelper.DeserializeResponseData<GunCardService>(gunCardItem.responseData);
                if (gunCard == null)
                {
                    ViewData["error"] = "ไม่สามารถแปลงข้อมูลใบอนุญาต ป.4 จาก API ได้";
                    return View(s);
                }

                s.people.guncard = gunCard;

                _logger.LogInformation("GunController.Index search completed for PID: {PID}", pidText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GunController.Index failed");
                ViewData["error"] = ex.Message;
            }

            return View(s);
        }

        [HttpGet("Gun/GUN")]
        public IActionResult GUN()
        {
            return View("Gun", new Searchs());
        }

        [HttpPost("Gun/GUN")]
        public async Task<IActionResult> GUN(Searchs s, IFormCollection f)
        {
            s ??= new Searchs();
            s.people ??= new People();
            ViewData["error"] = f["error"].ToString() ?? "";

            try
            {
                var pidText = f["xyspid"].ToString();
                if (string.IsNullOrWhiteSpace(pidText))
                {
                    ViewData["error"] = "กรุณากรอกเลขประจำตัวประชาชน";
                    return View("Gun", s);
                }

                if (!long.TryParse(pidText, out _) || pidText.Length != 13)
                {
                    ViewData["error"] = "เลขประจำตัวประชาชนไม่ถูกต้อง";
                    return View("Gun", s);
                }

                ViewData["spid"] = pidText;

                var lkToken = User.FindFirst("lktoken")?.Value;
                if (string.IsNullOrWhiteSpace(lkToken))
                {
                    ViewData["error"] = "ไม่พบ token สำหรับเรียกใช้งานระบบ กรุณาเข้าสู่ระบบใหม่";
                    return View("Gun", s);
                }

                var request = new
                {
                    jobID = GunNumberJobId,
                    data = new[]
                    {
                        new
                        {
                            serviceID = GunNumberServiceId,
                            query = new
                            {
                                personalID = pidText
                            }
                        }
                    }
                };

                var response = await _apiService.PostDataAsync<AggregateApiResponse>("/api/center/request/", request, lkToken);
                _logger.LogInformation("GunController.GUN aggregate response status code: {StatusCode}", _apiService.statusCode);

                if (_apiService.statusCode != 200 && _apiService.statusCode != 404)
                {
                    ViewData["error"] = $"API error: {_apiService.statusCode} {response?.errorMessage ?? "Unknown error"}";
                    return View("Gun", s);
                }

                if (!AggregateResponseHelper.HasData(response))
                {
                    ViewData["error"] = string.IsNullOrWhiteSpace(ViewData["error"]?.ToString())
                        ? "ไม่พบข้อมูลจาก API"
                        : ViewData["error"]?.ToString();
                    return View("Gun", s);
                }

                var gunItem = AggregateResponseHelper.GetItem(response, GunNumberServiceId);
                if (gunItem == null)
                {
                    ViewData["error"] = "ไม่พบบริการข้อมูลใบอนุญาต ป.4 (หมายเลขอาวุธ) ในผลลัพธ์ที่ได้รับ";
                    return View("Gun", s);
                }

                if (gunItem.responseStatus != 200 && gunItem.responseStatus != 404)
                {
                    ViewData["error"] = $"API service error: {gunItem.responseStatus}";
                    return View("Gun", s);
                }

                if (gunItem.responseStatus == 404)
                {
                    ViewData["error"] = "ไม่พบข้อมูลใบอนุญาต ป.4 (หมายเลขอาวุธ)";
                    return View("Gun", s);
                }

                var gun = AggregateResponseHelper.DeserializeResponseData<GunService>(gunItem.responseData);
                if (gun == null)
                {
                    ViewData["error"] = "ไม่สามารถแปลงข้อมูลใบอนุญาต ป.4 (หมายเลขอาวุธ) จาก API ได้";
                    return View("Gun", s);
                }

                s.people.gun = gun;
                ViewData["mode"] = "gun-number";

                _logger.LogInformation("GunController.GUN search completed for PID: {PID}", pidText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GunController.GUN failed");
                ViewData["error"] = ex.Message;
            }

            return View("Gun", s);
        }
    }
}
