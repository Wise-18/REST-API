using Microsoft.EntityFrameworkCore;
using WWW_XXXX.Domain.Data;
using WWW_XXXX.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Добавляем контекст базы данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("WW_XXX.API") // Указываем сборку для миграций
    ));

// Добавляем CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});

var app = builder.Build();

// Применяем миграции и добавляем начальные данные
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
        // Добавляем начальные данные
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Name = "Первые блюда", NormalizedName = "first-courses" },
                new Category { Name = "Салаты", NormalizedName = "salads" },
                new Category { Name = "Напитки", NormalizedName = "drinks" }
            );
            context.SaveChanges();
        }

        if (!context.Dishes.Any())
        {
            var category = context.Categories.First();
            context.Dishes.AddRange(
                new Dish
                {
                    Name = "Борщ",
                    Description = "Классический борщ",
                    Calories = 350,
                    CategoryId = category.Id
                },
                new Dish
                {
                    Name = "Суп-харчо",
                    Description = "Острый суп",
                    Calories = 400,
                    CategoryId = category.Id
                }
            );
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Произошла ошибка при миграции базы данных");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();