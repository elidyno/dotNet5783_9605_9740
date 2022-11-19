using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    internal interface ICrud <T>
    {
        public T Add(T item);
        public void Delete(T item);

        public void Update (T item);

        public T Get (T item);

        public IEnumerable<T> GetList (T item);  


    }
}
