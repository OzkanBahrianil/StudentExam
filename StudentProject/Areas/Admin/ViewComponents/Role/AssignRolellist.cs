using StudentProject.Areas.Admin.Models;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentProject.Areas.Admin.ViewComponents.Role
{ 
    public class AssignRolellist : ViewComponent
    {
        private readonly RoleManager<AppRole> _roleManeger;
        private readonly UserManager<AppUser> _userManeger;


        public AssignRolellist(RoleManager<AppRole> roleManeger, UserManager<AppUser> userManeger)
        {
            _roleManeger = roleManeger;
            _userManeger = userManeger;
        }



      
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var roles = _roleManeger.Roles.ToList();
            var User = _userManeger.Users.FirstOrDefault(x => x.Id == id);
            TempData["UserId"] = User.Id;
            var userRoles = await _userManeger.GetRolesAsync(User);

            List<RoleAssignViewModel> model = new List<RoleAssignViewModel>();
            foreach (var item in roles)
            {
                RoleAssignViewModel m = new RoleAssignViewModel();
                m.RoleId = item.Id;
                m.UserId = User.Id;
                m.Name = item.Name;
                m.Exists = userRoles.Contains(item.Name);
                model.Add(m);
            }
            return View(model);
        }
    }
}
