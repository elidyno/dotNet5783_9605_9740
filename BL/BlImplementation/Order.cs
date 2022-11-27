using BlApi;
//using BO;
using System.Collections.Generic;

namespace BlImplementation;

internal class Order : IOrder
{
    private DalApi.IDal Dal = new Dal.DalList(); //Using it we can access the data access classes

    /// <summary>
    /// Returns an order (logical entity) by order id
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public BO.Order Get(int orderId)
    {
        if (orderId <= 0)
            throw new ArgumentOutOfRangeException("exception");

        //else  
        DO.Order dataOrder = new DO.Order();
        //Try requesting an order from a data layer
        try
        {
            dataOrder = Dal.Order.Get(orderId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        IEnumerable<DO.OrderItem> items = new List<DO.OrderItem>();
        //Try requesting a List of orderItems from a data layer
        try
        {
            items = Dal.OrderItem.GetItemsListByOrderId(orderId); 
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }


        //Creating a list of orderItems - logical entities
        List<BO.OrderItem> orderItems = new List<BO.OrderItem>();
        foreach (DO.OrderItem item in items)          
        {
            orderItems.Add(new BO.OrderItem
            {
                Id = item.Id,
                Amount = item.Amount,
                Price = item.Price,
                ProductId = item.ProductId,
                TotalPrice = item.Price * item.Amount,
                ProductName = Dal.Product.Get(item.ProductId).Name
            });
        }

        //Calculates the total order price
        double totalOrderPrice = 0;
        foreach (BO.OrderItem item in orderItems)
            totalOrderPrice += item.TotalPrice;

        //Create Order - logical entity
        BO.Order boOrder = new BO.Order()
        {
            Id = dataOrder.Id,
            CustomerName = dataOrder.CustomerName,
            CustomerEmail = dataOrder.CustomerEmail,
            CustomerAdress = dataOrder.CustomerAdress,
            OrderDate = dataOrder.OrderDate,
            ShipDate = dataOrder.ShipDate,
            DeliveryDate = dataOrder.DeliveryDate,
            Items = orderItems,
            TotalPrice = totalOrderPrice
        };

        //Calculates the status according to the order data in relation to the current time.
        if (boOrder.DeliveryDate < DateTime.Now)
            boOrder.status = BO.Status.DELIVERED;
        else if (boOrder.ShipDate < DateTime.Now)
            boOrder.status = BO.Status.SHIPPED;
        else if (boOrder.OrderDate < DateTime.Now)
            boOrder.status = BO.Status.APPROVED;

        return boOrder;
    }

    /// <summary>
    /// Returns a list of orders
    /// </summary>
    /// <returns></returns>
    public IEnumerable<BO.OrderForList> GetList()
    {

        IEnumerable<DO.Order> orders = new List<DO.Order>();
        //Try requesting order list from a data layer
        try
        {
            orders = Dal.Order.GetList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        //Creating a list of OrderForList -logical entities
        List<BO.OrderForList> orderForList = new List<BO.OrderForList>();
        
        //Populates the list by creating "OrderForList" type objects based on order data and OrderItem data and additional calculations.
        foreach (DO.Order order in orders)
        {
            //For each order, request the list of orderItems
            IEnumerable<DO.OrderItem> items = new List<DO.OrderItem>();
            items = Dal.OrderItem.GetItemsListByOrderId(order.Id);

            int amountOfItems = items.Count();
            
            //Calculates the Total Price
            double totalPrice = 0;
            foreach (DO.OrderItem item in items)
                totalPrice += item.Price * item.Amount;

            //Calculates the status according to the order data in relation to the current time.
            BO.Status status = BO.Status.APPROVED;     //?
            if (order.DeliveryDate < DateTime.Now)
                status = BO.Status.DELIVERED;
            else if (order.ShipDate < DateTime.Now)
                status = BO.Status.SHIPPED;
            else if (order.OrderDate < DateTime.Now)
                status = BO.Status.APPROVED;


            orderForList.Add(new BO.OrderForList()
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                AmountOfItems = amountOfItems,
                TotalPrice = totalPrice,
                status = status
            });

        }
        
        return orderForList;
    }

    public BO.OrderTracking GetTracking(int orderId)
    {
        DO.Order dataOrder = new DO.Order();
        try
        {
            dataOrder = Dal.Order.Get(orderId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        BO.OrderTracking orderTracking = new BO.OrderTracking() { ID = orderId };
       
        //Calculates the status according to the order data in relation to the current time.
        if (dataOrder.DeliveryDate < DateTime.Now)
            orderTracking.status = BO.Status.DELIVERED;
        else if (dataOrder.ShipDate < DateTime.Now)
            orderTracking.status = BO.Status.SHIPPED;
        else if (dataOrder.OrderDate < DateTime.Now)
            orderTracking.status = BO.Status.APPROVED;

        orderTracking.TrackingList.Add(new Tuple <DateTime, BO.Status>(dataOrder.OrderDate, BO.Status.APPROVED));
        orderTracking.TrackingList.Add(new Tuple<DateTime, BO.Status>(dataOrder.ShipDate, BO.Status.SHIPPED));
        orderTracking.TrackingList.Add(new Tuple<DateTime, BO.Status>(dataOrder.DeliveryDate, BO.Status.DELIVERED));

        return orderTracking;
    }

    public BO.Order UpdateOrderDelivery(int orderId)
    {
        DO.Order dataOrder = new DO.Order();
        try                                         // (כבר מתבצע ביצירת ישות לוגית הזמנה)
        {
            dataOrder = Dal.Order.Get(orderId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        BO.Order boOrder = Get(orderId); //Try?
       
        //If the order has been shipped (but not yet delivered) then update the delivery date
        if (boOrder.status == BO.Status.SHIPPED) 
        {   
            dataOrder.DeliveryDate = DateTime.Now;
            boOrder.DeliveryDate = DateTime.Now;
            boOrder.status = BO.Status.DELIVERED;
        }

        //Attempting to update the data layer
        try
        {
            Dal.Order.Update(dataOrder);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return boOrder;
    }

    public BO.Order UpdateOrderSheep(int orderId)
    {

        DO.Order dataOrder = new DO.Order();
        try                                         // (כבר מתבצע ביצירת ישות לוגית הזמנה)
        {
            dataOrder = Dal.Order.Get(orderId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        BO.Order boOrder = Get(orderId);
        //If the given status is that the order has been approved (but not yet shipped),
        //then update the delivery date to now.
        if (boOrder.status == BO.Status.APPROVED)
        {
            dataOrder.ShipDate = DateTime.Now;
            boOrder.ShipDate = DateTime.Now;
            boOrder.status = BO.Status.SHIPPED;
        }

        //Update the data layer
        Dal.Order.Update(dataOrder);

        return boOrder;
    }
}
