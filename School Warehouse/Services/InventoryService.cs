using Microsoft.EntityFrameworkCore;
using WarehouseAPI.Data;
using WarehouseAPI.DTOs;
using WarehouseAPI.Models;

namespace WarehouseAPI.Services;

public class InventoryService : IInventoryService
{
    private readonly WarehouseDbContext _context;

    public InventoryService(WarehouseDbContext context)
    {
        _context = context;
    }

    public async Task<List<Item>> GetAllItemsAsync()
    {

        return await _context.Items.ToListAsync();
    }

    public async Task<Item> CreateItemAsync(ItemCreateDTO request)
    {
        var newItem = new Item
        {
            Name = request.Name,
            Category = request.Category.ToLower(),
            Quantity = request.Quantity,
            Location = request.Location
        };

        await _context.Items.AddAsync(newItem);
        await _context.SaveChangesAsync();
        return newItem;
    }

    public async Task<Item?> UpdateItemAsync(int id, ItemUpdateDTO request)
    {
        var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id);
        if (item == null) return null;

        item.Name = request.Name;
        item.Category = request.Category;
        item.Quantity = request.Quantity;
        item.Location = request.Location;

        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> DeleteItemAsync(int id)
    {
        var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id);
        if (item == null) return false;

        _context.Items.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}