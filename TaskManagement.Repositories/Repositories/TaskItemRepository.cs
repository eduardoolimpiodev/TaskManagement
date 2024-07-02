using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Repositories
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly TaskManagementContext _context;

        public TaskItemRepository(TaskManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllByTaskListIdAsync(int taskListId)
        {
            return await _context.TaskItems.Where(t => t.TaskListId == taskListId).ToListAsync();
        }

        public async Task<TaskItem> GetByIdAsync(int id)
        {
            return await _context.TaskItems.FindAsync(id);
        }

        public async Task AddAsync(TaskItem taskItem)
        {
            await _context.TaskItems.AddAsync(taskItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskItem taskItem)
        {
            _context.TaskItems.Update(taskItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            _context.TaskItems.Remove(taskItem);
            await _context.SaveChangesAsync();
        }
    }
}
