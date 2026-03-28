using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using CISApps.Models;
using CISApps.Models.Linkage;
using CISApps.Models.Linkage.Department;
using CISApps.Models.Linkage.Nsho;
using cis.Models.Rest;

namespace cis.Controllers
{
    public class PeopleController : Controller
    {
        private readonly ILogger<PeopleController> _logger;
        private readonly ApiService api;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

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
            var s = new Searchs();
            s.errorMessage = f["error"].ToString() ?? "";

            try
            {
                s.setData(f);

                var pidText = f["xyspid"].ToString();
                if (!long.TryParse(pidText, out var pid) || pid <= 0)
                {
                    s.errorMessage = "เลขประจำตัวประชาชนไม่ถูกต้อง";
                    return View(s);
                }

                s.people ??= new People();

                // API response shape:
                // {
                //   "data": [
                //     { "serviceID": 1, "responseData": { ... } },
                //     ...
                //   ],
                //   "executeTimeMs": 900
                // }
                var response = await api.GetDataAsync<AggregateApiResponse>($"/api/people/pid/{pid}");
                if (response?.data == null || response.data.Count == 0)
                {
                    s.errorMessage = string.IsNullOrWhiteSpace(s.errorMessage)
                        ? "ไม่พบข้อมูลจาก API"
                        : s.errorMessage;
                    return View(s);
                }

                foreach (var item in response.data)
                {
                    if (item.responseData.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
                        continue;

                    switch (item.serviceID)
                    {
                        case 1:
                            s.people.pop = DeserializeResponseData<Pop>(item.responseData) ?? s.people.pop;
                            break;
                        case 38:
                            s.people.house = DeserializeResponseData<House>(item.responseData) ?? s.people.house;
                            break;
                        case 98:
                            s.people.nsho = DeserializeResponseData<NshoService>(item.responseData) ?? s.people.nsho;
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
            }

            return View(s);
        }

        private static void MapByPayloadShape(JsonElement responseData, People people)
        {
            try
            {
                if (people.pop == null && HasAllProperties(responseData, "fullnameAndRank", "dateOfBirth", "statusOfPersonDesc"))
                {
                    people.pop = DeserializeResponseData<Pop>(responseData);
                    return;
                }

                if (people.house == null && HasAllProperties(responseData, "houseID", "houseNo", "provinceDesc", "districtDesc"))
                {
                    people.house = DeserializeResponseData<House>(responseData);
                    return;
                }

                if (people.child == null && (HasProperty(responseData, "child") || HasProperty(responseData, "totalChild")))
                {
                    people.child = DeserializeResponseData<Child>(responseData);
                    return;
                }

                if (people.nsho == null && HasAllProperties(responseData, "MAININSCL", "MAININSCL_NAME", "PERSON_ID"))
                {
                    people.nsho = DeserializeResponseData<NshoService>(responseData);
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

        private static T? DeserializeResponseData<T>(JsonElement responseData)
        {
            try
            {
                return responseData.Deserialize<T>(JsonOptions);
            }
            catch
            {
                return default;
            }
        }

        public IActionResult NAME()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NAME(IFormCollection f)
        {
            return View();
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
