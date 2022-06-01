using EntityLayer.Concrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IAppUserService : IGenericService<AppUser>
    {
       List<AppUser> GetByFilterWithStudent(Expression<Func<AppUser, bool>> filter);
       List<AppUser> GetByFilterWithStudentSearch(Expression<Func<AppUser, bool>> filter, string key);
       
    }
}
