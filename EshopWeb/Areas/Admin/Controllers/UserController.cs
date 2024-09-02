using Eshop.DataAccess.Data;
using Eshop.DataAccess.Repository.IRepository;
using Eshop.Models;
using Eshop.Models.ViewModels;
using Eshop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EshopWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = StaticDetails.Role_Admin)]
public class UserController : Controller
{
	private readonly ApplicationDbContext _context;
	private readonly UserManager<IdentityUser> _userManager;
	public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
	{
		_context = context;
		_userManager = userManager;
	}
	public IActionResult Index()
	{
		return View();
	}

	public IActionResult RoleManagement(string userId)
	{
		string RoleID = _context.UserRoles.FirstOrDefault(ur => ur.UserId == userId).RoleId;

		RoleManagementVM RoleVM = new()
		{
			ApplicationUser = _context.ApplicationUsers.Include(u => u.Company).FirstOrDefault(u => u.Id == userId),
			RoleList = _context.Roles.Select(i => new SelectListItem
			{
				Text = i.Name,
				Value = i.Name
			}),
			CompanyList = _context.Companies.Select(i => new SelectListItem
			{
				Text = i.Name,
				Value = i.Id.ToString()
			})
		};

		RoleVM.ApplicationUser.Role = _context.Roles.FirstOrDefault(u => u.Id == RoleID).Name;

		return View(RoleVM);
	}

	[HttpPost]
    public IActionResult RoleManagement(RoleManagementVM roleManagementVM)
    {
        string RoleID = _context.UserRoles.FirstOrDefault(ur => ur.UserId == roleManagementVM.ApplicationUser.Id).RoleId;
		string oldRole = _context.Roles.FirstOrDefault(u => u.Id == RoleID).Name;

		if (!(roleManagementVM.ApplicationUser.Role == oldRole))
		{
			ApplicationUser applicationUser = _context.ApplicationUsers.FirstOrDefault(u => u.Id == roleManagementVM.ApplicationUser.Id);
			if (roleManagementVM.ApplicationUser.Role == StaticDetails.Role_Company)
			{
				applicationUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;
			}
			if (oldRole == StaticDetails.Role_Company)
			{
				applicationUser.CompanyId = null;
			}
			_context.SaveChanges();

			_userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
			_userManager.AddToRoleAsync(applicationUser, roleManagementVM.ApplicationUser.Role).GetAwaiter().GetResult();
        }

        return RedirectToAction("Index");
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