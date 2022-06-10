using System.ComponentModel.DataAnnotations;

namespace StudentProject.Areas.Admin.Models
{
    public class UserSignUpViewModel
    {
        [Display(Name = "Ad")]
        [Required(ErrorMessage = "Lütfen Ad Giriniz")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Lütfen Email Giriniz")]
        public string Email { get; set; }

        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "Lütfen Soyad Giriniz")]
        public string Surname { get; set; }

        [Display(Name = "Öğrenci Numarası")]
        [Required(ErrorMessage = "Lütfen Öğrenci Numarası Giriniz")]
        public string Student_number { get; set; }

        [Display(Name = "Fakülte İsmi")]
        [Required(ErrorMessage = "Lütfen Fakülte İsmi Giriniz")]
        public string Faculty_Name { get; set; }
        
        [Display(Name = "Bölüm İsmi")]
        [Required(ErrorMessage = "Lütfen Bölüm İsmi Giriniz")]
        public string Section_name { get; set; }
    }
}
