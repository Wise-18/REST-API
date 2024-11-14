// WW_XXX.UI/Services/CategoryService.cs
using WW_XXX.API.Services;
using WWW_XXXX.Domain.Entities;
using WWW_XXXX.Domain.Models;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(HttpClient httpClient, IConfiguration configuration, ILogger<CategoryService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        _httpClient.BaseAddress = new Uri(_configuration["ApiUrl"]);
    }

    public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseData<List<Category>>>("api/categories");
            return response ?? ResponseData<List<Category>>.Error("Не удалось получить данные");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении списка категорий");
            return ResponseData<List<Category>>.Error($"Сервис временно недоступен: {ex.Message}");
        }
    }
}