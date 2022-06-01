using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName) => new IdentityError { Code = "DuplicateUserName", Description = $"\"{userName}\" kullanıcı adı kullanılmaktadır." };
        public override IdentityError InvalidUserName(string userName) => new IdentityError { Code = "InvalidUserName", Description = "Geçersiz kullanıcı adı." };
        public override IdentityError DuplicateEmail(string email) => new IdentityError { Code = "DuplicateEmail", Description = $"\"{email}\" başka bir kullanıcı tarafından kullanılmaktadır." };
        public override IdentityError InvalidEmail(string email) => new IdentityError { Code = "InvalidEmail", Description = "Geçersiz email." };
        public override IdentityError InvalidToken() => new IdentityError { Code = "InvalidToken", Description = "Geçersiz Token." };
        public override IdentityError PasswordRequiresLower() => new IdentityError { Code = "PasswordRequiresLower", Description = "Şifre En az 1 Adet Küçük Karakter İçermelidir" };
        public override IdentityError PasswordRequiresUpper() => new IdentityError { Code = "PasswordRequiresUpper", Description = "Şifre En az 1 Adet Büyük Karakter İçermelidir" };
        public override IdentityError UserAlreadyHasPassword() => new IdentityError { Code = "UserAlreadyHasPassword", Description = "Şifreniz Eski Şifreniz İle Aynı Olamaz" };
        public override IdentityError UserLockoutNotEnabled() => new IdentityError { Code = "UserLockoutNotEnabled", Description = "Giriş Engellendi. Daha Sonra Tekrar Deneyiniz" };
        public override IdentityError PasswordMismatch() => new IdentityError { Code = "PasswordMismatch", Description = "Şifreler Aynı Değil" };

    }
}
