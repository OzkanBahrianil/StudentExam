using ENOCA.Areas.Admin.Models;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ENOCA.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<AppRole> _roleManeger;

        public RoleController(RoleManager<AppRole> roleManeger)
        {
            _roleManeger = roleManeger;
        }

        public IActionResult Index()
        {
            var values = _roleManeger.Roles.ToList();
            return View(values);
        }
        public async Task<IActionResult> AddRole(RoleViewModel p)
        {
            if (ModelState.IsValid)
            {
                AppRole role = new AppRole
                {
                    Name = p.Name
                };
                var result = await _roleManeger.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["AlertMessage"] = "Hatalı Giriş Yaptınız";
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteRole(int Id)
        {
            var values = _roleManeger.Roles.FirstOrDefault(x => x.Id == Id);
            var result = await _roleManeger.DeleteAsync(values);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["AlertMessage"] = "Hatalı İşlem Yaptınız";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UpdateRole(int Id)
        {
            var values = _roleManeger.Roles.FirstOrDefault(x => x.Id == Id);
            RoleUpdateViewModel model = new RoleUpdateViewModel
            {
                Id = values.Id,
                Name = values.Name
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole(RoleUpdateViewModel p)
        {
            var values = _roleManeger.Roles.Where(x => x.Id == p.Id).FirstOrDefault();
            values.Name = p.Name;
            var result = await _roleManeger.UpdateAsync(values);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["AlertMessage"] = "Hatalı Giriş Yaptınız";

            }
            return View(values);
        }
    }
}
