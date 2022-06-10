using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFremawork;
using StudentProject.Areas.Admin.Models;
using EntityLayer.Concrate;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace StudentProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class StudentController : Controller
    {
        StudentManeger stm = new StudentManeger(new EfStudentDal());
        private readonly UserManager<AppUser> _userManager;
        AppUserManeger aum = new AppUserManeger(new EfAppUserDal());

        public StudentController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var values = aum.GetByFilterWithStudent(x => x.Students.studentName != null);

            return View(values);
        }

        [HttpGet]
        public IActionResult StudentUpdate(int id)
        {
            var values = aum.GetByFilterWithStudent(x => x.Students.studentName != null && x.Id == id).FirstOrDefault();

            UserUpdateViewModel userUpdate = new UserUpdateViewModel()
            {
                Section_name = values.Students.studentDepartment,
                Faculty_Name = values.Students.studentFacultyName,
                Student_number = values.Students.studentNumber,
                Surname = values.Students.studentSurname,
                UserId = values.Id,
                Name = values.Students.studentName,
                StudentId = values.Students.studentId,
                Email = values.Email

            };
            return View(userUpdate);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentUpdate(UserUpdateViewModel p)
        {

            var finduser = await _userManager.FindByIdAsync(p.UserId.ToString());
            var checkusername = finduser.UserName;

           
                finduser.Email = p.Email;
                finduser.UserName = p.Student_number;


                var resultasyc = await _userManager.UpdateAsync(finduser);
                if (resultasyc.Succeeded)
                {
                    var student = stm.GetByIDT(p.StudentId);
                    student.studentDepartment = p.Section_name;
                    student.studentFacultyName = p.Faculty_Name;
                    student.studentNumber = p.Student_number;
                    student.studentSurname = p.Surname;
                    student.studentName = p.Name;
                    if (checkusername != p.Student_number)
                    {
                        MailMessage mail = new MailMessage();
                        mail.IsBodyHtml = true;
                        mail.To.Add(p.Email);
                        mail.From = new MailAddress("healthprojectblog@gmail.com", "Güncelleme İşlemi", Encoding.UTF8);
                        mail.Subject = "Güncelleme İşlemi tamamlandı Kullanıcı Adı";
                        mail.Body = $"<p>Kullanıcı Adınız: {p.Student_number} olarak güncellenmiştir.</p>";
                        mail.IsBodyHtml = true;
                        SmtpClient smp = new SmtpClient();
                        smp.Credentials = new NetworkCredential("healthprojectblog@gmail.com", "11051998Fb.");
                        smp.Port = 587;
                        smp.Host = "smtp.gmail.com";
                        smp.EnableSsl = true;
                        smp.Send(mail);
                    }

                    StudentValidationRules cv = new StudentValidationRules();
                    ValidationResult result = cv.Validate(student);
                    if (result.IsValid)
                    {


                        stm.TUpdate(student);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                        }
                    }
                }
                else
                {
                    resultasyc.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                }
            
          

            
            return View(p);
        }


        public IActionResult AddStudent()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudent(UserSignUpViewModel p)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser()
                {
                    Email = p.Email,
                    UserName = p.Student_number,
                };

                var password = RandomPassword();

                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    var finduser = await _userManager.FindByEmailAsync(p.Email);
                    Student student = new Student();
                    student.studentFacultyName = p.Faculty_Name;
                    student.studentNumber = p.Student_number;
                    student.studentSurname = p.Surname;
                    student.UserID = finduser.Id;
                    student.studentName = p.Name;
                    student.studentDepartment = p.Section_name;
                    StudentValidationRules sv = new StudentValidationRules();
                    ValidationResult validateResult = sv.Validate(student);
                    if (validateResult.IsValid)
                    {
                        stm.TAdd(student);

                        MailMessage mail = new MailMessage();
                        mail.IsBodyHtml = true;
                        mail.To.Add(user.Email);
                        mail.From = new MailAddress("healthprojectblog@gmail.com", "Kayıt İşlemi", Encoding.UTF8);
                        mail.Subject = "Kayıt İşlemi tamamlandı Kullanıcı Adı ve Parola";
                        mail.Body = $"<p>Kullanıcı Adı: {p.Student_number} Şifre: {password} </p>";
                        mail.IsBodyHtml = true;
                        SmtpClient smp = new SmtpClient();
                        smp.Credentials = new NetworkCredential("healthprojectblog@gmail.com", "11051998Fb.");
                        smp.Port = 587;
                        smp.Host = "smtp.gmail.com";
                        smp.EnableSsl = true;
                        smp.Send(mail);
                        await _userManager.AddToRoleAsync(finduser, "Student");
                        TempData["Success"] = "Başarılı Bir Şekilde Öğrenci Eklenmiştir";
                        return RedirectToAction("AddStudent");
                    }
                    else
                    {
                        await _userManager.DeleteAsync(finduser);
                        foreach (var item in result.Errors)
                        {
                            TempData["Errors"] = item.Description;
                        }
                    }

                }

                else
                {
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));

                }
            }
            return View(p);
        }

        private string RandomPassword()
        {
            Random rnd = new Random();
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomLetter(3, true));
            builder.Append(rnd.Next(100, 999));
            builder.Append(RandomLetter(2, false));
            return builder.ToString();
        }
        public string RandomLetter(int boyut, bool kucukHarf)
        {
            string harfler = "";
            int sayi, min = 65;
            char harf;

            if (kucukHarf)
            {
                min = 97;
            }

            for (int i = 0; i < boyut; i++)
            {
                var rnd = new Random();
                sayi = rnd.Next(min, min + 25);
                harf = Convert.ToChar(sayi);
                harfler += harf;
            }
            return harfler;
        }

    }
}
