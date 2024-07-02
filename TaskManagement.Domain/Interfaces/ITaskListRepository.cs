using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskListRepository
    {
        Task<IEnumerable<TaskList>> GetAllAsync();
        Task<TaskList> GetByIdAsync(int id);
        Task AddAsync(TaskList taskList);
        Task UpdateAsync(TaskList taskList);
        Task DeleteAsync(int id);
    }
}
