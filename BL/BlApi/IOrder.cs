using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface IOrder
    {
        public Order UpdateOrderSheep(int orderId);
        public Order UpdateOrderDelivery(int orderId);
        public Order Get(int orderId);
        public IEnumerable<OrderForList> GetList();
        public OrderTracking GetTracking(int orderId);
    }
}
