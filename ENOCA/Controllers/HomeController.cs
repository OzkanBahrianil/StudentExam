using BusinessLayer.Concrete;
using DataAccessLayer.EntityFremawork;
using ENOCA.Models;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ENOCA.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManeger;
        AppUserManeger aum = new AppUserManeger(new EfAppUserDal());

        public HomeController(UserManager<AppUser> userManeger)
        {
            _userManeger = userManeger;
        }

        public IActionResult Index()
        {
            var username = User.Identity.Name;
            var writer = aum.GetByFilterWithStudent(x => x.UserName == username).FirstOrDefault();
            UpdateUserSettings userSettings = new UpdateUserSettings();
            userSettings.Email = writer.Email;
            return View(userSettings);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UpdateUserSettings updateUserSettings)
        {

            var username = User.Identity.Name;
            var p = await _userManeger.FindByNameAsync(username);
            p.Email = updateUserSettings.Email;
            var result = await _userManeger.UpdateAsync(p);
            if (result.Succeeded)
            {
                await _userManeger.UpdateSecurityStampAsync(p);
                return RedirectToAction("LogOut", "Login");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    TempData["AlertMessage"] = item.Description;
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPassword(UpdateUserSettings profileImage)
        {

            var username = User.Identity.Name;
            var p = await _userManeger.FindByNameAsync(username);

            var chechpassword = await _userManeger.ChangePasswordAsync(p, profileImage.PasswordOld, profileImage.Password);
            if (chechpassword.Succeeded)
            {
                p.PasswordHash = _userManeger.PasswordHasher.HashPassword(p, profileImage.Password);
                var result = await _userManeger.UpdateAsync(p);
                await _userManeger.UpdateSecurityStampAsync(p);
                return RedirectToAction("LogOut", "Login");

            }
            else
            {
                var error = chechpassword.Errors.ToList();
                foreach (var item in error)
                {
                    TempData["AlertMessage"] = item.Description;
                }
            }


            return RedirectToAction("Index");
        }
    }
}
