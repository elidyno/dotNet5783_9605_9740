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
        Console.WriteLine("Welcome to the 'Jerusalem Shoes' shoe Store");

        do
        {
            Console.WriteLine("Please select one of the following options to test\n"
               + "  1) Product operation\n"
               + "  2) Order operation\n "
               + "  3) Order Item operation\n"       
               + "  0) To exit from menu");


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
            DateTime? orderCreate = null;
            DateTime? orderShip = null;
            DateTime? delivery = null;

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
                    DateTime.value.TryParse(Console.ReadLine(), out orderCreate);
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