using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure;

namespace TaskManagement.Repositories
{
    public class TaskListRepository : ITaskListRepository
    {
        private readonly TaskManagementContext _context;

        public TaskListRepository(TaskManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskList>> GetAllAsync()
        {
            return await _context.TaskLists.ToListAsync();
        }

        public async Task<TaskList> GetByIdAsync(int id)
        {
            return await _context.TaskLists.FindAsync(id);
        }

        public async Task AddAsync(TaskList taskList)
        {
            _context.TaskLists.Add(taskList);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskList taskList)
        {
            _context.TaskLists.Update(taskList);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var taskList = await _context.TaskLists.FindAsync(id);
            if (taskList != null)
            {
                _context.TaskLists.Remove(taskList);
                await _context.SaveChangesAsync();
            }
        }
    }
}
