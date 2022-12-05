﻿using BlApi;
using System.Collections.Generic;

namespace BlImplementation;

/// <summary>
/// implementation of order operation
/// </summary>
internal class Order : IOrder
{
    private DalApi.IDal Dal = new Dal.DalList(); //Using it we can access the data access classes

    /// <summary>
    /// Returns an order (logical entity) by order i
    /// <param name="orderId"></param>
    /// <returns></returns>
    /// <exception cref="BO.InvalidValueException"></exception>
    /// <exception cref="BO.DataRequestFailedException"></exception>
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
            dataOrder = Dal.Order.Get(orderId);
        }
        catch (Exception e)
        {
            throw new BO.DataRequestFailedException(e.Message);
        }

        IEnumerable<DO.OrderItem> items = new List<DO.OrderItem>();
        //Try requesting a List of orderItems from a data layer
        try
        {
            items = Dal.OrderItem.GetItemsListByOrderId(orderId);
        }
        catch (Exception e)
        {
            throw new BO.DataRequestFailedException(e.Message);
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
            //Items = new List<BO.OrderItem>(orderItems);
            TotalPrice = totalOrderPrice,
            status = status_
        };

        boOrder.Items = new List<BO.OrderItem>();
        boOrder.Items = orderItems;
        //foreach (BO.OrderItem item in orderItems)
        //    boOrder.Items.Add(item);
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
        orders = Dal.Order.GetList();

        //Creating a list of OrderForList -logical entities
        List<BO.OrderForList> ordersForList = new List<BO.OrderForList>();

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
            BO.Status status_ = GetStatus(order);

            ordersForList.Add(new BO.OrderForList()
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                AmountOfItems = amountOfItems,
                TotalPrice = totalPrice,
                status = status_
            });

        }

        return ordersForList;
    }
    /// <summary>
    /// Returns a watchlist for a given order
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    /// <exception cref="BO.DataRequestFailedException"></exception>
    public BO.OrderTracking GetTracking(int orderId)
    {
        DO.Order dataOrder = new DO.Order();
        try
        {
            dataOrder = Dal.Order.Get(orderId);
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

        orderTracking.TrackingList = new List<(DateTime?, string)>();
        orderTracking.TrackingList.Add((dataOrder.OrderDate, "The order created"));
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
    public BO.Order UpdateOrderDelivery(int orderId)
    {
        BO.Order boOrder = new BO.Order();
        DO.Order dataOrder = new DO.Order();
        try
        {
            dataOrder = Dal.Order.Get(orderId);
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
            Dal.Order.Update(dataOrder);
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
    public BO.Order UpdateOrderSheep(int orderId)
    {
        BO.Order boOrder = new BO.Order();
        DO.Order dataOrder = new DO.Order();
        try
        {
            dataOrder = Dal.Order.Get(orderId);
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
            Dal.Order.Update(dataOrder);
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
    public BO.Status GetStatus(DO.Order order)
    {
        BO.Status status;
        if (order.DeliveryDate != null)
            status = BO.Status.DELIVERED;
        else
        {
            if (order.ShipDate != null)
                status = BO.Status.SHIPPED;
            else status = BO.Status.APPROVED;
        }
        return status;
    }
}