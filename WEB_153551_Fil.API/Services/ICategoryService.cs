using WWW_XXXX.Domain.Entities;
using WWW_XXXX.Domain.Models;

namespace WW_XXX.API.Services
{

    // WW_XXX.UI/Services/ICategoryService.cs
    public interface ICategoryService
    {
        Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
