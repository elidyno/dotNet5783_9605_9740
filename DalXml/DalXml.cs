using DalApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    internal sealed class DalXml : IDal
    {
        private DalXml() { }
        public static IDal Instance { get; } = new DalXml();
 
        public IOrder Order { get; } = new Dal.Order();

        public IProduct Product { get; } = new Dal.Product();

        public IOrderItem OrderItem { get; } = new Dal.OrderItem();

    }
}
