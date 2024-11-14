using Microsoft.AspNetCore.Mvc;
using WW_XXX.API.Services;

namespace WW_XXX.API.Controllers
{
    // WW_XXX.UI/Controllers/ProductController.cs
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {
            var categories = await _categoryService.GetCategoryListAsync();
            var products = await _productService.GetProductListAsync(category, pageNo);

            ViewBag.Categories = categories.Data;
            ViewBag.CurrentCategory = category;

            return View(products.Data);
        }
    }
}
