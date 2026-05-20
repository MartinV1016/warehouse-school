using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WarehouseAPI.DTOs;

[ApiController]
[Route("[controller]")]
[Authorize]
public class InventoryController : ControllerBase
{
	private static List<Item> inventory = new()
	{
		new Item { Id = 1, Name = "Pens", Category = "stationery", Quantity = 100 },
		new Item { Id = 2, Name = "Laptops", Category = "electronics", Quantity = 20 }
	};

	[HttpGet]
	public IActionResult GetAll()
	{
		return Ok(inventory);
	}

	[HttpPost]
	public IActionResult Add(itemCreateDto request)
	{
		var role = User.FindFirst(ClaimTypes.Role)?.Value;

		if (role != "admin")
			return Forbid();

		var newItem = new Item
		{
			Id = inventory.Count + 1,
			Name = request.Name,
			Category = request.Category.ToLower(),
			Quantity = request.Quantity
		};

		
		inventory.Add(newitem);

		return Ok(newitem);
	}

	[HttpPut("{id}")]
	public IActionResult Update(int id, ItemUpdateDTO request)
	{
		var item = inventory.FirstOrDefault(i => i.Id == id);
		if (item == null) return NotFound();

		var role = User.FindFirst(ClaimTypes.Role)?.Value;

		if (role != "admin" && role != item.Category)
			return Forbid();

		item.Name = request.Name;
		item.Quantity = request.Quantity;

		return Ok(item);
	}

	[HttpDelete("{id}")]
	public IActionResult Delete(int id)
	{
		var role = User.FindFirst(ClaimTypes.Role)?.Value;

		if (role != "admin")
			return Forbid();

		var item = inventory.FirstOrDefault(i => i.Id == id);
		if (item == null) return NotFound();

		inventory.Remove(item);
		return Ok();
	}
}