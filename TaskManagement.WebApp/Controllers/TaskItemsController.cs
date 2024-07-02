using Microsoft.AspNetCore.Mvc;
using TaskManagement.Domain.Entities;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace TaskManagement.WebApp.Controllers
{
    public class TaskItemsController : Controller
    {
        private readonly HttpClient _httpClient;

        public TaskItemsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index(int taskListId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:5001/api/taskitems?taskListId={taskListId}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var taskItems = JsonConvert.DeserializeObject<IEnumerable<TaskManagement.Domain.Entities.TaskItem>>(content);
            return View(taskItems);
        }

        public IActionResult Create(int taskListId)
        {
            var taskItem = new TaskManagement.Domain.Entities.TaskItem { TaskListId = taskListId };
            return View(taskItem);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskManagement.Domain.Entities.TaskItem taskItem)
        {
            var content = new StringContent(JsonConvert.SerializeObject(taskItem), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:5001/api/taskitems", content);
            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Index), new { taskListId = taskItem.TaskListId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:5001/api/taskitems/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var taskItem = JsonConvert.DeserializeObject<TaskManagement.Domain.Entities.TaskItem>(content);
            return View(taskItem);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaskManagement.Domain.Entities.TaskItem taskItem)
        {
            var content = new StringContent(JsonConvert.SerializeObject(taskItem), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"https://localhost:5001/api/taskitems/{taskItem.Id}", content);
            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Index), new { taskListId = taskItem.TaskListId });
        }

        public async Task<IActionResult> Delete(int id, int taskListId)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:5001/api/taskitems/{id}");
            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Index), new { taskListId });
        }
    }
}
