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
        DataSource._orderList.Add(new Order());
        DataSource._orderList[DataSource._orderList.Count - 1] = o;
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
        bool found = false;
        int i;
        for (i = 0; i < DataSource._orderList.Count; i++)
        {
            if (OrderId == DataSource._orderList[i].Id)
                break;
        }
        if (i == DataSource._orderList.Count && !found)
            throw new Exception("the Order id not exist in list");
        return DataSource._orderList[i];
    }
    /// <summary>
    /// Returns all orders
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Order> GetList()
    {

        List<Order> orders = new List<Order>();
        orders = DataSource._orderList.ToList<Order>();

        //Order[] Orders = new Order[DataSource._orderList.Count];
        //for (int i = 0; i < DataSource._orderList.Count; i++)
        //{
        //    Orders[i] = DataSource._orderList[i];
        //}

        return orders;
    }
    /// <summary>
    /// Receives an order number and cancels it
    /// </summary>
    /// <param name="orderId"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(int orderId)
    {
        int delIndex = int.MinValue;
        for (int i = 0; i < DataSource._orderList.Count; i++)
        {
            if (orderId == DataSource._orderList[i].Id)
            {
                delIndex = i;
            }
        }
        if (delIndex == int.MinValue)
            throw new Exception("The Order not exist in list");

        DataSource._orderList.RemoveAt(delIndex);
    }
    /// <summary>
    /// Receives updated order details and updates them
    /// </summary>
    /// <param name="p"></param>
    /// <exception cref="Exception"></exception>
    public void Update(Order p)
    {
        for (int i = 0; i < DataSource._orderList.Count; i++)
        {
            if (p.Id == DataSource._orderList[i].Id)
            {
                DataSource._orderList[i] = p;
                return;
            }
        }

        throw new Exception("The Order not exist in list");
    }

}