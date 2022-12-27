using DalApi;
using DO;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Dal;
#region Emums
/// <summary>
/// enums for main menu
/// </summary>
enum MainMenuCode { Exit, Product, Order, OrderItem };
/// <summary>
/// enum to for CRAUD option in sub menus
/// </summary>
enum SubMenu_CRAUD { ExitSubMenu = 0, Add, View, ViewAll, Update, Delete, };
/// <summary>
/// special sub menu to check additional operation OrderItem
/// </summary>
enum SubMenu_OrderItem { Exit, Add, View, ViewAll, Update, Delete, itemByOrderAndProduct, itemListByOrder };
#endregion     
class Program
{
    #region Variable enum
    private static MainMenuCode menuCode;// enum for main menu
    private static SubMenu_CRAUD subMenu_CRAUD;// enum for sub menue: craud operation
    private static SubMenu_OrderItem subMenu_OrderItem; //enum for OrderItem sub menue: includ also privet method of OrderItem
    #endregion

    private static DalApi.IDal? dal = DalApi.Factory.Get();
    static void Main(string[] args)
    {

        Console.WriteLine("\nWelcome to the 'Jerusalem Shoes' shoe Store");

        do
        {
            Console.WriteLine(@"Please select one of the following options to test
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
                        throw new Exception("Invalide choice");
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
        int id;
        string? name = null;
        //DO.Category? category_ = null;
        DO.Category tmpCategory_ = 0;
        double price;
        int inStock;
        bool success = false;
        do
        {
            Console.WriteLine(@"
    Select operation to test:
        0) to return to main menu
        1) To add a product
        2) To show a product
        3) To Show all product list
        4) To updat a product
        5) To delete a product");

          
            try
            {
                success = SubMenu_CRAUD.TryParse(Console.ReadLine(), out subMenu_CRAUD);
                if (!success)
                    throw new DalTest.InvalidInputFormatException("please use only an intiger number from the menu");
                switch (subMenu_CRAUD)
                {
                    case SubMenu_CRAUD.ExitSubMenu:
                        break;
                    case SubMenu_CRAUD.Add:
                        Console.WriteLine(@"
        Enter the product ID number that you want to add");
                        success = int.TryParse(Console.ReadLine(), out id);
                        if (!success)
                            throw new DalTest.InvalidInputFormatException("please use only an intiger number");
                        Console.WriteLine(@"
        Enter a product name that you want to add");
                        name = Console.ReadLine();
                        Console.WriteLine(@"
        Enter a product category that you want to add");
                        success = DO.Category.TryParse(Console.ReadLine(), out tmpCategory_);
                        if (!success)
                            throw new DalTest.InvalidInputFormatException("please use only a Category name");
                        Console.WriteLine(@"
        Enter a product price that you want to add");
                        success = double.TryParse(Console.ReadLine(), out price);
                        if (!success)
                            throw new DalTest.InvalidInputFormatException("please use only an double number");
                        Console.WriteLine(@"
        Enter a product Amount in stok that you want to add");
                        success = int.TryParse(Console.ReadLine(), out inStock);
                        if (!success)
                            throw new DalTest.InvalidInputFormatException("please use only an intiger number");
                        Product addedProduct = new Product()
                        {
                            Id = id,
                            Name = name,
                            Category = tmpCategory_,
                            Price = price,
                            InStock = inStock
                        };
                        //<Receives a new product and returns his ID>
                        int addedProductId = dal?.Product.Add(addedProduct) ?? throw new NullException();
                        Console.WriteLine("\tThe Product id: " + addedProductId);
                        break;
                    case SubMenu_CRAUD.View:
                        Console.WriteLine(@"
        Enter the product ID number that you want to see his details");
                        success = int.TryParse(Console.ReadLine(), out id);
                        if (!success)
                            throw new DalTest.InvalidInputFormatException("please entry only an intiger number");
                        Product productToShow = new Product();
                        //<Receives a product ID and returns all product details>
                        productToShow = dal?.Product.Get(product => product?.Id == id) ?? throw new NullException();
                        Console.WriteLine("\n\t" + productToShow);
                        break;
                    case SubMenu_CRAUD.ViewAll:
                        //<Returns all products in the store>
                        IEnumerable<Product?> productsList = dal?.Product.GetList() ?? throw new NullException(); ;
                        foreach (Product item in productsList)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                    case SubMenu_CRAUD.Update:
                        Console.WriteLine(@"
        Enter the product ID number that you want to update his details");
                        success = int.TryParse(Console.ReadLine(), out id);
                        if(!success)
                            throw new DalTest.InvalidInputFormatException("please entry only an intiger number");
                        Product oldProduct = dal.Product.Get(product => product?.Id == id);
                        Console.WriteLine(oldProduct);

                        Console.WriteLine("enter new data to updated in Product (only in failde yo want to update):");
                        name = null; //to check after if the user put a value for update
                        Console.WriteLine(@"
        name:");
                        name = Console.ReadLine();

                        Console.WriteLine(@"
        category:");

                        success = DO.Category.TryParse(Console.ReadLine(), out tmpCategory_);
                        if (!success)
                            tmpCategory_ = (DO.Category)oldProduct.Category;

                        Console.WriteLine(@"
       price:");
                        price = double.MinValue; //to check after if the user put a value for update
                        double.TryParse(Console.ReadLine(), out price);
                       inStock = int.MinValue; //to check after if the user put a value for update
                        Console.WriteLine(@"
        amount in stok:");
                        int.TryParse(Console.ReadLine(), out inStock);

                        //if user not update keep the old value
                        if (name == "")
                            name = oldProduct.Name;
                        if (price == double.MinValue)
                            price = oldProduct.Price;
                        if (inStock == int.MinValue)
                            inStock = oldProduct.InStock;
                        Product updateProduct = new Product()
                        {
                            Id = id,
                            Name = name, // if name is null kipe the old value
                            Category = tmpCategory_,
                            Price = price,
                            InStock = inStock
                        };
                        //<Receives preferred product details and updates it>
                        dal.Product.Update(updateProduct);
                        Console.WriteLine("The item was updated successfuly");
                        break;
                    case SubMenu_CRAUD.Delete:
                        Console.WriteLine(@"
        Enter the product ID number that you want to remove");
                        int.TryParse(Console.ReadLine(), out id);
                        //<Receives a product tag and deletes it from the menu>
                        dal?.Product.Delete(id);
                        Console.WriteLine("The Item was deleted successfuly");
                        break;
                    default:
                        throw new DalTest.InvalidInputFormatException("please select one of the menu option");
                        break;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("ERROR: " + e);
            }

            Console.ReadKey();
            Console.Clear();
            Console.WriteLine(@"
        presss any  key to continue");
            Console.ReadKey();

        } while (subMenu_CRAUD != SubMenu_CRAUD.ExitSubMenu);
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

            bool success;
            int id;
            string? customeName = null;
            string? email = null;
            string? adress = null;
            DateTime orderCreate;
            DateTime orderShip;
            DateTime delivery;
            try
            {
                success = SubMenu_CRAUD.TryParse(Console.ReadLine(), out subMenu_CRAUD);
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

                        orderCreate = Convert.ToDateTime(Console.ReadLine());
                        success = DateTime.TryParse(Console.ReadLine(), out orderCreate);
                        if (!success)
                            throw new DalTest.InvalidInputFormatException("please entry a DateTime format");
                        Console.WriteLine(@"
        Enter a Ship Order  time");
                        success = DateTime.TryParse(Console.ReadLine(), out orderShip);
                        if (!success)
                            throw new DalTest.InvalidInputFormatException("please entry a DateTime format");
                        Console.WriteLine(@"
        Enter a delivery Order time");
                        success = DateTime.TryParse(Console.ReadLine(), out delivery);
                        if (!success)
                            throw new DalTest.InvalidInputFormatException("please entry a DateTime format");
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
                        orderToShow = dal.Order.Get(order => order?.Id == id);
                        Console.WriteLine("\n\t" + orderToShow);
                        break;
                    case SubMenu_CRAUD.ViewAll:
                        //<Returns all order details>
                        IEnumerable<Order?> OrdersList = dal.Order.GetList();
                        foreach (Order Order in OrdersList)
                            Console.WriteLine("\t" + Order);
                        break;
                    case SubMenu_CRAUD.Update:

                        Console.WriteLine(@"Enter the Order ID number that you want to update his details");
                        int.TryParse(Console.ReadLine(), out id);
                        Order oldOrder = dal.Order.Get(order => order?.Id == id);
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
                            orderCreate = (DateTime)oldOrder.OrderDate;

                        Console.WriteLine(@"
        Enter the new ship date");
                        DateTime.TryParse(Console.ReadLine(), out orderShip);
                        if (orderShip == DateTime.MinValue)
                            orderShip = (DateTime)oldOrder.OrderDate;

                        Console.WriteLine(@"Enter the new delivery date");
                        DateTime.TryParse(Console.ReadLine(), out delivery);
                        if (delivery == DateTime.MinValue)
                            delivery = (DateTime)oldOrder.OrderDate;

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
                        Console.WriteLine("The item was updated successfuly");
                        break;
                    case SubMenu_CRAUD.Delete:
                        Console.WriteLine(@"
        Enter the Order ID number that you want to remove");
                        int.TryParse(Console.ReadLine(), out id);
                        //<Receives an order number and cancels it>
                        dal.Order.Delete(id);
                        break;
                    default:
                        throw new DalTest.InvalidInputFormatException("please select one of the menu option");
                        break;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine(@"
        presss any key to continue");
            Console.ReadKey();
        } while (subMenu_CRAUD != SubMenu_CRAUD.ExitSubMenu);

    }
    #endregion

    #region OrderItem menu

    // <Gives a submenu for the OrderItem entity>
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
        7) To Shw all order item belongs to one order
        0) To Exit the sub menu");

            SubMenu_OrderItem OrderItem;
            int id = int.MinValue;
            int productId = int.MinValue;
            int orderId = int.MinValue;
            int amount = int.MinValue;
            double price = double.MaxValue;
            SubMenu_OrderItem.TryParse(Console.ReadLine(), out subMenu_OrderItem);
            switch (subMenu_OrderItem)
            {
                case SubMenu_OrderItem.Add:
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
                case SubMenu_OrderItem.View:
                    Console.WriteLine(@"
        Enter the Order Item ID number that you want to see his details");
                    while (!int.TryParse(Console.ReadLine(), out orderId)) ;
                    OrderItem OrderItemToShow = new OrderItem();
                    //<Receives an ID number of the item in the order and returns its details>
                    OrderItemToShow = dal.OrderItem.Get(orderItem => orderItem?.Id == id);
                    Console.WriteLine("\n\t" + OrderItemToShow);
                    break;
                case SubMenu_OrderItem.ViewAll:
                    //<Returns all items in the order>
                    IEnumerable<OrderItem?> OrdersItemList = dal.OrderItem.GetList();
                    foreach (OrderItem orderItem in OrdersItemList)
                        Console.WriteLine("\t" + orderItem);
                    break;
                case SubMenu_OrderItem.Update:
                    Console.WriteLine(@"
        Enter the Order item ID number that you want to update his details");
                    int.TryParse(Console.ReadLine(), out orderId);
                    OrderItem oldOrderItem = dal.OrderItem.Get(orderItem => orderItem?.Id == id);
                    Console.WriteLine("\t" + oldOrderItem);

                    Console.WriteLine(@"
        Enter the productID number  to update");
                    int.TryParse(Console.ReadLine(), out productId);
                    if (productId == int.MinValue)
                        productId = oldOrderItem.ProductId;

                    Console.WriteLine(@"
        Enter the order to update");
                    int.TryParse(Console.ReadLine(), out orderId);
                    if (orderId == int.MinValue)
                        orderId = oldOrderItem.OrderId;

                    Console.WriteLine(@"
        Enter the amountto update");
                    int.TryParse(Console.ReadLine(), out amount);
                    if (amount == int.MinValue)
                        amount = oldOrderItem.Amount;

                    Console.WriteLine(@"
        Enter the price to update");
                    double.TryParse(Console.ReadLine(), out price);
                    if (price == double.MinValue)
                        price = oldOrderItem.Price;

                    OrderItem updateOrderItem = new OrderItem()
                    {
                        Id = id,
                        ProductId = productId,
                        OrderId = orderId,
                        Amount = amount,
                        Price = price
                    };
                    //<Receives an updated order item and updates it>
                    dal.OrderItem.Update(updateOrderItem);
                    Console.WriteLine("The item was updated successfuly");
                    break;
                case SubMenu_OrderItem.Delete:
                    Console.WriteLine(@"
        Enter the Order item ID number to remove");
                    int.TryParse(Console.ReadLine(), out orderId);
                    //<Receives an ID number of an item in the order and cancels it>
                    dal.OrderItem.Delete(orderId);
                    break;
                case SubMenu_OrderItem.itemByOrderAndProduct:
                    Console.WriteLine(@"
        Enter the Order ID:");
                    int.TryParse(Console.ReadLine(), out orderId);
                    Console.WriteLine(@"
        Enter the product ID:");
                    int.TryParse(Console.ReadLine(), out productId);
                    //<Receives the ID number of the order and the product and returns the item details in the order>
                    OrderItem itemToShowByProductAndOrder = dal.OrderItem.Get(orderItem => orderItem?.OrderId == orderId &&
                    orderItem?.ProductId == productId);
                    Console.WriteLine(itemToShowByProductAndOrder);
                    break;
                case SubMenu_OrderItem.itemListByOrder:
                    Console.WriteLine(@"
        Enter the Order ID number that you want to see Item Order that belogs to him");
                    int.TryParse(Console.ReadLine(), out orderId);
                    //<Receives an ID number of the order and returns all the details of the products in this order>
                    IEnumerable<OrderItem?> ordersItemsList = dal.OrderItem.GetList(orderItem => orderItem?.OrderId == orderId);
                    foreach (OrderItem item in ordersItemsList)
                        Console.WriteLine(item);
                    break;
                default:
                    break;
            }
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine(@"
             presss any key to continue");
            Console.ReadKey();

        } while (subMenu_OrderItem != SubMenu_OrderItem.Exit);

    }
    #endregion
}