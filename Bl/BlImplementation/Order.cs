﻿using BlApi;
using BO;
using DO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BlImplementation;

/// <summary>
/// implementation of order operation
/// </summary>
internal class Order : IOrder
{
    DalApi.IDal? dal = DalApi.Factory.Get();  //Using it we can access the data access classes

    /// <summary>
    /// Returns an order (logical entity) by order i
    /// <param name="orderId"></param>
    /// <returns></returns>
    /// <exception cref="BO.InvalidValueException"></exception>
    /// <exception cref="BO.DataRequestFailedException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order Get(int orderId)
    {
        DO.Order dataOrder = new DO.Order();
        if (orderId <= 0)
        {
            throw new BO.InvalidValueException("Id must be greater than zero");
        }
        //Try requesting an order from data layer
        try
        {
            dataOrder = dal?.Order.Get(x => x?.Id == orderId) ?? throw new NullableException();
        }
        catch (Exception e)
        {
            throw new BO.DataRequestFailedException(e.Message);
        }

        IEnumerable<DO.OrderItem?> items = new List<DO.OrderItem?>();
        //Try requesting a List of orderItems from data layer
        try
        {
            items = dal.OrderItem.GetList( x => x?.OrderId == orderId);
        }
        catch (Exception e)
        {
            throw new BO.DataRequestFailedException(e.Message);
        }

        //Creating a list of orderItems - logical entities
        List<BO.OrderItem> orderItems = items
            .Select(item => new BO.OrderItem
            {
                Id = item?.Id ?? 0,
                Amount = item?.Amount ?? 0,
                Price = item?.Price ?? 0,
                ProductId = item?.ProductId ?? 0,
                TotalPrice = (item?.Price ?? 0) * (item?.Amount ?? 0),
                ProductName = dal.Product.Get(x => x?.Id == item?.ProductId).Name
            })
            .ToList();

        //Calculates the total order price
        double totalOrderPrice = orderItems.Sum(item => item?.TotalPrice ?? 0);
       

        //Calculates the status according to the order data in relation to the current time.
        BO.Status status_ = GetStatus(dataOrder);

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
            Items = new List<BO.OrderItem>(orderItems),
            TotalPrice = totalOrderPrice,
            status = status_
        };

        //boOrder.Items = new List<BO.OrderItem?>();
        //boOrder.Items = orderItems;
        //foreach (BO.OrderItem item in orderItems)
        //    boOrder.Items.Add(item);
        return boOrder;
    }

    /// <summary>
    /// Returns a list of orders
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<BO.OrderForList?> GetList()
    {
        IEnumerable<DO.Order?> orders = new List<DO.Order?>();
        //Try requesting order list from a data layer
        orders = dal?.Order.GetList() ?? throw new NullableException();

        //Creating a list of OrderForList -logical entities
        //List<BO.OrderForList?> ordersForList = new List<BO.OrderForList?>();

        //Populates the list by creating "OrderForList" type objects based on order data and OrderItem data and additional calculations.
        List<BO.OrderForList> ordersForList = orders
            .Select(order =>
            {
                //For each order, request the list of orderItems
                IEnumerable<DO.OrderItem?> items = dal.OrderItem.GetList(x => x?.OrderId == order?.Id);

                int amountOfItems = items.Count();

                //Calculates the Total Price
                double totalPrice = items.Sum(item => (item?.Price ?? 0) * (item?.Amount ?? 0));

                //Calculates the status according to the order data in relation to the current time.
                BO.Status status_ = GetStatus(order);

                return new BO.OrderForList()
                {
                    Id = order?.Id ?? 0,
                    CustomerName = order?.CustomerName,
                    AmountOfItems = amountOfItems,
                    TotalPrice = totalPrice,
                    status = status_
                };
            })
            .OrderBy(x => x.Id)
            .ToList();

        return ordersForList;
    }
    /// <summary>
    /// Returns a watchlist for a given order
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    /// <exception cref="BO.DataRequestFailedException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.OrderTracking GetTracking(int orderId)
    {
        DO.Order dataOrder = new DO.Order();
        try
        {
            dataOrder = dal?.Order.Get(x => x?.Id == orderId) ?? throw new NullableException();
        }
        catch (Exception e)
        {
            throw new BO.DataRequestFailedException(e.Message);
        }
        BO.Status status_ = GetStatus(dataOrder);
        BO.OrderTracking orderTracking = new BO.OrderTracking()
        {
            ID = orderId,
            status = status_
        };

        orderTracking.TrackingList = new List<(DateTime?, string?)>
        {
            (dataOrder.OrderDate, "The order created")
        };
        if (dataOrder.ShipDate != null)
            orderTracking.TrackingList.Add((dataOrder.ShipDate, "The order shipped"));
        if (dataOrder.DeliveryDate != null)
            orderTracking.TrackingList.Add((dataOrder.DeliveryDate, "The order delivered"));

        return orderTracking;
    }
    /// <summary>
    /// Updates the order delivery date
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    /// <exception cref="BO.DataRequestFailedException"></exception>
    /// <exception cref="BO.InvalidValueException"></exception>
    /// <exception cref="BO.UpdateFailedException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order UpdateOrderDelivery(int orderId)
    {
        BO.Order boOrder = new BO.Order();
        DO.Order dataOrder = new DO.Order();
        try
        {
            dataOrder = dal?.Order.Get(x => x?.Id == orderId) ?? throw new NullableException();
            boOrder = Get(orderId);
        }
        catch (DO.NotFoundException e)
        {
            throw new BO.DataRequestFailedException(e.Message);
        }
        catch (BO.InvalidValueException e)
        {
            throw new BO.InvalidValueException(e.Message);
        }

        //If the order has been shipped (but not yet delivered) then update the delivery date
        if (boOrder.status == BO.Status.SHIPPED)
        {
            dataOrder.DeliveryDate = DateTime.Now;
            boOrder.DeliveryDate = DateTime.Now;
            boOrder.status = BO.Status.DELIVERED;
        }
        else
            throw new BO.UpdateFailedException("The order has not yet been sent or has already been delivered");

        //Attempting to update the data layer
        try
        {
            dal.Order.Update(dataOrder);
        }
        catch (DO.NotFoundException e)
        {
            throw new BO.DataRequestFailedException(e.Message);
        }

        return boOrder;
    }
    /// <summary>
    /// Updates the order shipping date
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    /// <exception cref="BO.DataRequestFailedException"></exception>
    /// <exception cref="BO.InvalidValueException"></exception>
    /// <exception cref="BO.UpdateFailedException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order UpdateOrderSheep(int orderId)
    {
        BO.Order boOrder = new BO.Order();
        DO.Order dataOrder = new DO.Order();
        try
        {
            dataOrder = dal?.Order.Get(x => x?.Id == orderId) ?? throw new NullableException(); 
            boOrder = Get(orderId);
        }
        catch (DO.NotFoundException e)
        {
            throw new BO.DataRequestFailedException(e.Message);
        }
        catch (BO.InvalidValueException e)
        {
            throw new BO.InvalidValueException(e.Message);
        }


        //If the given status is that the order has been approved (but not yet shipped),
        //then update the delivery date to now.
        if (boOrder.status == BO.Status.APPROVED)
        {
            dataOrder.ShipDate = DateTime.Now;
            boOrder.ShipDate = DateTime.Now;
            boOrder.status = BO.Status.SHIPPED;
        }
        else
            throw new BO.UpdateFailedException("The order  has already been shipped");

        //Attempting to update the data layer
        try
        {
            dal.Order.Update(dataOrder);
        }
        catch (DO.NotFoundException e)
        {
            throw new BO.DataRequestFailedException(e.Message);
        }

        return boOrder;
    }
    /// <summary>
    /// Calculates the status of the order
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Status GetStatus(DO.Order? order)
    {
        BO.Status status;
        if (order?.DeliveryDate != null)
            status = BO.Status.DELIVERED;
        else
        {
            if (order?.ShipDate != null)
                status = BO.Status.SHIPPED;
            else status = BO.Status.APPROVED;
        }
        return status;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public int? OldestOrder()
    {
        List<DO.Order?> orders = dal!.Order.GetList(order => order?.DeliveryDate == null).ToList();
        //DateTime? earliest = orders[0]?.ShipDate ?? orders[0]?.OrderDate;
        //int? oldest = orders[0]?.Id;
        //foreach (DO.Order? order in orders) 
        //{ 
        //    if(order?.ShipDate != null)
        //    {
        //        if(order?.ShipDate < earliest)
        //        {
        //            earliest = order?.ShipDate;
        //            oldest = order?.Id;
        //        }
        //    }              
        //    else
        //    {
        //        if (order?.OrderDate < earliest)
        //        {
        //            earliest = order?.OrderDate;
        //            oldest = order?.Id;
        //        }
        //    }

        //}

        var oldestOrder = orders
            .Where(o => o != null)
            .OrderBy(o => o?.ShipDate ?? o?.OrderDate) //OrderByDescending?
            .FirstOrDefault();
        int? oldestId = oldestOrder?.Id;

        return oldestId;
    }
}