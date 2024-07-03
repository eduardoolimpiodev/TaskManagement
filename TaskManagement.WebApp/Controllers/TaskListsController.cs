using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskManagement.Domain.Entities;

namespace TaskManagement.WebApp.Controllers
{
    public class TaskListsController : Controller
    {
        private readonly HttpClient _httpClient;

        public TaskListsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private void SetAuthorizationHeader()
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<IActionResult> Index()
        {
            SetAuthorizationHeader();
            var response = await _httpClient.GetAsync("http://localhost:5200/api/tasklists");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var taskLists = JsonConvert.DeserializeObject<IEnumerable<TaskManagement.Domain.Entities.TaskList>>(content);
            return View(taskLists);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskList taskList)
        {
            if (!ModelState.IsValid)
            {
                return View(taskList);
            }

            var response = await _httpClient.PostAsJsonAsync("http://localhost:5200/api/tasklists", taskList);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            return View(taskList);
        }

        public async Task<IActionResult> Edit(int id)
        {
            SetAuthorizationHeader();
            var response = await _httpClient.GetAsync($"http://localhost:5200/api/tasklists/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var taskList = JsonConvert.DeserializeObject<TaskManagement.Domain.Entities.TaskList>(content);
            return View(taskList);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaskManagement.Domain.Entities.TaskList taskList)
        {
            SetAuthorizationHeader();
            var content = new StringContent(JsonConvert.SerializeObject(taskList), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"http://localhost:5200/api/tasklists/{taskList.Id}", content);
            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            SetAuthorizationHeader();
            var response = await _httpClient.DeleteAsync($"http://localhost:5200/api/tasklists/{id}");
            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Index));
        }
    }
}
