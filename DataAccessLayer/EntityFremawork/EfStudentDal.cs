using DataAccessLayer.Abstract;
using DataAccessLayer.Repositories;
using EntityLayer.Concrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityFremawork
{
    public class EfStudentDal : GenericRepository<Student>, IStudentDal
    {
    }
}
