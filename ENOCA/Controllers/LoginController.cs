using ENOCA.Models;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ENOCA.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        readonly UserManager<AppUser> _userManager;

        public LoginController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserSignInViewModel p)
        {
            if (ModelState.IsValid)
            {


                var result = await _signInManager.PasswordSignInAsync
                    (p.username, p.password, p.RememberMe, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {


                    TempData["AlertLogin"] = "Hata Giriş İşlemi Başarısız! Kullanıcı Adınız Veya Şifreniz Yanlış.";
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                TempData["AlertLogin"] = "Hata Giriş İşlemi Başarısız! Kullanıcı Adınız Veya Şifreniz Yanlış.";
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }
        [HttpGet]
        public IActionResult PasswordReset()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PasswordReset(ResetPasswordViewModel model)
        {
            AppUser user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(resetToken);
                var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
                var token1 = codeEncoded.Substring(0, 25);
                var token2 = codeEncoded.Substring(25);

                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;
                mail.To.Add(user.Email);
                mail.From = new MailAddress("healthprojectblog@gmail.com", "Şifre Güncelleme", Encoding.UTF8);
                mail.Subject = "Şifre Güncelleme Talebi";
                mail.Body = $"<a target=\"_blank\" href=\"https://localhost:44369{Url.Action("Re", "Login", new { userId = user.Id, token = token1, token2 = token2 })}\">Yeni şifre talebi için tıklayınız</a>";
                mail.IsBodyHtml = true;
                SmtpClient smp = new SmtpClient();
                smp.Credentials = new NetworkCredential("healthprojectblog@gmail.com", "11051998Fb.");
                smp.Port = 587;
                smp.Host = "smtp.gmail.com";
                smp.EnableSsl = true;
                smp.Send(mail);
                ViewBag.State = true;
            }
            else
                ViewBag.State = false;

            return View();
        }


        [HttpGet("[action]/{userId}/{token}/{token2}")]
        public IActionResult Re(string userId, string token, string token2)
        {
            return View();
        }


        [HttpPost("[action]/{userId}/{token}/{token2}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Re(UpdatePasswordViewModel model, string userId, string token, string token2)
        {
            var codeDecodedBytes = WebEncoders.Base64UrlDecode(token + token2);
            var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);


            AppUser user = await _userManager.FindByIdAsync(userId);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, codeDecoded, model.Password);
            if (result.Succeeded)
            {
                ViewBag.State = true;
                await _userManager.UpdateSecurityStampAsync(user);
                TempData["AlertUpdate"] = "Güncelleme İşlemi Başarılı...!";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.State = false;
                result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));

            }
            return View();

        }
    }
}
