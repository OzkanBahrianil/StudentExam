using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFremawork;
using ENOCA.Areas.Admin.Models;
using ENOCA.Models;
using EntityLayer.Concrate;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace ENOCA_APİ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ValuesController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        AppUserManeger aum = new AppUserManeger(new EfAppUserDal());
        private readonly UserManager<AppUser> _userManager;
        StudentManeger stm = new StudentManeger(new EfStudentDal());



        public ValuesController(SignInManager<AppUser> signInManager, IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public IActionResult ListStudent()
        {
            var values = aum.GetByFilterWithStudent(x => x.Students.studentName != null);

            return Ok(values);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetStudent(int id)
        {
            var values = aum.GetByFilterWithStudent(x => x.Students.studentName != null && x.Id == id).FirstOrDefault();
            if (values == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(values);
            }

        }
        [HttpPost]
        [Authorize]
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

                        return Ok();
                    }
                    else
                    {
                        await _userManager.DeleteAsync(finduser);
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError(item.Code, item.Description);
                        }
                    }

                }

                else
                {
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));

                }
            }
            return NotFound(p);
        }





        [HttpPut]
        [Authorize]
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
                    return Ok("Index");

                }
                else
                {

                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);

                    }

                    return NotFound(ModelState);
                }
            }
            else
            {
                resultasyc.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                return NotFound(resultasyc.Errors);
            }

        }



      
        [HttpPost("getToken")]
        [AllowAnonymous]
        public async Task<ActionResult> GetToken([FromBody]UserSignInViewModel userlogin)
        {

            var user = await _userManager.FindByNameAsync(userlogin.username);
            var result = await _signInManager.PasswordSignInAsync
                (userlogin.username, userlogin.password, userlogin.RememberMe, true);
            if (user != null && result.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();

        }
        [HttpGet("LogOut")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }


        private static string RandomPassword()
        {
            Random rnd = new Random();
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomLetter(3, true));
            builder.Append(rnd.Next(100, 999));
            builder.Append(RandomLetter(2, false));
            return builder.ToString();
        }
        private static string RandomLetter(int boyut, bool kucukHarf)
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
