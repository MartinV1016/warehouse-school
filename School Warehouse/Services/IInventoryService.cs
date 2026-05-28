using WarehouseAPI.DTOs;
using WarehouseAPI.Models;

namespace WarehouseAPI.Services;

public interface IInventoryService
{
    Task<List<Item>> GetAllItemsAsync();
    Task<Item> CreateItemAsync(ItemCreateDTO request);
    Task<Item?> UpdateItemAsync(int id, ItemUpdateDTO request);
    Task<bool> DeleteItemAsync(int id);
}