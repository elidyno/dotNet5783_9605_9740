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
    /// implementation of craut method in dal list configration for orderItem
    /// </summary>
    internal class OrderItem : IOrderItem
    {
        const string s_orderItems = "OrderItems";
        DataSurceInitialize.RunninId RunninId = new();

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int Add(DO.OrderItem item)
        {
            item.Id = RunninId.OrderItemId;
            List<DO.OrderItem?> orderItems = XMLTools.LoadListFromXMLSerializer<DO.OrderItem>(s_orderItems);

            if (orderItems.FirstOrDefault(x => x?.Id == item.Id) != null)
                throw new AlreadyExistsException();

            orderItems.Add(item);
            XMLTools.SaveListToXMLSerializer(orderItems, s_orderItems);

            return item.Id;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Delete(int orderItemId)
        {
            List<DO.OrderItem?> orderItems = XMLTools.LoadListFromXMLSerializer<DO.OrderItem>(s_orderItems);

            if (orderItems.RemoveAll(x => x?.Id == orderItemId) == 0)
                throw new NotFoundException();

            XMLTools.SaveListToXMLSerializer(orderItems, s_orderItems);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DO.OrderItem Get(Func<DO.OrderItem?, bool>? select_)
        {
            List<DO.OrderItem?> orderItems = XMLTools.LoadListFromXMLSerializer<DO.OrderItem>(s_orderItems);
            return orderItems.Find(x => select_!(x)) ?? throw new NullException();

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DO.OrderItem?> GetList(Func<DO.OrderItem?, bool>? select_ = null)
        {
            List<DO.OrderItem?> orderItems = XMLTools.LoadListFromXMLSerializer<DO.OrderItem>(s_orderItems);
            return select_ == null ? orderItems : orderItems.FindAll(x => select_(x));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Update(DO.OrderItem item)
        {
            Delete(item.Id);
            Add(item);
        }
    }
}
