using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using WW_XXX.API.Services;
using WWW_XXXX.Domain.Entities;

namespace WW_XXX.UI.Areas.Admin.Pages
{
    // WW_XXX.UI/Areas/Admin/Pages/Edit.cshtml.cs
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public EditModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public Dish Product { get; set; } = new();

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public List<Category> Categories { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            if (categoriesResponse.Successfull)
                Categories = categoriesResponse.Data!;

            var productResponse = await _productService.GetProductByIdAsync(id);
            if (!productResponse.Successfull)
                return NotFound();

            Product = productResponse.Data!;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                var response = await _categoryService.GetCategoryListAsync();
                if (response.Successfull)
                    Categories = response.Data!;
                return Page();
            }

            await _productService.UpdateProductAsync(id, Product, ImageFile);
            return RedirectToPage("./Index");
        }
    }

}
