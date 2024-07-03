using Microsoft.AspNetCore.Mvc;
using TaskManagement.WebApp.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace TaskManagement.WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:5001/api/auth/login", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<dynamic>(responseContent).token;
            HttpContext.Session.SetString("JWTToken", (string)token);

            return RedirectToAction("Index", "TaskLists");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:5001/api/auth/register", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Registration failed.");
                return View(model);
            }

            return RedirectToAction("Login");
        }
    }
}
