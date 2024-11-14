using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWW_XXXX.Domain.Models
{
    // Domain/Models/ListModel.cs
    public class ListModel<T>
    {
        public List<T> Items { get; set; } = new();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
    }
}
