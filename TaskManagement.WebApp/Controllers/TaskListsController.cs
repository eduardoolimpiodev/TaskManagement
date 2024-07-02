using Microsoft.AspNetCore.Mvc;
using TaskManagement.WebApp.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
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

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("https://localhost:5001/api/tasklists");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var taskLists = JsonConvert.DeserializeObject<IEnumerable<TaskList>>(content);
            return View(taskLists);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskList taskList)
        {
            var content = new StringContent(JsonConvert.SerializeObject(taskList), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:5001/api/tasklists", content);
            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:5001/api/tasklists/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var taskList = JsonConvert.DeserializeObject<TaskList>(content);
            return View(taskList);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaskList taskList)
        {
            var content = new StringContent(JsonConvert.SerializeObject(taskList), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"https://localhost:5001/api/tasklists/{taskList.Id}", content);
            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:5001/api/tasklists/{id}");
            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Index));
        }
    }
}
