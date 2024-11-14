using WWW_XXXX.Domain.Entities;
using WWW_XXXX.Domain.Models;

namespace WW_XXX.API.Services
{
    // WW_XXX.UI/Services/IProductService.cs
    public interface IProductService
    {
        Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1);
        Task<ResponseData<Dish>> GetProductByIdAsync(int id);
        Task UpdateProductAsync(int id, Dish product, IFormFile? formFile);
        Task DeleteProductAsync(int id);
        Task<ResponseData<Dish>> CreateProductAsync(Dish product, IFormFile? formFile);
    }
}
