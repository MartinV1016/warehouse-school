using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using WarehouseAPI.Data;
using WarehouseAPI.Models;

namespace WarehouseAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly WarehouseDbContext _context;
    public UserRepository(WarehouseDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u=> u.Username == username);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}
