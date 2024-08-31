using Eshop.DataAccess.Data;
using Eshop.DataAccess.Repository.IRepository;
using Eshop.Models;
using Eshop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace EshopWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = StaticDetails.Role_Admin)]
public class UserController : Controller
{
	private readonly ApplicationDbContext _context;
	public UserController(ApplicationDbContext context)
	{
		_context = context;
	}
	public IActionResult Index()
	{
		return View();
	}

	#region API CALLS

	[HttpGet]
	public IActionResult GetAll()
	{
		List<ApplicationUser> objUserList = _context.ApplicationUsers.Include(u => u.Company).ToList();

		var userRoles = _context.UserRoles.ToList();
		var roles = _context.Roles.ToList();

		foreach (var user in objUserList)
		{
			var roleId = userRoles.FirstOrDefault(ur => ur.UserId == user.Id).RoleId;
			user.Role = roles.FirstOrDefault(r => r.Id == roleId).Name;

			if (user.Company == null)
			{
				user.Company = new() { Name = "" };
			}
		}

		return Json(new { data = objUserList });
	}

	[HttpPost]
	public IActionResult LockUnlock([FromBody] string id)
	{
		var objFromDb = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
		if (objFromDb == null)
		{
			return Json(new { success = false, message = "Error while Locking/Unlocking" });
		}

		if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
		{
			objFromDb.LockoutEnd = DateTime.Now;
		} else
		{
			objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
		}

		_context.SaveChanges();

		return Json(new { success = true, message = "Operation successful" });
	}

	#endregion
}