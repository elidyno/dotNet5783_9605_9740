using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    /// <summary>
    /// implementation of craut method in dal list configration for order
    /// </summary>
    internal class Order : IOrder
    {
        const string s_orders = "orders";

        DataSurceInitialize.RunninId RunninId = new();

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int Add(DO.Order order)
        {
            order.Id = RunninId.OrderId;
            List<DO.Order?> orders = XMLTools.LoadListFromXMLSerializer<DO.Order>(s_orders);

            if (orders.FirstOrDefault(O => O?.Id == order.Id) != null)
                throw new AlreadyExistsException();
            
            orders.Add(order);
            XMLTools.SaveListToXMLSerializer(orders, s_orders);

            return order.Id;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Delete(int id)
        {
            List<DO.Order?> orders = XMLTools.LoadListFromXMLSerializer<DO.Order>(s_orders);

            if (orders.RemoveAll(O => O?.Id == id) == 0)
                throw new NotFoundException(); 

            XMLTools.SaveListToXMLSerializer(orders, s_orders);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DO.Order Get(Func<DO.Order?, bool>? select_)
        {
            List<DO.Order?> orders = XMLTools.LoadListFromXMLSerializer<DO.Order>(s_orders);

            return orders.Find(x => select_!(x)) ??
                throw new NotFoundException("The requested order does not exist");
           
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DO.Order?> GetList(Func<DO.Order?, bool>? select_ = null)
        {
            List<DO.Order?> orders = XMLTools.LoadListFromXMLSerializer<DO.Order>(s_orders);
            return select_ == null ? orders : orders.FindAll(x => select_(x));
            
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Update(DO.Order order)
        {
            Delete(order.Id);
            Add(order);
        }
    }
}
