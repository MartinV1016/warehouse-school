using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
	public IActionResult Add(Item item)
	{
		var role = User.FindFirst(ClaimTypes.Role)?.Value;

		if (role != "admin")
			return Forbid();

		item.Id = inventory.Count + 1;
		inventory.Add(item);

		return Ok(item);
	}

	[HttpPut("{id}")]
	public IActionResult Update(int id, Item updated)
	{
		var item = inventory.FirstOrDefault(i => i.Id == id);
		if (item == null) return NotFound();

		var role = User.FindFirst(ClaimTypes.Role)?.Value;

		if (role != "admin" && role != item.Category)
			return Forbid();

		item.Name = updated.Name;
		item.Quantity = updated.Quantity;

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