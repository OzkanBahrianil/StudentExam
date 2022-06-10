using StudentProject.Areas.Admin.Models;

using EntityLayer.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StudentProject.Areas.Api.Controllers
{
    [Area("Api")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {


        public async Task<IActionResult> Index()
        {
            try
            {
                var token2 = HttpContext.Session.GetString("Token");

                var httpClient2 = new HttpClient();
                httpClient2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);

                var responseMessagee = await httpClient2.GetAsync("https://localhost:44324/api/Values");
                var jsonStringg = await responseMessagee.Content.ReadAsStringAsync();
                var values2 = JsonConvert.DeserializeObject<List<AppUser>>(jsonStringg);
          

                return View(values2);


            }
            catch (Exception ex)
            {
               
                throw new("",ex);
            }


        }
        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddStudent(UserSignUpViewModel UserSignUpViewModel)
        {

            var token = HttpContext.Session.GetString("Token");
            var httpClient = new HttpClient();
            var jsonStudents = JsonConvert.SerializeObject(UserSignUpViewModel);
            StringContent stringContent = new StringContent(jsonStudents, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.PostAsync("https://localhost:44324/api/Values", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["Success"] = "Ekleme İşlemi Başarılı";
                return RedirectToAction("Index");
            }
            else
            {

                return View(UserSignUpViewModel);

            }

        }

        [HttpGet]
        public async Task<IActionResult> StudentUpdate(int id)
        {
            var token = HttpContext.Session.GetString("Token");
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.GetAsync("https://localhost:44324/api/Values/" + id);

            if (responseMessage.IsSuccessStatusCode)
            {


                var jsonString = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<AppUser>(jsonString);

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
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> StudentUpdate(UserUpdateViewModel p)
        {
            var token = HttpContext.Session.GetString("Token");
            var httpClient = new HttpClient();
            var jsonStudents = JsonConvert.SerializeObject(p);
            StringContent stringContent = new StringContent(jsonStudents, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.PutAsync("https://localhost:44324/api/Values", stringContent);


            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {

                return View(p);
            }



        }

    }
}

