﻿using DalApi;
namespace Dal;

using DO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

//A class that links between the order class (DO file) and the Data class (which is linked to collections in Data) through methods
internal class DalOrder : IOrder
{
    /// <summary>
    /// due to deficult to initilize the data surce
    /// and i not wanted to cencel the Config class
    /// as Dan Zilbershtain suggested
    //i found a solution: asaiment an tem int with the lenth of the arries 
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)]  // needless?
    public DalOrder()
    {
        //only for initilize the DataSurce
        int initilizeDataSurce = DataSource._orderList.Count;
    }

    /// <summary>
    /// Receives a new order and returns the order number
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(Order o)
    {
        o.Id = DataSource.Config.OrderRunningId;
        DataSource._orderList.Add(o);
        return o.Id;
    }
    ///// <summary>
    ///// Receives an order number and returns all order details
    ///// </summary>
    ///// <param name="OrderId"></param>
    ///// <returns></returns>
    ///// <exception cref="Exception"></exception>
    //public Order Get(int OrderId)   //?
    //{
    //    return DataSource._orderList.Find(x => x?.Id == OrderId) ?? 
    //           throw new NotFoundException("Order Id not exist");
    //}


    /// <summary>
    /// Returns the list of all orders or requested orders according to a condition
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Order?> GetList(Func<Order?, bool>? select_ = null) //?
    {
        List<Order?> orders = new List<Order?>();
        if (select_ == null)
            orders = DataSource._orderList.ToList<Order?>();
        else
            orders = DataSource._orderList.FindAll(x => select_(x));
        return orders;    
    }
    /// <summary>
    /// Receives an order number and cancels it
    /// </summary>
    /// <param name="orderId"></param>
    /// <exception cref="Exception"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int orderId)
    {
        int delIndex = DataSource._orderList.FindIndex(x => x?.Id == orderId);
        if (delIndex == -1)
            throw new NotFoundException("Order Id not exist");
        else
        DataSource._orderList.RemoveAt(delIndex);
    }
    /// <summary>
    /// Receives updated order details and updates them
    /// </summary>
    /// <param name="p"></param>
    /// <exception cref="Exception"></exception>
    
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Order p)
    {
        int updateIndex = DataSource._orderList.FindIndex(x => x?.Id == p.Id);
        if (updateIndex != -1)
            DataSource._orderList[updateIndex] = p;
        else
            throw new NotFoundException("Order Id not exist");
    }
    /// <summary>
    /// Returns a requested order according to the conditions
    /// </summary>
    /// <param name="select_"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Order Get(Func<Order?, bool>? select_)
    {
        return DataSource._orderList.Find(x => select_(x)) ??
            throw new NotFoundException("The requested order does not exist");
    }
}