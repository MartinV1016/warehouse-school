using WarehouseAPI.DTOs;
using WarehouseAPI.Models;

namespace WarehouseAPI.Services;

public interface IInventoryService
{
    Task<List<Item>> GetAllItemsAsync();
    Task<Item> CreateItemAsync(ItemCreateDto request);
    Task<Item?> UpdateItemAsync(int id, ItemUpdateDto request);
    Task<bool> DeleteItemAsync(int id);
}