using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WWW_XXXX.Domain.Data;
using WWW_XXXX.Domain.Entities;
using WWW_XXXX.Domain.Models;

namespace WW_XXX.API.Controllers
{
    // WW_XXX.API/Controllers/DishesController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class DishesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private const int PageSize = 3;

        public DishesController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseData<ListModel<Dish>>>> GetDishes(
            string? categoryNormalizedName, int pageNo = 1)
        {
            try
            {
                var query = _context.Dishes.AsQueryable();

                if (!string.IsNullOrEmpty(categoryNormalizedName))
                {
                    query = query.Where(d => d.Category.NormalizedName == categoryNormalizedName);
                }

                var count = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(count / (double)PageSize);
                pageNo = Math.Max(1, Math.Min(pageNo, totalPages));

                var items = await query
                    .Skip((pageNo - 1) * PageSize)
                    .Take(PageSize)
                    .Include(d => d.Category)
                    .ToListAsync();

                var result = new ListModel<Dish>
                {
                    Items = items,
                    CurrentPage = pageNo,
                    TotalPages = totalPages
                };

                return ResponseData<ListModel<Dish>>.Success(result);
            }
            catch (Exception ex)
            {
                return ResponseData<ListModel<Dish>>.Error(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseData<Dish>>> GetDish(int id)
        {
            try
            {
                var dish = await _context.Dishes
                    .Include(d => d.Category)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (dish == null)
                    return ResponseData<Dish>.Error("Dish not found");

                return ResponseData<Dish>.Success(dish);
            }
            catch (Exception ex)
            {
                return ResponseData<Dish>.Error(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ResponseData<Dish>>> CreateDish(
            [FromForm] Dish dish,
            IFormFile? formFile)
        {
            try
            {
                if (formFile != null)
                {
                    dish.Image = await SaveImage(formFile);
                    dish.MimeType = formFile.ContentType;
                }

                _context.Dishes.Add(dish);
                await _context.SaveChangesAsync();

                return ResponseData<Dish>.Success(dish);
            }
            catch (Exception ex)
            {
                return ResponseData<Dish>.Error(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseData<Dish>>> UpdateDish(
            int id,
            [FromForm] Dish dish,
            IFormFile? formFile)
        {
            try
            {
                var existingDish = await _context.Dishes.FindAsync(id);
                if (existingDish == null)
                    return ResponseData<Dish>.Error("Dish not found");

                if (formFile != null)
                {
                    if (!string.IsNullOrEmpty(existingDish.Image))
                    {
                        DeleteImage(existingDish.Image);
                    }

                    existingDish.Image = await SaveImage(formFile);
                    existingDish.MimeType = formFile.ContentType;
                }

                existingDish.Name = dish.Name;
                existingDish.Description = dish.Description;
                existingDish.CategoryId = dish.CategoryId;
                existingDish.Calories = dish.Calories;

                await _context.SaveChangesAsync();

                return ResponseData<Dish>.Success(existingDish);
            }
            catch (Exception ex)
            {
                return ResponseData<Dish>.Error(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseData<bool>>> DeleteDish(int id)
        {
            try
            {
                var dish = await _context.Dishes.FindAsync(id);
                if (dish == null)
                    return ResponseData<bool>.Error("Dish not found");

                if (!string.IsNullOrEmpty(dish.Image))
                {
                    DeleteImage(dish.Image);
                }

                _context.Dishes.Remove(dish);
                await _context.SaveChangesAsync();

                return ResponseData<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return ResponseData<bool>.Error(ex.Message);
            }
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            var uploadPath = Path.Combine(_env.WebRootPath, "images");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        private void DeleteImage(string fileName)
        {
            var filePath = Path.Combine(_env.WebRootPath, "images", fileName);
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }
    }
}
