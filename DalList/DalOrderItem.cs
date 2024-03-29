﻿using DalApi;
using DO;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace Dal;
//A class that links between the order-item class (DO file) and the Data class (which is linked to collections in Data) through methods
internal class DalOrderItem : IOrderItem
{
    /// <summary>
    /// due to deficult to initilize the data surce
    /// and i not wanted to cencel the Config class
    /// as Dan Zilbershtain suggested
    ///i found a solution: asaiment an tem int with the lenth of the arries 
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public DalOrderItem()
    {
        //only for initilize the DataSurce
        int initilizeDataSurce = DataSource._orderItemList.Count;
    }
    /// <summary>
    /// Receives a new order item and returns its ID number
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(OrderItem item)
    {
        item.Id = DataSource.Config.OrderItemRunningId;
        DataSource._orderItemList.Add(item);
        return item.Id;
    }
    ///// <summary>
    ///// Receives an ID number of an item in the order and returns the item
    ///// </summary>
    ///// <param name="OrderItemId"></param>
    ///// <returns></returns>
    ///// <exception cref="Exception"></exception>
    //public OrderItem Get(int OrderItemId)
    //{
    //    return DataSource._orderItemList.Find(x => x?.Id == OrderItemId) ??
    //        throw new NotFoundException("OrderItem Id not exist");

    //}
    /// <summary>
    /// Returns all items in the order
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<OrderItem?> GetList()
    {
        List<OrderItem?> orderItems = new List<OrderItem?>();
        orderItems = DataSource._orderItemList.ToList<OrderItem?>();
        return orderItems;
    }
    /// <summary>
    /// Receives an ID number of the item in the order and cancels it
    /// </summary>
    /// <param name="orderId"></param>
    /// <exception cref="Exception"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int orderItemId)
    {
        int delIndex = DataSource._orderItemList.FindIndex(x => x?.Id == orderItemId);
        if (delIndex == -1)
            throw new NotFoundException("OrderItem Id not exist");
        else
            DataSource._orderItemList.RemoveAt(delIndex);
    }
    /// <summary>
    /// Receives an item in the order with updated details and updates it
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>  
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(OrderItem item)
    {
        int updateIndex = DataSource._orderItemList.FindIndex(x => x?.Id == item.Id);
        if (updateIndex != -1)
            DataSource._orderItemList[updateIndex] = item;
        else
            throw new NotFoundException("OrderItem Id not exist");
    }
   
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<OrderItem?> GetList(Func<OrderItem?, bool>? select_ = null)
    {
        List<OrderItem?> orderItems = new List<OrderItem?>();
        if (select_ == null)
            orderItems = DataSource._orderItemList.ToList<OrderItem?>();
        else
            orderItems = DataSource._orderItemList.FindAll(x => select_(x));

        return orderItems;
    }
    
    [MethodImpl(MethodImplOptions.Synchronized)]
    public OrderItem Get(Func<OrderItem?, bool>? select_)
    {
        return DataSource._orderItemList.Find(x => select_!(x)) ??
             throw new NullException();
    }
}