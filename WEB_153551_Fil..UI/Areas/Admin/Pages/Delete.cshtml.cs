using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using WW_XXX.API.Services;
using WWW_XXXX.Domain.Entities;

namespace WW_XXX.UI.Areas.Admin.Pages
{
    // WW_XXX.UI/Areas/Admin/Pages/Delete.cshtml.cs
    public class DeleteModel : PageModel
    {
        private readonly IProductService _productService;

        public DeleteModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Dish Product { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (!response.Successfull)
                return NotFound();

            Product = response.Data!;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToPage("./Index");
        }
    }
}
