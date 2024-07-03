using System.Threading.Tasks;
using TaskManagement.Api.Models;

namespace TaskManagement.Api.Services
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(UserRegisterModel model);
        Task<AuthResult> LoginAsync(UserLoginModel model);
    }
}
