namespace Dal;

using DO;

//A class that links between the order class (DO file) and the Data class (which is linked to collections in Data) through methods
public class DalOrder
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
        int initilizeDataSurce = DataSource._orderList.Length;
    }

    /// <summary>
    /// Receives a new order and returns the order number
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int AddOrder(Order o)
    {
        if (DataSource._orderList.Length == DataSource.Config._orderIndexer)
            throw new Exception("no place in list to add");
        o.Id = DataSource.Config.OrderRunningId;
        DataSource._orderList[DataSource.Config._orderIndexer++] = new Order();
        DataSource._orderList[DataSource.Config._orderIndexer] = o;
        return o.Id;
    }
    /// <summary>
    /// Receives an order number and returns all order details
    /// </summary>
    /// <param name="OrderId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Order GetOrder(int OrderId)
    {
        bool found = false;
        int i;
        for (i = 0; i < DataSource.Config._orderIndexer; i++)
        {
            if (OrderId == DataSource._orderList[i].Id)
                break;
        }
        if (i == DataSource.Config._orderIndexer && !found)
            throw new Exception("the Order id not exist in list");
        return DataSource._orderList[i];
    }
    /// <summary>
    /// Returns all orders
    /// </summary>
    /// <returns></returns>
    public Order[] GetOrdersList()
    {
        Order[] Orders = new Order[DataSource.Config._orderIndexer];
        for (int i = 0; i < DataSource.Config._orderIndexer; i++)
        {
            Orders[i] = DataSource._orderList[i];
        }

        return Orders;
    }
    /// <summary>
    /// Receives an order number and cancels it
    /// </summary>
    /// <param name="orderId"></param>
    /// <exception cref="Exception"></exception>
    public void DelateOrder(int orderId)
    {
        int delIndex = int.MinValue;
        for (int i = 0; i < DataSource.Config._orderIndexer; i++)
        {
            if (orderId == DataSource._orderList[i].Id)
            {
                delIndex = i;
            }
        }
        if (delIndex == int.MinValue)
            throw new Exception("The Order not exist in list");

        //move back all items that was after the deleted item
        --DataSource.Config._orderIndexer;
        for (int i = delIndex; i < DataSource.Config._orderIndexer; i++)
        {
            DataSource._orderList[i] = DataSource._orderList[i + 1];
        }
    }
    /// <summary>
    /// Receives updated order details and updates them
    /// </summary>
    /// <param name="p"></param>
    /// <exception cref="Exception"></exception>
    public void UpdateOrder(Order p)
    {
        for (int i = 0; i < DataSource.Config._orderIndexer; i++)
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