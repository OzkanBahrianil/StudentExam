using EntityLayer.Concrate;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class StudentValidationRules : AbstractValidator<Student>
    {
        public StudentValidationRules()
        {
            RuleFor(x => x.studentName).NotEmpty().WithMessage("İsim Boş Bırakılamaz").MinimumLength(2).WithMessage("İsim En Az 2 Karekter Yazılabilir").MaximumLength(200).WithMessage("İsim En Fazla 200 Karekter Yazılabilir");
            RuleFor(x => x.studentSurname).NotEmpty().WithMessage("Soyisim Boş Bırakılamaz").MinimumLength(2).WithMessage("Soyisim En Az 2 Karekter Yazılabilir").MaximumLength(200).WithMessage("Soyisim En Fazla 200 Karekter Yazılabilir");
            RuleFor(x => x.studentFacultyName).NotEmpty().WithMessage("Fakülte İsmi Boş Bırakılamaz").MinimumLength(2).WithMessage("Fakülte İsmi En Az 2 Karekter Yazılabilir").MaximumLength(300).WithMessage("Fakülte İsmi En Fazla 300 Karekter Yazılabilir");
            RuleFor(x => x.studentDepartment).NotEmpty().WithMessage("Bölüm Boş Bırakılamaz").MinimumLength(2).WithMessage("Bölüm En Az 2 Karekter Yazılabilir").MaximumLength(300).WithMessage("Bölüm En Fazla 300 Karekter Yazılabilir");
          

        }
    }
}
