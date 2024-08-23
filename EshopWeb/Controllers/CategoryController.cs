using Eshop.Models;
using Eshop.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace EshopWeb.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryRepository _categoryRepo;
    public CategoryController(ICategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }
    public IActionResult Index()
    {
        List<Category> objCategoryList = _categoryRepo.GetAll().ToList();
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
            _categoryRepo.Add(obj);
            _categoryRepo.Save();
            TempData["success"] = "Category has been created successfully";
            return RedirectToAction("Index");
        }
        return View();
    }

    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0) return NotFound();
        Category? category = _categoryRepo.Get(u => u.Id == id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost]
    public IActionResult Edit(Category obj)
    {
        if (ModelState.IsValid)
        {
            _categoryRepo.Update(obj);
            _categoryRepo.Save();
            TempData["success"] = "Category has been updated successfully";
            return RedirectToAction("Index");
        }
        return View();
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0) return NotFound();
        Category? category = _categoryRepo.Get(u => u.Id == id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {
        Category? obj = _categoryRepo.Get(u => u.Id == id);
        if (obj == null) return NotFound();
        _categoryRepo.Remove(obj);
        _categoryRepo.Save();
        TempData["success"] = "Category has been deleted successfully";
        return RedirectToAction("Index");
    }
}