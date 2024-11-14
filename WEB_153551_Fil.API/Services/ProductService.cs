using WWW_XXXX.Domain.Entities;
using WWW_XXXX.Domain.Models;

namespace WW_XXX.API.Services
{
    // WW_XXX.UI/Services/ProductService.cs
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ProductService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["ApiUrl"]);
        }

        public async Task<ResponseData<ListModel<Dish>>> GetProductListAsync(
            string? categoryNormalizedName, int pageNo = 1)
        {
            var url = $"api/dishes?pageNo={pageNo}";
            if (!string.IsNullOrEmpty(categoryNormalizedName))
                url += $"&categoryNormalizedName={categoryNormalizedName}";

            var response = await _httpClient.GetFromJsonAsync<ResponseData<ListModel<Dish>>>(url);
            return response!;
        }

        public async Task<ResponseData<Dish>> GetProductByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseData<Dish>>($"api/dishes/{id}");
            return response!;
        }

        public async Task UpdateProductAsync(int id, Dish product, IFormFile? formFile)
        {
            var content = new MultipartFormDataContent();

            if (formFile != null)
            {
                var fileContent = new StreamContent(formFile.OpenReadStream());
                content.Add(fileContent, "formFile", formFile.FileName);
            }

            content.Add(new StringContent(product.Name), "Name");
            content.Add(new StringContent(product.Description ?? ""), "Description");
            content.Add(new StringContent(product.CategoryId.ToString()), "CategoryId");
            content.Add(new StringContent(product.Calories.ToString()), "Calories");

            await _httpClient.PutAsync($"api/dishes/{id}", content);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/dishes/{id}");
        }

        public async Task<ResponseData<Dish>> CreateProductAsync(Dish product, IFormFile? formFile)
        {
            var content = new MultipartFormDataContent();

            if (formFile != null)
            {
                var fileContent = new StreamContent(formFile.OpenReadStream());
                content.Add(fileContent, "formFile", formFile.FileName);
            }

            content.Add(new StringContent(product.Name), "Name");
            content.Add(new StringContent(product.Description ?? ""), "Description");
            content.Add(new StringContent(product.CategoryId.ToString()), "CategoryId");
            content.Add(new StringContent(product.Calories.ToString()), "Calories");

            var response = await _httpClient.PostAsync("api/dishes", content);
            var result = await response.Content.ReadFromJsonAsync<ResponseData<Dish>>();
            return result!;
        }
    }
}
