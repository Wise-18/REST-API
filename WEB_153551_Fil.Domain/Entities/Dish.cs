using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WWW_XXXX.Domain.Entities
{
    // Domain/Entities/Dish.cs
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int Calories { get; set; }
        public string? Image { get; set; }
        public string? MimeType { get; set; }

        [JsonIgnore]  // Добавляем этот атрибут
        public Category? Category { get; set; }
    }

}
