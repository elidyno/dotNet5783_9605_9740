using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    internal class OrderItem : IOrderItem
    {
        public int Add(DO.OrderItem item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int item)
        {
            throw new NotImplementedException();
        }

        public DO.OrderItem Get(Func<DO.OrderItem?, bool>? select_)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DO.OrderItem?> GetList(Func<DO.OrderItem?, bool>? select_ = null)
        {
            throw new NotImplementedException();
        }

        public void Update(DO.OrderItem item)
        {
            throw new NotImplementedException();
        }
    }
}
