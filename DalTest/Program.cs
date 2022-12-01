using DalApi;
using DO;
using System.ComponentModel.DataAnnotations;

namespace Dal;
#region Emums
enum MainMenuCode { Exit, Product, Order, OrderItem };
enum SubMenu_CRAUD { ExitSubMenu = 0, Add, View, ViewAll, Update, Delete, };
enum SubMenu_ItemOrder { Add = 1, View, ViewAll, Update, Delete, itemByOrderAndProduct, itemListByOrder };
#endregion     
class Program
{
    #region Variable enum
    private static MainMenuCode menuCode;// enum for main menu
    private static SubMenu_CRAUD subMenu_CRAUD;// enum for sub menue: craud operation
    private static SubMenu_ItemOrder subMenu_ItemOrder; //enum for ItemOrder sub menue: includ also privet method of itemOrder
    private static bool exitSubMenu = false; // indicate if exit the sub menue
    private static int exit_ = int.MinValue;
    #endregion

    private static IDal dal = new DalList();
    static void Main(string[] args)
    {
        Console.WriteLine(@"
        
    Welcome to the 'Jerusalem Shoes' shoe Store");

        do
        {
            Console.WriteLine(@"
    Please select one of the following options to test:
        1) Product operation
        2) Order operation
        3) Order Item operation
        0) To exit from menu");

            MainMenuCode.TryParse(Console.ReadLine(), out menuCode);
            #region main menu
            try
            {
                switch (menuCode)
                {
                    case MainMenuCode.Product:
                        ProductMenue();
                        break;
                    case MainMenuCode.Order:
                        OrderMenue();
                        break;
                    case MainMenuCode.OrderItem:
                        OrderItemMenue();
                        break;
                    case MainMenuCode.Exit:
                        return;
                    default:
                        throw new Exception("Unvalide choice");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\tERROR: " + ex.Message);
                Console.WriteLine(@"
    press any key to continu...");
                Console.ReadKey();
                Console.Clear();
            }
        } while (menuCode != MainMenuCode.Exit);
        #endregion
    }
    #region Product Menu
    // <Gives a submenu for the product entity>
    static void ProductMenue()
    {
        do
        {
            Console.WriteLine(@"
    Select operation to test:
        1) To add a product
        2) To show a product
        3) To Show all product list
        4) To updat a product
        5) To delete a product");

            SubMenu_CRAUD.TryParse(Console.ReadLine(), out subMenu_CRAUD);
            switch (subMenu_CRAUD)
            {
                case SubMenu_CRAUD.Add:
                    Console.WriteLine(@"
        Enter the product ID number that you want to add");
                    int id;
                    int.TryParse(Console.ReadLine(), out id);
                    Console.WriteLine(@"
        Enter a product name that you want to add");
                    string? name;
                    name = Console.ReadLine();
                    Console.WriteLine(@"
        Enter a product category that you want to add");
                    Category catgory;
                    Category.TryParse(Console.ReadLine(), out catgory);

                    Console.WriteLine(@"
        Enter a product price that you want to add");
                    double tmpPrice;
                    double.TryParse(Console.ReadLine(), out tmpPrice);
                    int tmpAmount;
                    Console.WriteLine(@"
        Enter a product Amount in stok that you want to add");
                    int.TryParse(Console.ReadLine(), out tmpAmount);

                    Product addedProduct = new Product()
                    {
                        Id = id,
                        Name = name,
                        Category = catgory,
                        Price = tmpPrice,
                        InStock = tmpAmount
                    };
                    //<Receives a new product and returns his ID>
                    int addedProductId = dal.Product.Add(addedProduct);
                    Console.WriteLine("\tThe Product id: " + addedProductId);
                    break;
                case SubMenu_CRAUD.View:
                    Console.WriteLine(@"
        Enter the product ID number that you want to see his details");
                    int.TryParse(Console.ReadLine(), out id);
                    Product productToShow = new Product();
                    //<Receives a product ID and returns all product details>
                    productToShow = dal.Product.Get(id);
                    Console.WriteLine("\n\t" + productToShow);
                    break;
                case SubMenu_CRAUD.ViewAll:
                    //<Returns all products in the store>
                    IEnumerable<Product> productsList = dal.Product.GetList();
                    foreach (Product item in productsList)
                    {
                        Console.WriteLine(item);
                    }
                    break;
                case SubMenu_CRAUD.Update:
                    Console.WriteLine(@"
        Enter the product ID number that you want to update his details");
                    int.TryParse(Console.ReadLine(), out id);
                    Product oldProduct = dal.Product.Get(id);
                    Console.WriteLine(oldProduct);

                    name = null; //to check after if the user put a value for update
                    Console.WriteLine(@"
        Enter the new name");
                    name = Console.ReadLine();

                    Console.WriteLine(@"
        Enter the new category");
                    int tmpCategory = int.MinValue; //to check after if the user put a value for update
                    int.TryParse(Console.ReadLine(), out tmpCategory);

                    Console.WriteLine(@"
        Enter The New new price");
                    double tmpPrice2 = double.MinValue; //to check after if the user put a value for update
                    double.TryParse(Console.ReadLine(), out tmpPrice2);

                    int tmpAmount2 = int.MinValue; //to check after if the user put a value for update
                    Console.WriteLine(@"
        Enter the new amount in stok");
                    int.TryParse(Console.ReadLine(), out tmpAmount2);

                    //if user not update keep the old value
                    if (name == null)
                        name = oldProduct.Name;
                    if (tmpCategory == null)
                        tmpCategory = (int)oldProduct.Category;
                    if (tmpPrice2 == double.MinValue)
                        tmpPrice2 = oldProduct.Price;
                    if (tmpAmount2 == int.MinValue)
                        tmpPrice2 = oldProduct.InStock;
                    Product updateProduct = new Product()
                    {
                        Id = id,
                        Name = name, // if name is null kipe the old value
                        Category = (Category)tmpCategory,
                        Price = tmpPrice2,
                        InStock = tmpAmount2
                    };
                    //<Receives preferred product details and updates it>
                    dal.Product.Update(updateProduct);
                    break;
                case SubMenu_CRAUD.Delete:
                    Console.WriteLine(@"
        Enter the product ID number that you want to remove");
                    int.TryParse(Console.ReadLine(), out id);
                    //<Receives a product tag and deletes it from the menu>
                    dal.Product.Delete(id);
                    break;
                default:
                    break;
            }

            exitSubMenu = false; // innitilize the  continue flag
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine(@"
        press 0 to return to Main menu
        presss any other key to continue");

            int.TryParse(Console.ReadLine(), out exit_);
            if (exit_ == (int)SubMenu_CRAUD.ExitSubMenu)
                exitSubMenu = true;

        } while (!exitSubMenu);
    }
    #endregion

    #region Order Menu
    /// <summary>
    ///  Gives a submenu for the order entity
    /// </summary>
    static void OrderMenue()
    {
        do
        {
            Console.WriteLine(@"
    Select operation to test:
        1) To add an order
        2) To show an order
        3) To Show all order list
        4) To updat an order
        5) To delete an order");

            int id = int.MinValue;
            string? customeName = null;
            string? email = null;
            string? adress = null;
            DateTime orderCreate = DateTime.MinValue;
            DateTime orderShip = DateTime.MinValue;
            DateTime delivery = DateTime.MinValue;

            SubMenu_CRAUD.TryParse(Console.ReadLine(), out subMenu_CRAUD);
            switch (subMenu_CRAUD)
            {
                case SubMenu_CRAUD.Add:

                    Console.WriteLine(@"
        Enter a Customer name that you want to add");
                    customeName = Console.ReadLine();
                    Console.WriteLine(@"
        Enter a Customer email that you want to add");
                    email = Console.ReadLine();
                    Console.WriteLine(@"
        Enter a Customer adress that you want to add");
                    adress = Console.ReadLine();
                    Console.WriteLine(@"
        Enter a created Order  time");
                    DateTime.TryParse(Console.ReadLine(), out orderCreate);
                    Console.WriteLine(@"
        Enter a Ship Order  time");
                    DateTime.TryParse(Console.ReadLine(), out orderShip);
                    Console.WriteLine(@"
        Enter a delivery Order time");
                    DateTime.TryParse(Console.ReadLine(), out delivery);
                    Order addedOrder = new Order()
                    {
                        CustomerAdress = adress,
                        CustomerEmail = email,
                        CustomerName = customeName,
                        OrderDate = orderCreate,
                        ShipDate = orderShip,
                        DeliveryDate = delivery
                    };
                    //<Receives a new product order and returns the order number>
                    int addedOrderId = dal.Order.Add(addedOrder);
                    Console.WriteLine("\tThe Order id: " + addedOrderId);
                    break;
                case SubMenu_CRAUD.View:
                    Console.WriteLine(@"
        Enter the Order ID number that you want to see his details");
                    int.TryParse(Console.ReadLine(), out id);
                    Order orderToShow = new Order();
                    //<Gets the order number and returns the order details>
                    orderToShow = dal.Order.Get(id);
                    Console.WriteLine("\n\t" + orderToShow);
                    break;
                case SubMenu_CRAUD.ViewAll:
                    //<Returns all order details>
                    IEnumerable<Order> OrdersList = dal.Order.GetList();
                    foreach (Order Order in OrdersList)
                        Console.WriteLine("\t" + Order);
                    break;
                case SubMenu_CRAUD.Update:

                    Console.WriteLine(@"Enter the Order ID number that you want to update his details");
                    int.TryParse(Console.ReadLine(), out id);
                    Order oldOrder = dal.Order.Get(id);
                    Console.WriteLine(oldOrder);

                    Console.WriteLine(@"
        To update a faild enter the new value when it requaide,
        For keep the old value press on the 'enter' key");

                    //ask the new detaile to update
                    Console.WriteLine(@"
        Enter the new customer name");
                    customeName = Console.ReadLine();
                    if (customeName == "\n" || customeName == null)
                        customeName = oldOrder.CustomerName;

                    Console.WriteLine(@"
        Enter the new customer email");
                    email = Console.ReadLine();
                    if (email == "\n" || email == null)
                        email = oldOrder.CustomerEmail;

                    Console.WriteLine(@"
        Enter the new customer adress");
                    adress = Console.ReadLine();
                    if (adress == "\n" || adress == null)
                        adress = oldOrder.CustomerAdress;

                    Console.WriteLine(@"
        Enter the new create date");
                    DateTime.TryParse(Console.ReadLine(), out orderCreate);
                    if (orderCreate == DateTime.MinValue)
                        orderCreate = oldOrder.OrderDate;

                    Console.WriteLine(@"
        Enter the new ship date");
                    DateTime.TryParse(Console.ReadLine(), out orderShip);
                    if (orderShip == DateTime.MinValue)
                        orderShip = oldOrder.OrderDate;

                    Console.WriteLine(@"Enter the new delivery date");
                    DateTime.TryParse(Console.ReadLine(), out delivery);
                    if (delivery == DateTime.MinValue)
                        delivery = oldOrder.OrderDate;

                    Order updateOrder = new Order()
                    {
                        Id = id,
                        CustomerAdress = adress,
                        CustomerEmail = email,
                        CustomerName = customeName,
                        OrderDate = orderCreate,
                        ShipDate = orderShip,
                        DeliveryDate = delivery
                    };
                    //<Receives preferred order details and updates them>
                    dal.Order.Update(updateOrder);
                    break;
                case SubMenu_CRAUD.Delete:
                    Console.WriteLine(@"
        Enter the Order ID number that you want to remove");
                    int.TryParse(Console.ReadLine(), out id);
                    //<Receives an order number and cancels it>
                    dal.Order.Delete(id);
                    break;
                default:
                    break;
            }

            exitSubMenu = false; // innitilize the  continue flag 
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine(@"
       press 0 to return to Main menu
        presss any other key to continue");

            int.TryParse(Console.ReadLine(), out exit_);
            if (exit_ == (int)SubMenu_CRAUD.ExitSubMenu)
                exitSubMenu = true;

        } while (!exitSubMenu);

    }
    #endregion

    #region OrderItem menu

    // <Gives a submenu for the itemorder entity>
    static void OrderItemMenue()
    {
        
        do
        {
            Console.WriteLine(@"
    Select operation to test:
        1) To add an order item
        2) To show an order item
        3) To Show all order item list
        4) To updat an order item
        5) To delete an order item
        6) To Show an order item by given product and order Id's
        7) To Shw all order item belongs to one order");

            SubMenu_ItemOrder OrderItem;
            int id = int.MinValue;
            int productId = int.MinValue;
            int orderId = int.MinValue;
            int amount = int.MinValue;
            double price = double.MaxValue;
            SubMenu_ItemOrder.TryParse(Console.ReadLine(), out subMenu_ItemOrder);
            switch (subMenu_ItemOrder)
            {
                case SubMenu_ItemOrder.Add:
                    Console.WriteLine(@"
        Enter productID number to add");
                    int.TryParse(Console.ReadLine(), out productId);
                    Console.WriteLine(@"
        Enter Orde Id to add");
                    int.TryParse(Console.ReadLine(), out orderId);
                    Console.WriteLine(@"
        Enter amount to add");
                    int.TryParse(Console.ReadLine(), out amount);
                    Console.WriteLine(@"
        Enter price of product");
                    double.TryParse(Console.ReadLine(), out price);

                    OrderItem addedOrderItem = new OrderItem()
                    {
                        ProductId = productId,
                        OrderId = orderId,
                        Amount = amount,
                        Price = price
                    };
                    //<Receives a new order item and returns its ID numbe>
                    int addedOrderId = dal.OrderItem.Add(addedOrderItem);
                    Console.WriteLine("\tThe Orde item id: " + addedOrderId);
                    break;
                case SubMenu_ItemOrder.View:
                    Console.WriteLine(@"
        Enter the Order Item ID number that you want to see his details");
                    while (!int.TryParse(Console.ReadLine(), out orderId)) ;
                    OrderItem OrderItemToShow = new OrderItem();
                    //<Receives an ID number of the item in the order and returns its details>
                    OrderItemToShow = dal.OrderItem.Get(orderId);
                    Console.WriteLine("\n\t" + OrderItemToShow);
                    break;
                case SubMenu_ItemOrder.ViewAll:
                    //<Returns all items in the order>
                    IEnumerable<OrderItem> OrdersItemList = dal.OrderItem.GetList();
                    foreach (OrderItem orderItem in OrdersItemList)
                        Console.WriteLine("\t" + orderItem);
                    break;
                case SubMenu_ItemOrder.Update:
                    Console.WriteLine(@"
        Enter the Order item ID number that you want to update his details");
                    int.TryParse(Console.ReadLine(), out orderId);
                    OrderItem oldItemOrder = dal.OrderItem.Get(orderId);
                    Console.WriteLine("\t" + oldItemOrder);

                    Console.WriteLine(@"
        Enter the productID number  to update");
                    int.TryParse(Console.ReadLine(), out productId);
                    if (productId == int.MinValue)
                        productId = oldItemOrder.ProductId;

                    Console.WriteLine(@"
        Enter the order to update");
                    int.TryParse(Console.ReadLine(), out orderId);
                    if (orderId == int.MinValue)
                        orderId = oldItemOrder.OrderId;

                    Console.WriteLine(@"
        Enter the amountto update");
                    int.TryParse(Console.ReadLine(), out amount);
                    if (amount == int.MinValue)
                        amount = oldItemOrder.Amount;

                    Console.WriteLine(@"
        Enter the price to update");
                    double.TryParse(Console.ReadLine(), out price);
                    if (price == double.MinValue)
                        price = oldItemOrder.Price;

                    OrderItem updateItemOrder = new OrderItem()
                    {
                        Id = id,
                        ProductId = productId,
                        OrderId = orderId,
                        Amount = amount,
                        Price = price
                    };
                    //<Receives an updated order item and updates it>
                    dal.OrderItem.Update(updateItemOrder);
                    break;
                case SubMenu_ItemOrder.Delete:
                    Console.WriteLine(@"
        Enter the Order item ID number to remove");
                    int.TryParse(Console.ReadLine(), out orderId);
                    //<Receives an ID number of an item in the order and cancels it>
                    dal.OrderItem.Delete(orderId);
                    break;
                case SubMenu_ItemOrder.itemByOrderAndProduct:
                    Console.WriteLine(@"
        Enter the Order ID:");
                    int.TryParse(Console.ReadLine(), out orderId);
                    Console.WriteLine(@"
        Enter the product ID:");
                    int.TryParse(Console.ReadLine(), out productId);
                    //<Receives the ID number of the order and the product and returns the item details in the order>
                    OrderItem itemToShowByProductAndOrder = dal.OrderItem.GetItemByOrderAndProduct(orderId, productId);
                    Console.WriteLine(itemToShowByProductAndOrder);
                    break;
                case SubMenu_ItemOrder.itemListByOrder:
                    Console.WriteLine(@"
        Enter the Order ID number that you want to see Item Order that belogs to him");
                    int.TryParse(Console.ReadLine(), out orderId);
                    //<Receives an ID number of the order and returns all the details of the products in this order>
                    IEnumerable<OrderItem> ordersItemsList = dal.OrderItem.GetItemsListByOrderId(orderId);
                    foreach (OrderItem item in ordersItemsList)
                        Console.WriteLine(item);
                    break;
                default:
                    break;
            }
            exitSubMenu = false; // innitilize the  continue flag 
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine(@"
        press 0 to return to Main menu
        presss any other key to continue");

            int.TryParse(Console.ReadLine(), out exit_);
            if (exit_ == (int)SubMenu_CRAUD.ExitSubMenu)
                exitSubMenu = true;

        } while (!exitSubMenu);

    }
    #endregion
}


//BL test//

//using BlApi;
//using Dal;
//using DalApi;
//using BlImplementation;
//using BO;


//namespace BlTest
//{
//    internal class Program
//    {
//        // enum for main menu
//        enum MainMenuCode { Exit, Product, Order, Cart }
//        // enum for sub menue: order operation
//        enum SubMenu_Order
//        {
//            ExitSubMenu = 0,
//            View,
//            ViewAll,
//            GetTracking,
//            UpdateOrderDelivery,
//            UpdateOrderSheep
//        }
//        private static SubMenu_Order subMenu_Order;
//        private static MainMenuCode menuCode;
//        private static IBl bl = new Bl();

//        static void Main(string[] args)
//        {
//            bool success = false;
//            bool exit = false;
//            do
//            {
//                Console.WriteLine(@"
//    Please select one of the following options to test:
//        1) Product operation
//        2) Order operation
//        3) Cart operation
//        0) To exit from menu");
//                success = MainMenuCode.TryParse(Console.ReadLine(), out menuCode);
//                if (success)
//                {
//                    switch (menuCode)
//                    {
//                        case MainMenuCode.Cart:
//                            //call cart method
//                            break;
//                        case MainMenuCode.Order:
//                            OrderMenu();
//                            break;
//                        case MainMenuCode.Product:
//                            //call product method
//                            break;
//                        case MainMenuCode.Exit:
//                            exit = true;
//                            break;
//                        default:
//                            Console.WriteLine("Invalid value, please try again\n");
//                            break;
//                    }
//                }

//            } while (!exit);

//        }

//        static void OrderMenu()
//        {
//            bool success = false;
//            bool exit = false;
//            int id;
//            do
//            {
//                Console.WriteLine(
//                      "Select operation to test:\n" +
//                      "  1) To show an order\n" +
//                      "  2) To Show all order list\n" +
//                      "  3) To get order tracking\n" +
//                      "  4) To update order delivery\n" +
//                      "  5) To update order sheep\n" +
//                      "  0) To exit from sub menu\n"
//                       );
//                success = SubMenu_Order.TryParse(Console.ReadLine(), out subMenu_Order);
//                if (success)
//                {
//                    switch (subMenu_Order)
//                    {
//                        case SubMenu_Order.ExitSubMenu:
//                            exit = true;
//                            break;
//                        case SubMenu_Order.View:
//                            Console.WriteLine("Please enter order id");
//                            success = int.TryParse(Console.ReadLine(), out id);
//                            if (success)
//                            {
//                                BO.Order order = new BO.Order();
//                                try
//                                {
//                                    order = bl.Order.Get(id);
//                                }
//                                catch (Exception e)
//                                {
//                                    Console.WriteLine(e);
//                                    break;
//                                }
//                                Console.WriteLine(order);
//                            }
//                            break;
//                        case SubMenu_Order.ViewAll:
//                            IEnumerable<BO.OrderForList> ordersForList = new List<BO.OrderForList>();
//                            try
//                            {
//                                ordersForList = bl.Order.GetList();
//                            }
//                            catch (Exception e)
//                            {
//                                Console.WriteLine(e);
//                                break;
//                            }
//                            foreach (BO.OrderForList order in ordersForList)
//                                Console.WriteLine(order);
//                            break;
//                        case SubMenu_Order.GetTracking:
//                            Console.WriteLine("Please enter order id");
//                            success = int.TryParse(Console.ReadLine(), out id);
//                            if (success)
//                            {
//                                BO.OrderTracking orderTracking = new BO.OrderTracking();
//                                try
//                                {
//                                    orderTracking = bl.Order.GetTracking(id);
//                                }
//                                catch (Exception e)
//                                {
//                                    Console.WriteLine(e);
//                                    break;
//                                }
//                                Console.WriteLine(orderTracking);
//                            }
//                            break;
//                        case SubMenu_Order.UpdateOrderDelivery:
//                            Console.WriteLine("Please enter order id");
//                            success = int.TryParse(Console.ReadLine(), out id);
//                            if (success)
//                            {
//                                BO.Order order = new BO.Order();
//                                try
//                                {
//                                    order = bl.Order.UpdateOrderDelivery(id);
//                                }
//                                catch (Exception e)
//                                {
//                                    Console.WriteLine(e);
//                                    break;
//                                }
//                                Console.WriteLine(order);
//                            }
//                            break;
//                        case SubMenu_Order.UpdateOrderSheep:
//                            Console.WriteLine("Please enter order id");
//                            success = int.TryParse(Console.ReadLine(), out id);
//                            if (success)
//                            {
//                                BO.Order order = new BO.Order();
//                                try
//                                {
//                                    order = bl.Order.UpdateOrderSheep(id);
//                                }
//                                catch (Exception e)
//                                {
//                                    Console.WriteLine(e);
//                                    break;
//                                }
//                                Console.WriteLine(order);
//                            }
//                            break;
//                        default:
//                            Console.WriteLine("Invalid value, please try again\n");
//                            break;

//                    }



//                }

//            } while (!exit);
//        }
//    }
//}


//BO//


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BO
//{
//    public class Cart
//    {
//        public string CustomerName { get; set; }
//        public string CustomerEmail { get; set; }
//        public string CustomerAdress { get; set; }
//        public List<OrderItem> Items { get; set; }
//        public double TotalPrice { get; set; }
//        public override string ToString()
//        {
//            string tmp = "";
//            foreach (var item in Items)
//                tmp += item.ToString();
//            string toString;
//            toString = ($@"
//        Name: {CustomerName},
//        Email: {CustomerEmail},
//        Adress: {CustomerAdress}");
//            toString += tmp + ($@"
//        TotalPrice: {TotalPrice}");
//            return toString;
//        }
//    }
//}


//namespace BO;

//public enum Category
//{
//    MEN,
//    WOMANS,
//    BOYS,
//    GIRLS,
//    ACCESSORIES
//};

//public enum Status
//{
//    APPROVED, SHIPPED, DELIVERED
//}





//namespace BO
//{
//    [Serializable]
//    public class DataRequestFailedException : Exception
//    {
//        [NonSerialized] bool message_ = false;
//        public DataRequestFailedException() : base() { }
//        public DataRequestFailedException(string message) : base(message) { message_ = true; }
//        public DataRequestFailedException(string message, Exception inner) : base(message, inner) { }
//        public override string ToString()
//        {
//            string generalMess = "Data Request Failed Exception:";
//            string toString = message_ ? (Message + " " + generalMess + " " + InnerException)
//                : (generalMess + " " + InnerException);
//            return toString;
//        }
//    }

//    [Serializable]
//    public class InvalidValueException : Exception
//    {
//        public InvalidValueException() : base() { }
//        public InvalidValueException(string message) : base(message) { }
//        public override string ToString()
//        {
//            return "invalid input value: " + Message;
//        }
//    }

//    [Serializable]
//    public class InvalidEmailFormatException : Exception
//    {
//        public InvalidEmailFormatException() : base("invalid Email format:") { }
//        public override string ToString()
//        {
//            return @"
//invalid Email forma:
//The format must be: Example@domain.suffix";
//        }
//    }

//    [Serializable]
//    public class AmountAndPriceException : Exception
//    {
//        public AmountAndPriceException() : base() { }
//        public AmountAndPriceException(string message) : base(message) { }
//        public override string ToString()
//        {
//            return "Error - Amount or Price: " + Message;
//        }
//    }
//    [Serializable]
//    public class CantBeDeletedException : Exception
//    {
//        public CantBeDeletedException() : base() { }
//        public CantBeDeletedException(string message) : base(message) { }
//        public override string ToString()
//        {
//            return "The Item can't be deleted: " + Message;
//        }
//    }
//}





//namespace BO
//{
//    public class Order
//    {
//        public int Id { get; set; }
//        public string CustomerName { get; set; }
//        public string CustomerEmail { get; set; }
//        public string CustomerAdress { get; set; }
//        public DateTime? OrderDate { get; set; }
//        public DateTime? ShipDate { get; set; }
//        public DateTime? DeliveryDate { get; set; }
//        //public DateTime PaymentDate { get; set; }
//        public double TotalPrice { get; set; }
//        public List<OrderItem> Items { get; set; }
//        public Status status { get; set; }
//        public override string ToString()
//        {
//            string str = "";
//            foreach (var item in Items)
//                str += item.ToString();

//            return $@"
//            Order Id: {Id}
//            Name: {CustomerName}
//            Email: {CustomerEmail}
//            Adress: {CustomerAdress}
//            Order date: {OrderDate}
//            Ship Date: {ShipDate}
//            Delivery Date: {DeliveryDate}
//            Status: {status}
//            Items: {str}
//            Total Price: {TotalPrice}";

//        }


//    }
//}


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BO
//{
//    public class OrderForList
//    {
//        public int Id { get; set; }
//        public string CustomerName { get; set; }
//        public int AmountOfItems { get; set; }
//        public double TotalPrice { get; set; }
//        public Status status { get; set; }
//        public override string ToString() => $@"
//        Order Id: {Id},
//        Name: {CustomerName},
//        Amount of items: {AmountOfItems},
//        Total Price: {TotalPrice},
//        Status: {status}";

//    }
//}


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BO
//{
//    public class OrderItem
//    {
//        public int Id { get; set; }
//        public string ProductName { get; set; }
//        public int Amount { get; set; }
//        public int ProductId { get; set; }
//        public double Price { get; set; }
//        public double TotalPrice { get; set; }
//        public override string ToString() => $@"
//        Order item Id: {Id},
//        Product Name: {ProductName},
//        Product Id: {ProductId},
//        Price: {Price},
//        Amount: {Amount},
//        Total Price: {TotalPrice}";
//    }
//}


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BO
//{
//    public class OrderTracking
//    {
//        public int ID { get; set; }
//        public Status status { get; set; }
//        public List<(DateTime?, string)>? TrackingList { get; set; }
//        public override string ToString() => $@"
//        Order ID: {ID}, 
//        Status: {status}";
//    }
//}



//namespace BO;

//public class Product
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public double Price { get; set; }

//    public Category Category { get; set; }
//    public int InStock { get; set; }

//    public override string ToString() => $@"
//    Product Id : {Id},
//    Name :       {Name},
//    Category:    {Category},
//    Price:       {Price},
//    Amount in stock: {InStock}
//";
//}


// bl implation//

//using BlApi;


//namespace BlImplementation
//{
//    sealed public class Bl : IBl
//    {
//        public IProduct Product => new Product();

//        public IOrder Order => new Order();

//        public ICart Cart => new Cart();
//    }
//}


//using BlApi;
//using BO;
//using DO;
//using System.ComponentModel.DataAnnotations;

//namespace BlImplementation
//{
//    internal class Cart : ICart
//    {
//        private DalApi.IDal Dal = new Dal.DalList(); //Using it we can access the data access classes
//        /// <summary>
//        /// add a product to the Cart of Customer
//        /// </summary>
//        /// <param name="cart"></param>
//        /// <param name="productId"></param>
//        /// <returns>the cart withr the product adeed</returns>
//        /// <exception cref="BO.DataRequestFailedException"></exception>
//        public BO.Cart Add(BO.Cart cart, int productId)
//        {
//            DO.Product dataProduct = new DO.Product();
//            try
//            {
//                dataProduct = Dal.Product.Get(productId);
//            }
//            catch (Exception e)
//            {
//                throw new BO.DataRequestFailedException(e.Message);
//            }
//            //If the product does not exist in the cart, then add a new product
//            if (!cart.Items.Exists(x => x.ProductId == productId))
//            {
//                if (dataProduct.InStock > 0)
//                {
//                    BO.OrderItem orderItem = new BO.OrderItem()
//                    {
//                        ProductId = productId,
//                        ProductName = dataProduct.Name,
//                        Amount = 1,
//                        Id = 0,
//                        Price = dataProduct.Price,
//                        TotalPrice = dataProduct.Price
//                    };
//                    cart.Items.Add(orderItem);
//                    cart.TotalPrice += dataProduct.Price;

//                }

//            }
//            //If the product is in the cart, update the amount and price
//            else if (dataProduct.InStock > 0)
//            {
//                int i = cart.Items.FindIndex(x => x.ProductId == productId);
//                cart.Items[i].Amount += 1;
//                cart.Items[i].TotalPrice += cart.Items[i].Price;
//                cart.TotalPrice += cart.Items[i].Price;
//            }

//            return cart;
//        }

//        /// <summary>
//        /// Approve the cart of customer:
//        /// chack validation of data in cart
//        /// Checks that there is sufficient quantity in stock for the order
//        /// update the Amount of product in Dal
//        /// </summary>
//        /// <param name="cart"></param>
//        /// <param name="customerName"></param>
//        /// <param name="customerEmail"></param>
//        /// <param name="customerAdress"></param>
//        /// <exception cref="InvalidValueException"></exception>
//        /// <exception cref="InvalidEmailFormatException"></exception>
//        /// <exception cref="DataRequestFailedException"></exception>
//        /// <exception cref="AmountAndPriceException"></exception>
//        public void Approve(BO.Cart cart, string customerName, string customerEmail, string customerAdress)
//        {
//            //check validation of Customer data parameters
//            if (customerName == null)
//                throw new InvalidValueException("Name of customer can't be empthy");
//            if (customerAdress == null)
//                throw new InvalidValueException("adress of customer can't be empthy");
//            if (customerEmail == null)
//                throw new InvalidValueException("Email of customer can't be empthy");
//            if (!new EmailAddressAttribute().IsValid(customerEmail))
//                throw new InvalidEmailFormatException();
//            //check validation of cart parameter
//            if (cart.CustomerEmail != customerEmail)
//                throw new InvalidValueException("Email adress in cart not equal to EmailAdresss parameter");
//            if (cart.CustomerAdress != customerAdress)
//                throw new InvalidValueException("Adress in cart not equal to Adresss parameter");
//            if (cart.CustomerName != customerName)
//                throw new InvalidValueException("Customer name in cart not equal to Customer name parameter");
//            double totalPrice_ = 0;
//            DO.Product product_ = new();

//            //chack validation of each ItemOrder in cart
//            foreach (var item in cart.Items)
//            {
//                try
//                {
//                    product_ = Dal.Product.Get(item.ProductId);
//                }
//                catch (Exception e)
//                {
//                    throw new DataRequestFailedException($"ERROR in {item.ProductName}:", e);
//                }
//                if (item.Amount <= 0)
//                    throw new InvalidValueException(item.ProductName + " must be greater than zero");
//                if (product_.InStock < item.Amount)
//                    throw new AmountAndPriceException($"The product {item.ProductName} (ID:) {item.ProductId} is out of stock");
//                if (item.Price != product_.Price)
//                    throw new AmountAndPriceException($"price in cart of {item.ProductName} not match to price in Data Surce");
//                if (item.TotalPrice != (item.Amount * product_.Price))
//                    throw new AmountAndPriceException($"Total price of {item.ProductId} not match to Price and Amount in Cart");
//                totalPrice_ += item.TotalPrice;
//            }
//            if (totalPrice_ != cart.TotalPrice)
//                throw new AmountAndPriceException("Total price in cart not match to prices and Amont of all item in cart");

//            //creat a new  dal order
//            DO.Order order = new()
//            {
//                CustomerName = customerName,
//                CustomerAdress = customerAdress,
//                CustomerEmail = customerEmail,
//                OrderDate = DateTime.Now,
//                ShipDate = null,
//                DeliveryDate = null
//            };
//            try
//            {
//                //try to add order to data sirce in Dal
//                int orderId = Dal.Order.Add(order);
//                //create orderItem in Dal and update amount of product
//                DO.OrderItem orderItem = new();
//                foreach (var item in cart.Items)
//                {
//                    //create an orderItem for dall and add it
//                    orderItem.OrderId = orderId;
//                    orderItem.ProductId = item.ProductId;
//                    orderItem.Amount = item.Amount;
//                    orderItem.Price = item.Price;
//                    int orderItemId = Dal.OrderItem.Add(orderItem);
//                    //update amount of product in Dak
//                    product_ = Dal.Product.Get(item.ProductId);
//                    product_.InStock -= item.Amount;
//                    Dal.Product.Update(product_);
//                }
//            }
//            catch (Exception e)
//            {

//                throw new DataRequestFailedException(e.Message);
//            }
//        }

//        /// <summary>
//        /// update amount of a product in cart: chack if it's possible
//        /// </summary>
//        /// <param name="cart"></param>
//        /// <param name="productId"></param>
//        /// <param name="newAmount"></param>
//        /// <returns></returns>
//        /// <exception cref="BO.DataRequestFailedException"></exception>
//        public BO.Cart Update(BO.Cart cart, int productId, int newAmount)
//        {
//            int i = cart.Items.FindIndex(x => x.ProductId == productId);
//            if (i < 0)
//            {
//                throw new BO.DataRequestFailedException("knkn"); //?
//            }
//            //Adding items from an existing product
//            else if (cart.Items[i].Amount < newAmount)
//            {
//                int additionalItems = newAmount - cart.Items[i].Amount;
//                cart.Items[i].Amount = newAmount;
//                cart.Items[i].TotalPrice += cart.Items[i].Price * additionalItems;
//                cart.TotalPrice += cart.Items[i].Price * additionalItems;
//            }
//            //Deleting a product from the cart
//            else if (newAmount == 0)
//            {
//                cart.TotalPrice -= cart.Items[i].TotalPrice;
//                cart.Items.RemoveAt(i);
//            }
//            //Reducing items from an existing product
//            else
//            {
//                int reducingItems = cart.Items[i].Amount - newAmount;
//                cart.Items[i].Amount = newAmount;
//                cart.Items[i].TotalPrice -= cart.Items[i].Price * reducingItems;
//                cart.TotalPrice -= cart.Items[i].Price * reducingItems;
//            }
//            return cart;
//        }
//    }
//}


//using BlApi;
////using BO;
////using DO;
////using BO;
//using System.Collections.Generic;

//namespace BlImplementation;

//internal class Order : IOrder
//{
//    private DalApi.IDal Dal = new Dal.DalList(); //Using it we can access the data access classes

//    /// <summary>
//    /// Returns an order (logical entity) by order id
//    /// </summary>
//    /// <param name="orderId"></param>
//    /// <returns></returns>
//    /// <exception cref="ArgumentOutOfRangeException"></exception>
//    public BO.Order Get(int orderId)
//    {
//        DO.Order dataOrder = new DO.Order();
//        if (orderId <= 0)
//        {
//            throw new BO.InvalidValueException("Id must be greater than zero");
//        }
//        //Try requesting an order from data layer
//        try
//        {
//            dataOrder = Dal.Order.Get(orderId);
//        }
//        catch (Exception e)
//        {
//            throw new BO.DataRequestFailedException(e.Message);
//        }

//        IEnumerable<DO.OrderItem> items = new List<DO.OrderItem>();
//        //Try requesting a List of orderItems from a data layer
//        try
//        {
//            items = Dal.OrderItem.GetItemsListByOrderId(orderId);
//        }
//        catch (Exception e)
//        {
//            throw new BO.DataRequestFailedException(e.Message);
//        }

//        //Creating a list of orderItems - logical entities
//        List<BO.OrderItem> orderItems = new List<BO.OrderItem>();
//        foreach (DO.OrderItem item in items)
//        {
//            orderItems.Add(new BO.OrderItem
//            {
//                Id = item.Id,
//                Amount = item.Amount,
//                Price = item.Price,
//                ProductId = item.ProductId,
//                TotalPrice = item.Price * item.Amount,
//                ProductName = Dal.Product.Get(item.ProductId).Name
//            });
//        }

//        //Calculates the total order price
//        double totalOrderPrice = 0;
//        foreach (BO.OrderItem item in orderItems)
//            totalOrderPrice += item.TotalPrice;

//        //Calculates the status according to the order data in relation to the current time.
//        BO.Status status_ = GetStatus(dataOrder);

//        //Create Order - logical entity
//        BO.Order boOrder = new BO.Order()
//        {
//            Id = dataOrder.Id,
//            CustomerName = dataOrder.CustomerName,
//            CustomerEmail = dataOrder.CustomerEmail,
//            CustomerAdress = dataOrder.CustomerAdress,
//            OrderDate = dataOrder.OrderDate,
//            ShipDate = dataOrder.ShipDate,
//            DeliveryDate = dataOrder.DeliveryDate,
//            //Items = orderItems,
//            TotalPrice = totalOrderPrice,
//            status = status_
//        };

//        boOrder.Items = new List<BO.OrderItem>();
//        foreach (BO.OrderItem item in orderItems)
//            boOrder.Items.Add(item);
//        return boOrder;
//    }

//    /// <summary>
//    /// Returns a list of orders
//    /// </summary>
//    /// <returns></returns>
//    public IEnumerable<BO.OrderForList> GetList()
//    {
//        IEnumerable<DO.Order> orders = new List<DO.Order>();
//        //Try requesting order list from a data layer
//        orders = Dal.Order.GetList();

//        //Creating a list of OrderForList -logical entities
//        List<BO.OrderForList> ordersForList = new List<BO.OrderForList>();

//        //Populates the list by creating "OrderForList" type objects based on order data and OrderItem data and additional calculations.
//        foreach (DO.Order order in orders)
//        {
//            //For each order, request the list of orderItems
//            IEnumerable<DO.OrderItem> items = new List<DO.OrderItem>();
//            items = Dal.OrderItem.GetItemsListByOrderId(order.Id);

//            int amountOfItems = items.Count();

//            //Calculates the Total Price
//            double totalPrice = 0;
//            foreach (DO.OrderItem item in items)
//                totalPrice += item.Price * item.Amount;

//            //Calculates the status according to the order data in relation to the current time.
//            BO.Status status_ = GetStatus(order);

//            ordersForList.Add(new BO.OrderForList()
//            {
//                Id = order.Id,
//                CustomerName = order.CustomerName,
//                AmountOfItems = amountOfItems,
//                TotalPrice = totalPrice,
//                status = status_
//            });

//        }

//        return ordersForList;
//    }

//    public BO.OrderTracking GetTracking(int orderId)
//    {
//        DO.Order dataOrder = new DO.Order();
//        try
//        {
//            dataOrder = Dal.Order.Get(orderId);
//        }
//        catch (Exception e)
//        {
//            throw new BO.DataRequestFailedException(e.Message);
//        }
//        BO.Status status_ = GetStatus(dataOrder);
//        BO.OrderTracking orderTracking = new BO.OrderTracking()
//        {
//            ID = orderId,
//            status = status_
//        };

//        orderTracking.TrackingList = new List<(DateTime?, string)>();
//        orderTracking.TrackingList.Add((dataOrder.OrderDate, "The order created"));
//        if (dataOrder.ShipDate != null)
//            orderTracking.TrackingList.Add((dataOrder.ShipDate, "The order shipped"));
//        if (dataOrder.DeliveryDate != null)
//            orderTracking.TrackingList.Add((dataOrder.DeliveryDate, "The order delivered"));

//        return orderTracking;
//    }

//    public BO.Order UpdateOrderDelivery(int orderId)
//    {
//        BO.Order boOrder = new BO.Order();
//        DO.Order dataOrder = new DO.Order();
//        try
//        {
//            dataOrder = Dal.Order.Get(orderId);
//            boOrder = Get(orderId);
//        }
//        catch (DO.NotFoundException e)
//        {
//            throw new BO.DataRequestFailedException(e.Message);
//        }
//        catch (BO.InvalidValueException e)
//        {
//            throw new BO.InvalidValueException(e.Message);
//        }

//        //If the order has been shipped (but not yet delivered) then update the delivery date
//        if (boOrder.status == BO.Status.SHIPPED)
//        {
//            dataOrder.DeliveryDate = DateTime.Now;
//            boOrder.DeliveryDate = DateTime.Now;
//            boOrder.status = BO.Status.DELIVERED;
//        }

//        //Attempting to update the data layer
//        try
//        {
//            Dal.Order.Update(dataOrder);
//        }
//        catch (DO.NotFoundException e)
//        {
//            throw new BO.DataRequestFailedException(e.Message);
//        }

//        return boOrder;
//    }

//    public BO.Order UpdateOrderSheep(int orderId)
//    {
//        BO.Order boOrder = new BO.Order();
//        DO.Order dataOrder = new DO.Order();
//        try
//        {
//            dataOrder = Dal.Order.Get(orderId);
//            boOrder = Get(orderId);
//        }
//        catch (DO.NotFoundException e)
//        {
//            throw new BO.DataRequestFailedException(e.Message);
//        }
//        catch (BO.InvalidValueException e)
//        {
//            throw new BO.InvalidValueException(e.Message);
//        }


//        //If the given status is that the order has been approved (but not yet shipped),
//        //then update the delivery date to now.
//        if (boOrder.status == BO.Status.APPROVED)
//        {
//            dataOrder.ShipDate = DateTime.Now;
//            boOrder.ShipDate = DateTime.Now;
//            boOrder.status = BO.Status.SHIPPED;
//        }

//        //Attempting to update the data layer
//        try
//        {
//            Dal.Order.Update(dataOrder);
//        }
//        catch (DO.NotFoundException e)
//        {
//            throw new BO.DataRequestFailedException(e.Message);
//        }

//        return boOrder;
//    }

//    public BO.Status GetStatus(DO.Order order)
//    {
//        BO.Status status;
//        if (order.DeliveryDate != null)
//            status = BO.Status.DELIVERED;
//        else
//        {
//            if (order.ShipDate != null)
//                status = BO.Status.SHIPPED;
//            else status = BO.Status.APPROVED;
//        }
//        return status;
//    }
//}



//using BlApi;
//using BO;
//using Dal;
//using DalApi;
//using System.Collections.Generic;

//namespace BlImplementation
//{
//    /// <summary>
//    /// Icraud method of product and some other method. implementaion of IProduct
//    /// </summary>
//    internal class Product : BlApi.IProduct
//    {
//        DalApi.IDal Dal = new Dal.DalList();
//        /// <summary>
//        /// add a Bl product => chack logical valid of data and add to Dal data surce
//        /// </summary>
//        /// <param name="product"></param>
//        /// <exception cref="BO.InvalidValueException"></exception>
//        /// <exception cref="DataRequestFailedException"></exception>
//        public void Add(BO.Product product)
//        {
//            //Validity checks of input format
//            if (product.Id <= 0)
//                throw new BO.InvalidValueException("Id must be greater than zero");
//            if (product.Name == null)
//                throw new BO.InvalidValueException("Name can't be empthy be greater than zero"
//            if (product.Price <= 0)
//                throw new BO.InvalidValueException("Price must be greater than zero");
//            if (product.InStock < 0)
//                throw new BO.InvalidValueException("InStock Value must be greater than zero");

//            //add a Dal Product to DalList
//            DO.Product DoProduct = new DO.Product()
//            {
//                Id = product.Id,
//                Name = product.Name,
//                Price = product.Price,
//                InStock = product.InStock,
//                Category = (DO.Category)product.Category
//            };
//            try
//            {
//                int i = Dal.Product.Add(DoProduct);
//            }
//            catch (Exception e)
//            {

//                throw new DataRequestFailedException(e.Message);
//            }
//        }

//        /// <summary>
//        /// delete a Bl product => chack logical valid of data and delete from Dal data surce
//        /// </summary>
//        /// <param name="product"></param>
//        /// <exception cref="ArgumentException"></exception>
//        public void Delete(BO.Product product)
//        {
//            //chack if product id exsist in orderItem List
//            if (IsHasBeenOrderd(product.Id))
//                throw new CantBeDeletedException("The product exist in Item Order List");
//            try
//            {
//                Dal.Product.Delete(product.Id);
//            }
//            catch (Exception e)
//            {

//                throw new DataRequestFailedException(e.Message);
//            }

//        }

//        /// <summary>
//        /// get an product from dak and create a lojicial product and return it
//        /// </summary>
//        /// <param name="productId"></param>
//        /// <returns>BO.Product</returns>
//        /// <exception cref="InvalidValueException"></exception>
//        /// <exception cref="DataRequestFailedException"></exception>
//        public BO.Product Get(int productId)
//        {
//            if (productId <= 0)
//                throw new InvalidValueException("id must be greater than zero");
//            DO.Product dalProduct = new DO.Product();
//            try
//            {
//                dalProduct = Dal.Product.Get(productId);
//            }
//            catch (Exception e)
//            {

//                throw new DataRequestFailedException(e.Message);
//            }
//            BO.Product result = new BO.Product()
//            {
//                Id = dalProduct.Id,
//                Name = dalProduct.Name,
//                Price = dalProduct.Price,
//                Category = (BO.Category)dalProduct.Category,
//                InStock = dalProduct.InStock,
//            };
//            return result;
//        }

//        /// <summary>
//        /// get a product from Dal and create a logicial ProductItem to show un Costomer cart
//        /// </summary>
//        /// <param name="productId"></param>
//        /// <param name="cart"></param>
//        /// <returns>BO.ProductItem</returns>
//        /// <exception cref="InvalidValueException"></exception>
//        /// <exception cref="DataRequestFailedException"></exception>
//        public BO.ProductItem Get(int productId, BO.Cart cart)
//        {
//            if (productId <= 0)
//                throw new InvalidValueException("id must be greater than zero");
//            DO.Product dalProduct = new DO.Product();
//            try
//            {
//                dalProduct = Dal.Product.Get(productId);
//            }
//            catch (Exception e)
//            {

//                throw new DataRequestFailedException(e.Message);
//            }
//            //calculate the logicial data and return a logicial product for cart screan (customer)
//            bool inStock_ = dalProduct.InStock > 0;
//            int amount_ = 0;
//            foreach (BO.OrderItem item in cart.Items)
//            {
//                if (item.Id == productId)
//                    amount_++;
//            }
//            BO.ProductItem productItem = new BO.ProductItem()
//            {
//                Id = dalProduct.Id,
//                Name = dalProduct.Name,
//                Price = dalProduct.Price,
//                Category = (BO.Category)dalProduct.Category,
//                InStock = inStock_,
//                Amount = amount_
//            };
//            return productItem;
//        }

//        /// <summary>
//        /// get list of product from Dal and create a logicial productForList
//        /// </summary>
//        /// <returns>BO.ProductItem</returns>
//        /// <exception cref="DataRequestFailedException"></exception>
//        public IEnumerable<BO.ProductForList> GetList()
//        {
//            try
//            {
//                IEnumerable<DO.Product> dalProducts = Dal.Product.GetList();
//                List<BO.ProductForList> result = new List<BO.ProductForList>();

//                foreach (DO.Product product in dalProducts)
//                {
//                    result.Add(new BO.ProductForList()
//                    {
//                        Id = product.Id,
//                        Name = product.Name,
//                        Category = (BO.Category)product.Category,
//                        Price = product.Price
//                    });
//                }

//                return result;
//            }
//            catch (Exception e)
//            {

//                throw new DataRequestFailedException(e.Message);
//            }
//        }

//        /// <summary>
//        /// get as a parameter a logicial Product,
//        /// chack validation of data and send a DO product to update in Dal
//        /// </summary>
//        /// <param name="product"></param>
//        /// <exception cref="BO.InvalidValueException"></exception>
//        /// <exception cref="DataRequestFailedException"></exception>
//        public void Update(BO.Product product)
//        {
//            //Validity checks of input format
//            if (product.Id <= 0)
//                throw new BO.InvalidValueException("Id must be greater than zero");
//            if (product.Name == null)
//                throw new BO.InvalidValueException("Name can't be empthy be greater than zero"
//            if (product.Price <= 0)
//                throw new BO.InvalidValueException("Price must be greater than zero");
//            if (product.InStock < 0)
//                throw new BO.InvalidValueException("InStock Value must be greater than zero");

//            DO.Product item = new DO.Product()
//            {
//                Id = product.Id,
//                Name = product.Name,
//                Price = product.Price,
//                Category = (DO.Category)product.Category,
//                InStock = product.InStock
//            };
//            try
//            {
//                Dal.Product.Update(item);
//            }
//            catch (Exception e)
//            {

//                throw new DataRequestFailedException(e.Message);
//            }

//        }

//        /// <summary>
//        /// chack if Has the product been ordered?
//        /// </summary>
//        /// <param name="productId"></param>
//        /// <returns>false or true</returns>
//        public bool IsHasBeenOrderd(int productId)
//        {
//            bool exist = false;

//            exist = Dal.OrderItem.GetList().Any(x => x.ProductId == productId);
//            return exist;
//        }
//    }
//}


//BLaPI//

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Principal;
//using System.Text;
//using System.Threading.Tasks;

//namespace BlApi
//{
//    public interface IBl
//    {
//        public IProduct Product { get; }
//        public IOrder Order { get; }
//        public ICart Cart { get; }
//    }
//}




//using BO;

//namespace BlApi
//{
//    public interface ICart
//    {
//        public Cart Add(Cart cart, int productId);
//        public Cart Update(Cart cart, int productId, int newAmount);
//        public void Approve(Cart cart, string customerName, string customerEmail, string customerAdress);
//    }
//}


//using BO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BlApi
//{
//    public interface IOrder
//    {
//        public Order UpdateOrderSheep(int orderId);
//        public Order UpdateOrderDelivery(int orderId);
//        public Order Get(int orderId);
//        public IEnumerable<OrderForList> GetList();
//        public OrderTracking GetTracking(int orderId);
//    }
//}


//using BO;
//namespace BlApi
//{
//    public interface IProduct
//    {
//        public void Add(Product product);
//        public void Update(Product product);
//        public void Delete(Product product);
//        public Product Get(int productId);
//        public ProductItem Get(int productId, Cart cart);
//        public IEnumerable<ProductForList> GetList();
//    }
//}
