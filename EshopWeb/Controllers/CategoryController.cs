using Eshop.DataAccess.Data;
using Eshop.Models;
using Microsoft.AspNetCore.Mvc;

namespace EshopWeb.Controllers;

public class CategoryController : Controller {
    private readonly ApplicationDbContext _context;
    public CategoryController(ApplicationDbContext context) {
        _context = context;
    }
    public IActionResult Index() {
        List<Category> objCategoryList = _context.Categories.ToList();
        return View(objCategoryList);
    }

    public IActionResult Create() {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Category obj) {
        if (ModelState.IsValid) {
            _context.Categories.Add(obj);
            _context.SaveChanges();
            TempData["success"] = "Category has been created successfully";
            return RedirectToAction("Index");
        }
        return View();
    }

    public IActionResult Edit(int? id) {
        if (id == null || id == 0) return NotFound();
        Category? category = _context.Categories.FirstOrDefault(u => u.Id == id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost]
    public IActionResult Edit(Category obj) {
        if (ModelState.IsValid) {
            _context.Categories.Update(obj);
            _context.SaveChanges();
            TempData["success"] = "Category has been updated successfully";
            return RedirectToAction("Index");
        }
        return View();
    }

    public IActionResult Delete(int? id) {
        if (id == null || id == 0) return NotFound();
        Category? category = _context.Categories.FirstOrDefault(u => u.Id == id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id) {
        Category? obj = _context.Categories.FirstOrDefault(u => u.Id == id);
        if (obj == null) return NotFound();
        _context.Categories.Remove(obj);
        _context.SaveChanges();
        TempData["success"] = "Category has been deleted successfully";
        return RedirectToAction("Index");
    }
}