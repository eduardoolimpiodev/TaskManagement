using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskItemRepository
    {
        Task<IEnumerable<TaskItem>> GetAllByTaskListIdAsync(int taskListId);
        Task<TaskItem> GetByIdAsync(int id);
        Task AddAsync(TaskItem taskItem);
        Task UpdateAsync(TaskItem taskItem);
        Task DeleteAsync(int id);
    }
}
