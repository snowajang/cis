using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CISApps.Models;
using CISApps.Models.Home;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using CISApps.Models.Rest;

namespace cis.Controllers
{ 
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(int logout=0)
        {
            if (logout == 1)
            {
                await HttpContext.SignOutAsync();
            }
            if (User != null && (User?.Identity?.IsAuthenticated ?? false) ) { 
                return RedirectToAction("Index", "User"); 
            }
            LoginThaiD login  = new LoginThaiD();
            return View(login);
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginThaiD login)
        {
            try
            {
                if (login == null)
                {
                    login = new LoginThaiD();
                    login.error = "ไม่ได้รับการตอบกลับจากระบบ ThaID";
                    return View(login);
                }
                // xติดต่อ lk2 
                string token="";
                try
                {
                    if (string.IsNullOrEmpty(login?.error))
                    {
                        return View(login);
                    }
                    long ipid = long.Parse(login.pid);
                    var api = new Api();
                    api.setDomain("https://lk2proxy.bora.dopa.go.th");
                    var response = await api.POST("/api/center/login/auth", new { loginType = 2, officeID=333, accessToken = login.accessToken, pid = ipid });
                    var data = await response.Content.ReadAsStringAsync();
                    var json = data.ToObjectJson<ResponseLogin>();
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        if (json?.errorNumber > 0)
                        {
                            login.error = json?.errorMessage ?? "";
                        }
                        else
                        {
                            login.error = "พบปัญหาเชื่อมต่อระบบ";
                        }
                        return View(login);
                    }
                    token = json?.token;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "home-index");
                    throw;
                }
                // login 
                var identity = new ClaimsIdentity("Cookies", ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, login.uname));
                identity.AddClaim(new Claim(ClaimTypes.Name, login.pid));
                identity.AddClaim(new Claim("lktoken", token));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        AllowRefresh = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                    });
                return RedirectToAction("Index", "User");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "home-index");
                throw;
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}
