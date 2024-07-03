using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskManagement.WebApp.Dtos;
using TaskManagement.WebApp.Models;

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

            var taskLists = JsonConvert.DeserializeObject<IEnumerable<TaskManagement.Domain.Entities.TaskList>>(await response.Content.ReadAsStringAsync());
            var taskListModels = MapToTaskListModels(taskLists);

            return View(taskListModels);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskListDto taskListDto)
        {
            if (!ModelState.IsValid)
            {
                return View(taskListDto);
            }

            var jsonContent = JsonConvert.SerializeObject(taskListDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:5200/api/tasklists", content);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            SetAuthorizationHeader();
            var response = await _httpClient.GetAsync($"http://localhost:5200/api/tasklists/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var taskList = JsonConvert.DeserializeObject<TaskManagement.Domain.Entities.TaskList>(content);
            var taskListModel = MapToTaskListModel(taskList);

            return View(taskListModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaskList taskList)
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

        private IEnumerable<TaskList> MapToTaskListModels(IEnumerable<TaskManagement.Domain.Entities.TaskList> taskLists)
        {
            var taskListModels = new List<TaskList>();

            foreach (var taskList in taskLists)
            {
                taskListModels.Add(new TaskList
                {
                    Id = taskList.Id,
                    Name = taskList.Name,
                    Description = taskList.Description
                });
            }

            return taskListModels;
        }

        private TaskList MapToTaskListModel(TaskManagement.Domain.Entities.TaskList taskList)
        {
            return new TaskList
            {
                Id = taskList.Id,
                Name = taskList.Name,
                Description = taskList.Description
            };
        }
    }
}
