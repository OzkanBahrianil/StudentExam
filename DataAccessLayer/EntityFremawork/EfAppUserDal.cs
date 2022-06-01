using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityFremawork
{
    public class EfAppUserDal : GenericRepository<AppUser>, IAppUserDal
    {
        public List<AppUser> GetByFilterWithSt(Expression<Func<AppUser, bool>> filter = null)
        {
            using (var c = new Context()) { 
            if (filter == null)
                return c.Users.Include(x => x.Students).ToList();
            else
                return c.Users.Where(filter).Include(x => x.Students).ToList();
            }
        }
    }
}
