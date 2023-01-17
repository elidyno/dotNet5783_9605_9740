using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    internal class Order : IOrder
    {
        public int Add(DO.Order item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int item)
        {
            throw new NotImplementedException();
        }

        public DO.Order Get(Func<DO.Order?, bool>? select_)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DO.Order?> GetList(Func<DO.Order?, bool>? select_ = null)
        {
            throw new NotImplementedException();
        }

        public void Update(DO.Order item)
        {
            throw new NotImplementedException();
        }
    }
}
