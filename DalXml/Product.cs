using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    internal class Product : IProduct
    {
        public int Add(DO.Product item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int item)
        {
            throw new NotImplementedException();
        }

        public DO.Product Get(Func<DO.Product?, bool>? select_)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DO.Product?> GetList(Func<DO.Product?, bool>? select_ = null)
        {
            throw new NotImplementedException();
        }

        public void Update(DO.Product item)
        {
            throw new NotImplementedException();
        }
    }
}
