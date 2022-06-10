using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFremawork;
using EntityLayer.Concrate;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace StudentProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SearchController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        StudentManeger stm = new StudentManeger(new EfStudentDal());

        public SearchController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult StudentList()
        {
            var values = stm.GetListT();
            var jsonWriters = JsonConvert.SerializeObject(values);
            return Json(jsonWriters);
        }

        public IActionResult StudentByID(int Id)
        {

            var values = stm.GetByIDT(Id);
            var jsonWriters = JsonConvert.SerializeObject(values);
            return Json(jsonWriters);
        }
        public async Task<IActionResult> StudentUpdate(Student w)
        {

            var finduser = await _userManager.FindByIdAsync(w.UserID.ToString());
            var checkusername = finduser.UserName;

            finduser.UserName = w.studentNumber;




            var resultasyc = await _userManager.UpdateAsync(finduser);
            if (resultasyc.Succeeded)
            {

                if (checkusername != w.studentNumber)
                {
                    MailMessage mail = new MailMessage();
                    mail.IsBodyHtml = true;
                    mail.To.Add(finduser.Email);
                    mail.From = new MailAddress("healthprojectblog@gmail.com", "Güncelleme İşlemi", Encoding.UTF8);
                    mail.Subject = "Güncelleme İşlemi tamamlandı Kullanıcı Adı";
                    mail.Body = $"<p>Kullanıcı Adınız: {w.studentNumber} olarak güncellenmiştir.</p>";
                    mail.IsBodyHtml = true;
                    SmtpClient smp = new SmtpClient();
                    smp.Credentials = new NetworkCredential("healthprojectblog@gmail.com", "11051998Fb.");
                    smp.Port = 587;
                    smp.Host = "smtp.gmail.com";
                    smp.EnableSsl = true;
                    smp.Send(mail);
                }

                StudentValidationRules cv = new StudentValidationRules();
                ValidationResult result = cv.Validate(w);
                if (result.IsValid)
                {


                    stm.TUpdate(w);
                    var jsonWriters = JsonConvert.SerializeObject(w);
                    return Json(new { result = true, jsonWriters });

                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        return Json(new { result = false, error = item.ErrorMessage });

                    }
                }
            }
            else
            {
                foreach (var item in resultasyc.Errors)
                {
                    return Json(new { result = false, error = item.Description });
                }



            }
            return Json(new { result = false, error = "Güncelleme Başarısız" });


        }
    }
}
