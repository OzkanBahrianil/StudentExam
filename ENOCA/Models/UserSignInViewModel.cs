using System.ComponentModel.DataAnnotations;

namespace ENOCA.Models
{
    public class UserSignInViewModel
    {
        [Required(ErrorMessage = "Lütfen Kullanıcı Adını Gİrin")]
        public string username { get; set; }

        [Required(ErrorMessage = "Lütfen Şifrenizi Adını Gİrin")]
        public string password { get; set; }

        public bool RememberMe { get; set; }
    }
}
