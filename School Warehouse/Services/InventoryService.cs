using Microsoft.EntityFrameworkCore;
using WarehouseAPI.Data;
using WarehouseAPI.DTOs;
using WarehouseAPI.Models;
using WarehouseAPI.Repositories;

namespace WarehouseAPI.Services;

public class InventoryService : IInventoryService
{
    private readonly IItemRepository _itemRepository;

    public InventoryService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<IEnumerable<Item>> GetAllItemsAsync()
    {

        return await _itemRepository.GetAllAsync();
    }

    public async Task<Item> CreateItemAsync(ItemCreateDto request)
    {
        var newItem = new Item
        {
            Name = request.Name,
            Category = request.Category.ToLower(),
            Quantity = request.Quantity,
            Location = request.Location
        };

        await _itemRepository.AddAsync(newItem);
        return newItem;
    }

    public async Task<Item?> UpdateItemAsync(int id, ItemUpdateDto request)
    {
        var item = await _itemRepository.GetByIdAsync(id);
        if (item == null) return null;

        item.Name = request.Name;
        item.Category = request.Category;
        item.Quantity = request.Quantity;
        item.Location = request.Location;

        await _itemRepository.UpdateAsync(item);
        return item;
    }

    public async Task<bool> DeleteItemAsync(int id)
    {
        var item = await _itemRepository.GetByIdAsync(id);
        if (item == null) return false;

        await _itemRepository.DeleteAsync(item);
        return true;
    }
}