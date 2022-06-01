using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class AppUserManeger : IAppUserService
    {
        IAppUserDal _appUserDal;

        public AppUserManeger(IAppUserDal appUserDal)
        {
            _appUserDal = appUserDal;
        }

        public AppUser GetByIDT(int id)
        {
            return _appUserDal.Get(x => x.Id == id);
        }

        public List<AppUser> GetListT()
        {
            return _appUserDal.List();
        }
        public List<AppUser> Search(string key)
        {
            key = key.ToLower();
            return _appUserDal.List().Where(p => p.UserName.ToLower().Contains(key)
            || p.Id.ToString().ToLower().Contains(key)
            || p.Email.ToString().ToLower().Contains(key)
            || p.PhoneNumber.ToString().ToLower().Contains(key)).ToList();

        }

        public void TAdd(AppUser t)
        {
            _appUserDal.Insert(t);
        }

        public void TDelete(AppUser t)
        {
            _appUserDal.Delete(t);
        }

        public void TUpdate(AppUser t)
        {
            _appUserDal.Update(t);
        }
        public List<AppUser> GetByFilterWithStudent(Expression<Func<AppUser, bool>> filter)
        {
            return _appUserDal.GetByFilterWithSt(filter);
        }
        public List<AppUser> GetByFilterWithStudentSearch(Expression<Func<AppUser, bool>> filter, string key)
        {


            key = key.ToLower();
            var values = _appUserDal.GetByFilterWithSt(filter);

            var result = values.Where(p => p.Students.Name.ToLower().Contains(key)
              || p.Students.Section_name.ToString().ToLower().Contains(key)
              || p.Students.Surname.ToString().ToLower().Contains(key)
              || p.Students.Faculty_Name.ToString().ToLower().Contains(key)).ToList();
            return result;
        }

    }
}
