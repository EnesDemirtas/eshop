using Eshop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Eshop.Models.ViewModels;

public class ProductVM {
    public Product Product {get; set;}
    [ValidateNever]
    public IEnumerable<SelectListItem> CategoryList {get; set;}
}