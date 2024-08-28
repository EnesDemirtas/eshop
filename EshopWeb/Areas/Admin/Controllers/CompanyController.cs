using Eshop.Models;
using Eshop.Models.ViewModels;
using Eshop.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Eshop.Utility;
using Microsoft.AspNetCore.Authorization;

namespace EshopWeb.Controllers;

[Area("Admin")]
[Authorize(Roles = StaticDetails.Role_Admin)]
public class CompanyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public CompanyController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
        return View(objCompanyList);
    }

    public IActionResult Upsert(int? id)
    {
        if (id == null || id == 0)
        {
            return View(new Company());
        } else
        {
            Company companyObj = _unitOfWork.Company.Get(c => c.Id == id);
            return View(companyObj);
        }
    }

    [HttpPost]
    public IActionResult Upsert(Company companyObj)
    {
        if (ModelState.IsValid)
        {
            if (companyObj.Id == 0) {
                _unitOfWork.Company.Add(companyObj);
            } else {
                _unitOfWork.Company.Update(companyObj);
            }
            _unitOfWork.Save();
            TempData["success"] = "Company has been created successfully";
            return RedirectToAction("Index");
        } else {
            return View(companyObj);
        }
    }

    #region API CALLS

    [HttpGet]
    public IActionResult GetAll() {
        List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
        return Json(new { data = objCompanyList });
    }

    [HttpDelete]
    public IActionResult Delete(int id) {
        var Company = _unitOfWork.Company.Get(p => p.Id == id);
        if (Company == null) return Json(new { success = false, message = "Error while deleting" });

        _unitOfWork.Company.Remove(Company);
        _unitOfWork.Save();

        return Json(new { success = true, message = "Delete successful" });
    }

    #endregion
}