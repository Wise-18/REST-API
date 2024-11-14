using Microsoft.AspNetCore.Mvc;

namespace WW_XXX.UI.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        public IActionResult LogOut()
        {
            // Логика выхода
            return RedirectToAction("Index", "Home");
        }
    }
}
