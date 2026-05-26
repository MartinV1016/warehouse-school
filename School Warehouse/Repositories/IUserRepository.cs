using WarehouseAPI.Models;

namespace WarehouseAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUsernameAsync(string username);
        Task AddAsync(User user);
    }
}
