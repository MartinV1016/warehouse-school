using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WarehouseAPI.DTOs;
using WarehouseAPI.Models;
using WarehouseAPI.Data;
using WarehouseAPI.Services;

[ApiController]
[Route("[controller]")]
[Authorize]
public class InventoryController : ControllerBase
{
    /*private static List<Item> inventory = new()
	{
		new Item { Id = 1, Name = "Pens", Category = "stationery", Quantity = 100, Location="A-03-D-05" },
		new Item { Id = 2, Name = "Laptops", Category = "electronics", Quantity = 20, Location="C-01-B-02" }
	};*/
    /*private readonly WarehouseDbContext _context;

	public InventoryController(WarehouseDbContext context)
	{
		_context = context;
	}*/

    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
	public async Task<IActionResult> GetAll()
    {
		var items=await _inventoryService.GetAllItemsAsync();
		return Ok(items);
	}

	[HttpPost]
	[Authorize(Roles="admin")]
	public async Task<IActionResult> CreateItem([FromBody] ItemCreateDto request)
	{
		var newItem = await _inventoryService.CreateItemAsync(request);
		
		return CreatedAtAction(nameof(GetAll),new {id=newItem.Id},newItem);
	}
	
	/*public IActionResult Add(ItemCreateDto request)
	{
		var role = User.FindFirst(ClaimTypes.Role)?.Value;

		if (role != "admin")
			return Forbid();

		var newItem = new Item
		{
			Id = inventory.Count + 1,
			Name = request.Name,
			Category = request.Category.ToLower(),
			Quantity = request.Quantity,
			Location = request.Location
		};

		
		inventory.Add(newItem);

		return Ok(newItem);
	}*/

	[HttpPut("{id}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Update(int id, [FromBody] ItemUpdateDto request)
	{
		var item=await _inventoryService.UpdateItemAsync(id,request);
		if (item == null)
		{
			return NotFound($"Item with id {id} not found");
		}

		return Ok(item);
	}

	/*public IActionResult Update(int id, ItemUpdateDto request)
	{
		var item = inventory.FirstOrDefault(i => i.Id == id);
		if (item == null) 
		{ 
			return NotFound(ApiResponse<object>.Fail($"Item with ID {id} was not found")); 
		}

		/*var role = User.FindFirst(ClaimTypes.Role)?.Value;

		if (role != "admin" && role != item.Category)
			return Forbid();*/
	/*
		item.Name = request.Name;
		item.Category = request.Category;
		item.Quantity = request.Quantity;
		item.Location = request.Location;


		var response = ApiResponse<object>.Success(item, $"Item '{item.Name}' updated sucessfully.");
		return Ok(response);
	}*/

	[HttpDelete("{id}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Delete(int id)
	{
        var success = await _inventoryService.DeleteItemAsync(id);
        if (!success)
        {
            return NotFound($"Deletion failed. Item with ID {id} not found.");
        }	
        return Ok(new { message = $"Successfully deleted item" });
    }

	/*public IActionResult Delete(int id)
	{
		/*var role = User.FindFirst(ClaimTypes.Role)?.Value;

		if (role != "admin")
			return Forbid();*/
	/*
		var item = inventory.FirstOrDefault(i => i.Id == id);
		if (item == null) return NotFound(ApiResponse<object>.Fail($"Deletion failed. Item with {id} id not found"));

		inventory.Remove(item);
		var response = ApiResponse<object>.Success("Deleted", $"Sucessfully deleted '{item.Name}'");
		return Ok(response);
	}*/
}