using Eshop.DataAccess.Data;
using Eshop.DataAccess.Repository.IRepository;
using Eshop.Models;
using Eshop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

	[HttpDelete]
	public IActionResult Delete(int id)
	{
		

		return Json(new { success = true, message = "Delete successful" });
	}

	#endregion
}