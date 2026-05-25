using WarehouseAPI.DTOs;
using WarehouseAPI.Models;

namespace WarehouseAPI.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> AuthenticateAsync(Login request);
    }
}
