using WarehouseAPI.DTOs;
using WarehouseAPI.Models;

namespace WarehouseAPI.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDTO?> AuthenticateAsync(Login request);
        Task<bool> RegisterAsync(RegisterDTO request);
    }
}
