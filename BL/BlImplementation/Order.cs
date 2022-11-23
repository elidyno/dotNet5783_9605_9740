using BlApi;
using BO;

namespace BlImplementation
{
    internal class Order : IOrder
    {
        public BO.Order Get(int orderId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrderForList> GetList()
        {
            throw new NotImplementedException();
        }

        public OrderTracking GetTracking(int orderId)
        {
            throw new NotImplementedException();
        }

        public BO.Order UpdateOrderDelivery(int orderId)
        {
            throw new NotImplementedException();
        }

        public BO.Order UpdateOrderSheep(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}
