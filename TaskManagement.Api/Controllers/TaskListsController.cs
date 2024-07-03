using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Api.Dtos;

namespace TaskManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskListsController : ControllerBase
    {
        private readonly ITaskListRepository _taskListRepository;

        public TaskListsController(ITaskListRepository taskListRepository)
        {
            _taskListRepository = taskListRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<TaskList>> GetAll()
        {
            return await _taskListRepository.GetAllAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskList([FromBody] TaskListDto taskListDto)
        {
            if (_taskListRepository.GetAllAsync().Result.Any(tl => tl.Name == taskListDto.Name))
            {
                return BadRequest("A task list with the same name already exists.");
            }

            var taskList = new TaskList
            {
                Name = taskListDto.Name,
                Description = taskListDto.Description
            };

            await _taskListRepository.AddAsync(taskList);
            return CreatedAtAction(nameof(GetById), new { id = taskList.Id }, taskList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var taskList = await _taskListRepository.GetByIdAsync(id);
            if (taskList == null)
            {
                return NotFound();
            }

            return Ok(taskList);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTaskList(int id, [FromBody] TaskListDto taskListDto)
        {
            var taskList = await _taskListRepository.GetByIdAsync(id);
            if (taskList == null)
            {
                return NotFound();
            }

            if (_taskListRepository.GetAllAsync().Result.Any(tl => tl.Name == taskListDto.Name && tl.Id != id))
            {
                return BadRequest("A task list with the same name already exists.");
            }

            taskList.Name = taskListDto.Name;
            taskList.Description = taskListDto.Description;

            await _taskListRepository.UpdateAsync(taskList);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskList(int id)
        {
            var taskList = await _taskListRepository.GetByIdAsync(id);
            if (taskList == null)
            {
                return NotFound();
            }

            if (taskList.TaskItems.Any())
            {
                return BadRequest("Cannot delete a task list that contains tasks.");
            }

            await _taskListRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
