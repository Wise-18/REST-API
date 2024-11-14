using Microsoft.AspNetCore.Mvc;
using WW_XXX.UI.Models;

namespace WW_XXX.UI.Components
{
    public class CartInfoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(new CartInfoViewModel { TotalPrice = 0, ItemCount = 0 });
        }
    }
}
