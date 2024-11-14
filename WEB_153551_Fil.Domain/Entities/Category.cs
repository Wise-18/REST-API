using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WWW_XXXX.Domain.Entities
{
    // Domain/Entities/Category.cs

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string NormalizedName { get; set; } = "";

        [JsonIgnore]  // Добавляем этот атрибут
        public List<Dish> Dishes { get; set; } = new();
    }
}
