using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudentProject.Controllers
{
    [AllowAnonymous]
    public class Error : Controller
    {
        public IActionResult Error1(int code)
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
