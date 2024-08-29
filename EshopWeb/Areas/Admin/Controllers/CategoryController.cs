using Eshop.DataAccess.Repository.IRepository;
using Eshop.Models;
using Eshop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EshopWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = StaticDetails.Role_Admin)]
public class CategoryController : Controller
{
	private readonly IUnitOfWork _unitOfWork;
	public CategoryController(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}
	public IActionResult Index()
	{
		List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
		return View(objCategoryList);
	}

	public IActionResult Create()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Create(Category obj)
	{
		if (ModelState.IsValid)
		{
			_unitOfWork.Category.Add(obj);
			_unitOfWork.Save();
			TempData["success"] = "Category has been created successfully";
			return RedirectToAction("Index");
		}
		return View();
	}

	public IActionResult Edit(int? id)
	{
		if (id == null || id == 0) return NotFound();
		Category? category = _unitOfWork.Category.Get(u => u.Id == id);
		if (category == null) return NotFound();
		return View(category);
	}

	[HttpPost]
	public IActionResult Edit(Category obj)
	{
		if (ModelState.IsValid)
		{
			_unitOfWork.Category.Update(obj);
			_unitOfWork.Save();
			TempData["success"] = "Category has been updated successfully";
			return RedirectToAction("Index");
		}
		return View();
	}

	public IActionResult Delete(int? id)
	{
		if (id == null || id == 0) return NotFound();
		Category? category = _unitOfWork.Category.Get(u => u.Id == id);
		if (category == null) return NotFound();
		return View(category);
	}

	[HttpPost, ActionName("Delete")]
	public IActionResult DeletePOST(int? id)
	{
		Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
		if (obj == null) return NotFound();
		_unitOfWork.Category.Remove(obj);
		_unitOfWork.Save();
		TempData["success"] = "Category has been deleted successfully";
		return RedirectToAction("Index");
	}
}