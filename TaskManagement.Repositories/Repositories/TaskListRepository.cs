using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure;

namespace TaskManagement.Infrastructure.Repositories
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

        public async Task<TaskList> GetByNameAsync(string name)
        {
            return await _context.TaskLists.FirstOrDefaultAsync(tl => tl.Name == name);
        }

        public async Task AddAsync(TaskList taskList)
        {
            await _context.TaskLists.AddAsync(taskList);
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
