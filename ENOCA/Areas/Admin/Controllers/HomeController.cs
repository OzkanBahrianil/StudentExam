using BusinessLayer.Concrete;
using DataAccessLayer.EntityFremawork;
using ENOCA.Areas.Admin.Models;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ENOCA.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        AppUserManeger aum = new AppUserManeger(new EfAppUserDal());

        private readonly UserManager<AppUser> _userManeger;

        public HomeController(UserManager<AppUser> userManeger)
        {
            _userManeger = userManeger;
        }
        [HttpGet]
        public IActionResult Index(string sortOrder, string SearchString, int PageSize = 10, int page = 1)
        {
        

            ViewData["CurrentFilterSearch"] = SearchString;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = sortOrder == "Name" ? "NameDesc" : "Name";
            ViewData["SurnameSortParam"] = sortOrder == "Surname" ? "SurnameDesc" : "Surname";
            ViewData["NumberSortParam"] = sortOrder == "Number" ? "NumberDesc" : "Number";
            ViewData["FacultySortParam"] = sortOrder == "Faculty" ? "FacultyDesc" : "Faculty";
            ViewData["SectionSortParam"] = sortOrder == "Section" ? "SectionDesc" : "Section";
            ViewData["EmailSortParam"] = sortOrder == "Email" ? "EmailDesc" : "Email";
            ViewBag.PageSize = new List<SelectListItem>()
            {
                new SelectListItem() { Value="5", Text= "5" },
                new SelectListItem() { Value="10", Text= "10" },
                new SelectListItem() { Value="15", Text= "15" },
                new SelectListItem() { Value="25", Text= "25" },
                new SelectListItem() { Value="50", Text= "50" },
            };


            ViewBag.psize = PageSize;
            if (!String.IsNullOrEmpty(SearchString))
            {
                var valuesSearch = aum.GetByFilterWithStudentSearch(x => x.Students.Name != null, SearchString);

                switch (sortOrder)
                {
                    case "Name":
                        valuesSearch = valuesSearch.OrderBy(x => x.Students.Name).ToList();
                        break;
                    case "NameDesc":
                        valuesSearch = valuesSearch.OrderByDescending(x => x.Students.Name).ToList();
                        break;
                    case "Surname":
                        valuesSearch = valuesSearch.OrderBy(s => s.Students.Surname).ToList();
                        break;
                    case "SurnameDesc":
                        valuesSearch = valuesSearch.OrderByDescending(s => s.Students.Surname).ToList();
                        break;
                    case "Number":
                        valuesSearch = valuesSearch.OrderBy(s => s.Students.Student_number).ToList();
                        break;
                    case "NumberDesc":
                        valuesSearch = valuesSearch.OrderByDescending(s => s.Students.Student_number).ToList();
                        break;
                    case "Faculty":
                        valuesSearch = valuesSearch.OrderBy(s => s.Students.Faculty_Name).ToList();
                        break;
                    case "FacultyDesc":
                        valuesSearch = valuesSearch.OrderByDescending(s => s.Students.Faculty_Name).ToList();
                        break;
                    case "Section":
                        valuesSearch = valuesSearch.OrderBy(s => s.Students.Section_name).ToList();
                        break;
                    case "SectionDesc":
                        valuesSearch = valuesSearch.OrderByDescending(s => s.Students.Section_name).ToList();
                        break;
                    case "Email":
                        valuesSearch = valuesSearch.OrderBy(s => s.Email).ToList();
                        break;
                    case "EmailDesc":
                        valuesSearch = valuesSearch.OrderByDescending(s => s.Email).ToList();
                        break;

                    default:
                        valuesSearch = valuesSearch.OrderByDescending(s => s.Id).ToList();
                        break;
                }

                return View(valuesSearch.ToPagedList(page, PageSize));
            }
            else
            {
                var values = aum.GetByFilterWithStudent(x => x.Students.Name != null);
                switch (sortOrder)
                {
                    case "Name":
                        values = values.OrderBy(x => x.Students.Name).ToList();
                        break;
                    case "NameDesc":
                        values = values.OrderByDescending(x => x.Students.Name).ToList();
                        break;
                    case "Surname":
                        values = values.OrderBy(s => s.Students.Surname).ToList();
                        break;
                    case "SurnameDesc":
                        values = values.OrderByDescending(s => s.Students.Surname).ToList();
                        break;
                    case "Number":
                        values = values.OrderBy(s => s.Students.Student_number).ToList();
                        break;
                    case "NumberDesc":
                        values = values.OrderByDescending(s => s.Students.Student_number).ToList();
                        break;
                    case "Faculty":
                        values = values.OrderBy(s => s.Students.Faculty_Name).ToList();
                        break;
                    case "FacultyDesc":
                        values = values.OrderByDescending(s => s.Students.Faculty_Name).ToList();
                        break;
                    case "Section":
                        values = values.OrderBy(s => s.Students.Section_name).ToList();
                        break;
                    case "SectionDesc":
                        values = values.OrderByDescending(s => s.Students.Section_name).ToList();
                        break;
                    case "Email":
                        values = values.OrderBy(s => s.Email).ToList();
                        break;
                    case "EmailDesc":
                        values = values.OrderByDescending(s => s.Email).ToList();
                        break;

                    default:
                        values = values.OrderByDescending(s => s.Id).ToList();
                        break;
                }
                return View(values.ToPagedList(page, PageSize));

            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(List<RoleAssignViewModel> model)
        {


            foreach (var item in model)
            {
                var userId = item.UserId;
                var user = _userManeger.Users.FirstOrDefault(x => x.Id == userId);
                if (item.Exists)
                {
                    await _userManeger.AddToRoleAsync(user, item.Name);
                }
                else
                {
                    await _userManeger.RemoveFromRoleAsync(user, item.Name);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
