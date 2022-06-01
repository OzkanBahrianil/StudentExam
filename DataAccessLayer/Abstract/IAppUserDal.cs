using EntityLayer.Concrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IAppUserDal : IRepository<AppUser>
    {

        List<AppUser> GetByFilterWithSt(Expression<Func<AppUser, bool>> filter = null);
    }
}
