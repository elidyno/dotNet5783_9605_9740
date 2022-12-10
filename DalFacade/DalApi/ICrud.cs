using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public interface ICrud <T> where T : struct
    {
        public int Add(T item);
        public void Delete(int item);
        public void Update (T item);
        public T Get (int item);
        public IEnumerable<T?> GetList(Func<T?, bool>? select = null); 
    }
}
