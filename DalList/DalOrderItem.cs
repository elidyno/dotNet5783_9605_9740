using DO;

namespace Dal;
//A class that links between the order-item class (DO file) and the Data class (which is linked to collections in Data) through methods
public class DalOrderItem
{
    /// <summary>
    /// due to deficult to initilize the data surce
    /// and i not wanted to cencel the Config class
    /// as Dan Zilbershtain suggested
    ///i found a solution: asaiment an tem int with the lenth of the arries 
    /// </summary>
    public DalOrderItem()
    {
        //only for initilize the DataSurce
        int initilizeDataSurce = DataSource._orderItemList.Length;
    }
    /// <summary>
    /// Receives a new order item and returns its ID number
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int AddOrderItem(OrderItem item)
    {
        if (DataSource._orderItemList.Length == DataSource.Config._orderItemIndexer)
            throw new Exception("no place in list to add");
        item.Id = DataSource.Config.OrderItemRunningId;
        DataSource._orderItemList[DataSource.Config._orderItemIndexer++] = item;
        return item.Id;
    }
    /// <summary>
    /// Receives an ID number of an item in the order and returns the item
    /// </summary>
    /// <param name="OrderItemId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public OrderItem GetOrderItem(int OrderItemId)
    {
        int i;
        for (i = 0; i < DataSource.Config._orderItemIndexer; i++)
        {
            if (OrderItemId == DataSource._orderItemList[i].Id)
                break;
        }
        if (i == DataSource.Config._orderItemIndexer)
            throw new Exception("the OrderItem id not exist in list");
        OrderItem item = DataSource._orderItemList[i];

        return item;
    }
    /// <summary>
    /// Returns all items in the order
    /// </summary>
    /// <returns></returns>
    public OrderItem[] GetOrderItemsList()
    {
        OrderItem[] OrderItems = new OrderItem[DataSource.Config._orderItemIndexer];
        for (int i = 0; i < DataSource.Config._orderItemIndexer; i++)
        {
            OrderItems[i] = DataSource._orderItemList[i];
        }

        return OrderItems;
    }
    /// <summary>
    /// Receives an ID number of the item in the order and cancels it
    /// </summary>
    /// <param name="orderId"></param>
    /// <exception cref="Exception"></exception>
    public void DelateOrderItem(int orderId)
    {
        int delIndex = int.MinValue;
        for (int i = 0; i < DataSource.Config._orderItemIndexer; i++)
        {
            if (orderId == DataSource._orderItemList[i].Id)
            {
                delIndex = i;
            }
        }
        if (delIndex == int.MinValue)
            throw new Exception("The OrderItem not exist in list");

        //move back all items that was after the deleted item
        --DataSource.Config._orderItemIndexer;
        for (int i = delIndex; i < DataSource.Config._orderItemIndexer; i++)
        {
            DataSource._orderItemList[i] = DataSource._orderItemList[i + 1];
        }
    }
    /// <summary>
    /// Receives an item in the order with updated details and updates it
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public void UpdateOrderItem(OrderItem item)
    {
        int i;
        for (i = 0; i < DataSource.Config._orderItemIndexer; i++)
        {
            if (item.Id == DataSource._orderItemList[i].Id)
            {
                DataSource._orderItemList[i] = item;
                return;
            }
        }

        throw new Exception("The OrderItem not exist in list");
    }

    /// <summary>
    /// Receives a number of the order and a number of the product and returns the item in the appropriate orde
    /// </summary>
    /// <param name="OrderId"></param>
    /// <param name="ProductId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public OrderItem GetItemByOrderAndProduct(int OrderId, int ProductId)
    {
        int i;
        for (i = 0; i < DataSource.Config._orderItemIndexer; i++)
        {
            if (OrderId == DataSource._orderItemList[i].OrderId && ProductId == DataSource._orderItemList[i].ProductId)
                break;
        }
        if (i == DataSource.Config._orderItemIndexer)
            throw new Exception("the OrderItem id not exist in list");

        OrderItem item = DataSource._orderItemList[i];
        return item;
    }
    /// <summary>
    /// Receives an order number and returns all the items in the order
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    public OrderItem[] GetItemsListByOrderId(int orderId)
    {
        OrderItem[] orderItems = null;

        for (int i = 0; i < DataSource.Config._orderItemIndexer; i++)
        {
            if (DataSource._orderItemList[i].OrderId == orderId)
            {
                orderItems[i] = DataSource._orderItemList[i];
            }
        }
        return orderItems;
    }

}