using DalApi;
namespace Dal;

using DO;
using System.Collections.Generic;

//A class that links between the order class (DO file) and the Data class (which is linked to collections in Data) through methods
internal class DalOrder : IOrder
{
    /// <summary>
    /// due to deficult to initilize the data surce
    /// and i not wanted to cencel the Config class
    /// as Dan Zilbershtain suggested
    //i found a solution: asaiment an tem int with the lenth of the arries 
    /// </summary>
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
    public int Add(Order o)
    {
        //if (DataSource._orderList.Count == DataSource.Config._orderIndexer)     Unnecessary ??
        //    throw new Exception("no place in list to add");
        o.Id = DataSource.Config.OrderRunningId;
        DataSource._orderList.Add(o);
        return o.Id;
    }
    /// <summary>
    /// Receives an order number and returns all order details
    /// </summary>
    /// <param name="OrderId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Order Get(int OrderId)
    {
        if (DataSource._orderList.Exists(x => x.Id == OrderId))    //אולי אפשר לקצר
            return DataSource._orderList.Find(x => x.Id == OrderId);
        else
            throw new NotFoundException();  
    }
    /// <summary>
    /// Returns all orders
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Order> GetList()
    {
        List<Order> orders = new List<Order>();
        orders = DataSource._orderList.ToList<Order>();
        return orders;  
    }
    /// <summary>
    /// Receives an order number and cancels it
    /// </summary>
    /// <param name="orderId"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(int orderId)
    {
        int delIndex = DataSource._orderList.FindIndex(x => x.Id == orderId);
        if (delIndex == -1)
            throw new NotFoundException();
        else
        DataSource._orderList.RemoveAt(delIndex);
    }
    /// <summary>
    /// Receives updated order details and updates them
    /// </summary>
    /// <param name="p"></param>
    /// <exception cref="Exception"></exception>
    public void Update(Order p)
    {
        int updateIndex = DataSource._orderList.FindIndex(x => x.Id == p.Id);
        if (updateIndex != -1)
            DataSource._orderList[updateIndex] = p;
        else
            throw new NotFoundException();
    }

}