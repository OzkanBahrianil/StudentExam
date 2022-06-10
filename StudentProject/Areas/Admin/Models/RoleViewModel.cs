using System.ComponentModel.DataAnnotations;

namespace StudentProject.Areas.Admin.Models
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Lütfen Rol Adı Giriniz.!!!")]
        public string Name { get; set; }
    }
}
