using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    /// <summary>
    ///  Croud method for Dal object
    /// </summary>
    public interface ICrud <T> where T : struct
    {
        public int Add(T item);
        public void Delete(int item);
        public void Update (T item);
        public IEnumerable<T?> GetList(Func<T?, bool>? select_ = null);
        public T Get(Func<T?, bool>? select_);
    }
}
