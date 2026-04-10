using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using CISApps.Models;
using CISApps.Models.Linkage;
using CISApps.Models.Linkage.Department;
using CISApps.Models.Linkage.Nsho;
using cis.Models.Rest;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace cis.Controllers
{
    [Authorize]
    public class PeopleController : Controller
    {
        private readonly ILogger<PeopleController> _logger;
        private readonly ApiService api;
        public PeopleController(ILogger<PeopleController> logger, ApiService apiService)
        {
            _logger = logger;
            api = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PID()
        {
            return View(new Searchs());
        }

        [HttpPost]
        public async Task<IActionResult> PID(IFormCollection f)
        {
            if (f == null || !f.ContainsKey("spid"))
            {
                var sm = new Searchs();
                sm.errorMessage = "กรุณากรอกเลขประจำตัวประชาชน";
                return View(sm);
            }
            var s = new Searchs();
            s.errorMessage = f["error"].ToString() ?? "";
            _logger.LogInformation("Received PID search request with data: {error} {pid}", f["error"].ToString(), f["spid"].ToString());
            try
            {
                // s.setData(f);

                String pidText = f["spid"].ToString();
                long pid = 0L;
                if (!long.TryParse(pidText, out pid) || pidText.Length != 13)
                {
                    s.errorMessage = "เลขประจำตัวประชาชนไม่ถูกต้อง";
                    return View(s);
                }
                
                // _logger.LogInformation("Initiating API request for PID: {PID}", pidText);
                ViewData["spid"] = pidText;
                s.people = new People();
                // _logger.LogInformation("Sending API request {people}", s.people);
                var data = new  
                {
                    jobID = "f46d6089-723c-441c-86f3-92a4de07acef",
                    data= new [] {
                        new {
                            serviceID = 1,
                            query= new {
                                personalID = pidText
                            }
                        },
                        new {
                            serviceID = 9,
                            query = new {
                                personalID = pidText
                            }
                        },
                        new {
                            serviceID = 12,
                            query = new {
                                personalID = pidText
                            }
                        },
                        new {
                            serviceID = 21,
                            query = new {
                                personalID = pidText
                            }
                        },
                        new {
                            serviceID = 38,
                            query = new {
                                personalID = pidText
                            }
                        }
                    }                    
                };
                
                // _logger.LogInformation("Sending API request {data}", data);
                var lkToken = User.FindFirst("lktoken")?.Value ?? null;
                // _logger.LogInformation("Retrieved token from user claims: {token}", lkToken);
                if (string.IsNullOrWhiteSpace(lkToken))
                {
                    s.errorMessage = "ไม่พบ token สำหรับเรียกใช้งานระบบ กรุณาเข้าสู่ระบบใหม่";
                    return View(s);
                }
                var path = "/api/center/request/";
                // _logger.LogInformation("Sending API request {path} {data} {token}", path, data, lkToken);
                var response = await api.PostDataAsync<AggregateApiResponse>($"/api/center/request/", data, lkToken );
                _logger.LogInformation("API response received with status code: {statusCode}", api.statusCode);
                if (api.statusCode != 200 && api.statusCode != 404)
                {
                    s.errorMessage = $"API error: {api.statusCode} {response?.errorMessage ?? "Unknown error"}";
                    return View(s);
                }
                _logger.LogInformation("API response received with data: {response}", response);
                if (response?.data == null || response.data.Count == 0)
                {
                    s.errorMessage = string.IsNullOrWhiteSpace(s.errorMessage)
                        ? "ไม่พบข้อมูลจาก API"
                        : s.errorMessage;
                    return View(s);
                }

                _logger.LogInformation("API response received with  time: {time}", response.executeTimeMs);
                foreach (var item in response.data)
                {
                    if (item.responseData.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
                        continue;

                    switch (item.serviceID)
                    {
                        case 1:
                            s.people.pop = AggregateResponseHelper.DeserializeResponseData<Pop>(item.responseData) ?? s.people.pop;
                            break;
                        case 38:
                            s.people.house = AggregateResponseHelper.DeserializeResponseData<House>(item.responseData) ?? s.people.house;
                            break;
                        case 9:
                            s.people.rename = AggregateResponseHelper.DeserializeResponseData<Rename>(item.responseData) ?? s.people.rename;
                            break;
                        case 12:
                            s.people.child = AggregateResponseHelper.DeserializeResponseData<Child>(item.responseData) ?? s.people.child;
                            break;
                        case 21:
                            s.people.cardImage = AggregateResponseHelper.DeserializeResponseData<CardImage>(item.responseData) ?? s.people.cardImage;
                            break;
                        default:
                            MapByPayloadShape(item.responseData, s.people);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PeopleController.PID failed");
                s.errorMessage = ex.Message;
                return View(s);
            }

            return View(s);
        }

        private static void MapByPayloadShape(JsonElement responseData, People people)
        {
            try
            {
                if (people.pop == null && HasAllProperties(responseData, "fullnameAndRank", "dateOfBirth", "statusOfPersonDesc"))
                {
                    people.pop = AggregateResponseHelper.DeserializeResponseData<Pop>(responseData);
                    return;
                }

                if (people.house == null && HasAllProperties(responseData, "houseID", "houseNo", "provinceDesc", "districtDesc"))
                {
                    people.house = AggregateResponseHelper.DeserializeResponseData<House>(responseData);
                    return;
                }

                if (people.child == null && (HasProperty(responseData, "child") || HasProperty(responseData, "totalChild")))
                {
                    people.child = AggregateResponseHelper.DeserializeResponseData<Child>(responseData);
                    return;
                }

                if (people.nsho == null && HasAllProperties(responseData, "MAININSCL", "MAININSCL_NAME", "PERSON_ID"))
                {
                    people.nsho = AggregateResponseHelper.DeserializeResponseData<NshoService>(responseData);
                }
            }
            catch
            {
                // Ignore partial payload mapping failures for non-critical sections.
            }
        }

        private static bool HasProperty(JsonElement element, string propertyName)
            => element.ValueKind == JsonValueKind.Object && element.TryGetProperty(propertyName, out _);

        private static bool HasAllProperties(JsonElement element, params string[] propertyNames)
        {
            if (element.ValueKind != JsonValueKind.Object)
                return false;

            foreach (var propertyName in propertyNames)
            {
                if (!element.TryGetProperty(propertyName, out _))
                    return false;
            }

            return true;
        }
        public IActionResult NAME()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NAME(IFormCollection f)
        {
            if (f == null || !f.ContainsKey("spid"))
            {
                var sm = new Searchs();
                sm.errorMessage = "กรุณากรอกเลขประจำตัวประชาชน";
                return View(sm);
            }
            var s = new Searchs();
            s.errorMessage = f["error"].ToString() ?? "";
            _logger.LogInformation("Received PID search request with data: {error} {pid}", f["error"].ToString(), f["spid"].ToString());
            try
            {
                // s.setData(f);

                String pidText = f["spid"].ToString();
                long pid = 0L;
                if (!long.TryParse(pidText, out pid) || pidText.Length != 13)
                {
                    s.errorMessage = "เลขประจำตัวประชาชนไม่ถูกต้อง";
                    return View(s);
                }
                
                // _logger.LogInformation("Initiating API request for PID: {PID}", pidText);
                ViewData["spid"] = pidText;
                s.people = new People();
                // _logger.LogInformation("Sending API request {people}", s.people);
                var data = new  
                {
                    jobID = "082f70b0-d561-4161-83db-1f0af9ea7885",
                    data= new [] {
                        new {
                            serviceID = 51,
                            query= new {
                                limit= f["limit"].ToString() ?? "10",
                                firstName= f["firstName"].ToString() ?? "",
                                lastName= f["lastName"].ToString() ?? "",
                                middleName= f["middleName"].ToString() ?? "",
                                recordNumber= f["recordNumber"].ToString() ?? ""
                            }
                        }
                    }                    
                };
                
                // _logger.LogInformation("Sending API request {data}", data);
                var lkToken = User.FindFirst("lktoken")?.Value ?? null;
                // _logger.LogInformation("Retrieved token from user claims: {token}", lkToken);
                if (string.IsNullOrWhiteSpace(lkToken))
                {
                    s.errorMessage = "ไม่พบ token สำหรับเรียกใช้งานระบบ กรุณาเข้าสู่ระบบใหม่";
                    return View(s);
                }
                var path = "/api/center/request/";
                // _logger.LogInformation("Sending API request {path} {data} {token}", path, data, lkToken);
                var response = await api.PostDataAsync<AggregateApiResponse>($"/api/center/request/", data, lkToken );
                _logger.LogInformation("API response received with status code: {statusCode}", api.statusCode);
                if (api.statusCode != 200 && api.statusCode != 404)
                {
                    s.errorMessage = $"API error: {api.statusCode} {response?.errorMessage ?? "Unknown error"}";
                    return View(s);
                }
                _logger.LogInformation("API response received with data: {response}", response);
                if (response?.data == null || response.data.Count == 0)
                {
                    s.errorMessage = string.IsNullOrWhiteSpace(s.errorMessage)
                        ? "ไม่พบข้อมูลจาก API"
                        : s.errorMessage;
                    return View(s);
                }

                _logger.LogInformation("API response received with  time: {time}", response.executeTimeMs);
                foreach (var item in response.data)
                {
                    if (item.responseData.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
                        continue;

                    switch (item.serviceID)
                    {
                        case 1:
                            s.people.pop = AggregateResponseHelper.DeserializeResponseData<Pop>(item.responseData) ?? s.people.pop;
                            break;
                        case 38:
                            s.people.house = AggregateResponseHelper.DeserializeResponseData<House>(item.responseData) ?? s.people.house;
                            break;
                        case 9:
                            s.people.rename = AggregateResponseHelper.DeserializeResponseData<Rename>(item.responseData) ?? s.people.rename;
                            break;
                        case 12:
                            s.people.child = AggregateResponseHelper.DeserializeResponseData<Child>(item.responseData) ?? s.people.child;
                            break;
                        case 21:
                            s.people.cardImage = AggregateResponseHelper.DeserializeResponseData<CardImage>(item.responseData) ?? s.people.cardImage;
                            break;
                        default:
                            MapByPayloadShape(item.responseData, s.people);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PeopleController.NAME failed");
                s.errorMessage = ex.Message;
                return View(s);
            }

            return View(s);
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
