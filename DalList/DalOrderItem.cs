using DalApi;
using DO;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace Dal;
//A class that links between the order-item class (DO file) and the Data class (which is linked to collections in Data) through methods
internal class DalOrderItem :IOrderItem
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
        int initilizeDataSurce = DataSource._orderItemList.Count;
    }
    /// <summary>
    /// Receives a new order item and returns its ID number
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int Add(OrderItem item)
    {
        item.Id = DataSource.Config.OrderItemRunningId;
        DataSource._orderItemList.Add(item);
        return item.Id;
    }
    /// <summary>
    /// Receives an ID number of an item in the order and returns the item
    /// </summary>
    /// <param name="OrderItemId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public OrderItem Get(int OrderItemId)
    {
        return DataSource._orderItemList.Find(x => x?.Id == OrderItemId) ??
            throw new NotFoundException("OrderItem Id not exist");
        
    }
    /// <summary>
    /// Returns all items in the order
    /// </summary>
    /// <returns></returns>
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
    public void Update(OrderItem item)
    {
        int updateIndex = DataSource._orderItemList.FindIndex(x => x?.Id == item.Id);
        if (updateIndex != -1)
            DataSource._orderItemList[updateIndex] = item;
        else
            throw new NotFoundException("OrderItem Id not exist");
    }

    public IEnumerable<OrderItem?> GetList(Func<OrderItem?, bool>? select_ = null)
    {
        List<OrderItem?> orderItems = new List<OrderItem?>();
        if (select_ == null)
            orderItems = DataSource._orderItemList.ToList<OrderItem?>();
        else
            orderItems = DataSource._orderItemList.FindAll(x => select_(x));

        return orderItems;
    }

    public OrderItem Get(Func<OrderItem?, bool>? select_)
    {
        return DataSource._orderItemList.Find(x => select_(x)) ??
             throw new NullException();
    }


    /// <summary>
    /// Receives a number of the order and a number of the product and returns the item in the appropriate orde
    /// </summary>
    /// <param name="OrderId"></param>
    /// <param name="ProductId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public OrderItem GetItemByOrderAndProduct(int orderId, int productId)
    {
       return DataSource._orderItemList.Find(x => x?.ProductId == productId && x?.OrderId == orderId) ??
            throw new NotFoundException("OrderItem Id not exist");
    }
    /// <summary>
    /// Receives an order number and returns all the items in the order
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    public IEnumerable<OrderItem?> GetItemsListByOrderId(int orderId)
    {
        List<OrderItem?> orderItems = new List<OrderItem?>();
        orderItems = DataSource._orderItemList.FindAll(x => x?.OrderId == orderId);
        return orderItems;
    }
}