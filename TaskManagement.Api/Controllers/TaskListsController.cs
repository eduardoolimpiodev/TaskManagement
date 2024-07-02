using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskListsController : ControllerBase
    {
        private readonly ITaskListRepository _taskListRepository;

        public TaskListsController(ITaskListRepository taskListRepository)
        {
            _taskListRepository = taskListRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskList>>> GetTaskLists()
        {
            var taskLists = await _taskListRepository.GetAllAsync();
            return Ok(taskLists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskList>> GetTaskList(int id)
        {
            var taskList = await _taskListRepository.GetByIdAsync(id);
            if (taskList == null)
            {
                return NotFound();
            }
            return Ok(taskList);
        }

        [HttpPost]
        public async Task<ActionResult<TaskList>> PostTaskList(TaskList taskList)
        {
            await _taskListRepository.AddAsync(taskList);
            return CreatedAtAction(nameof(GetTaskList), new { id = taskList.Id }, taskList);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskList(int id, TaskList taskList)
        {
            if (id != taskList.Id)
            {
                return BadRequest();
            }

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

            if (taskList.Tasks.Any())
            {
                return BadRequest("Cannot delete a list with tasks.");
            }

            await _taskListRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
