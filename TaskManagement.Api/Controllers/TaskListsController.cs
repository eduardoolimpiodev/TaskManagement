using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

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
        public async Task<IActionResult> Create([FromBody] TaskList taskList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
        public async Task<IActionResult> Update(int id, TaskList taskList)
        {
            if (id != taskList.Id)
            {
                return BadRequest();
            }

            await _taskListRepository.UpdateAsync(taskList);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var taskList = await _taskListRepository.GetByIdAsync(id);
            if (taskList == null)
            {
                return NotFound();
            }

            await _taskListRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
