using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IGenericService<T>
    {

        List<T> GetListT();
        void TAdd(T t);

        T GetByIDT(int id);

        void TDelete(T t);

        void TUpdate(T t);
    }
}
