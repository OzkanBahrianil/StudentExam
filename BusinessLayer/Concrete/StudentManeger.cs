using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class StudentManeger : IStudentService
    {
        IStudentDal _studentDal;

        public StudentManeger(IStudentDal studentDal)
        {
            _studentDal = studentDal;
        }

        public Student GetByIDT(int id)
        {
            return _studentDal.Get(x => x.Id == id);
        }

        public List<Student> GetListT()
        {
            return _studentDal.List();
        }
        public List<Student> Search(string key)
        {
            key = key.ToLower();
            return _studentDal.List().Where(p => p.Name.ToLower().Contains(key)
            || p.Faculty_Name.ToString().ToLower().Contains(key)
            || p.Student_number.ToString().ToLower().Contains(key)
            || p.Section_name.ToString().ToLower().Contains(key)
            || p.Surname.ToString().ToLower().Contains(key)).ToList();

        }

        public void TAdd(Student t)
        {
            _studentDal.Insert(t);
        }

        public void TDelete(Student t)
        {
            _studentDal.Delete(t);
        }

        public void TUpdate(Student t)
        {
            _studentDal.Update(t);
        }



    }
}
