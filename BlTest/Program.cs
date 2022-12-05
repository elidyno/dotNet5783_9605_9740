using BlApi;
using Dal;
using DalApi;
using BlImplementation;
using BO;
using System.Numerics;
using Microsoft.VisualBasic;
using System.Diagnostics.Metrics;


namespace BlTest
{
    internal class Program
    {
        /// <summary>
        /// enum for main menu
        /// </summary>
        enum MainMenuCode { Exit, Product, Order, Cart }
        // enum for sub menue: product operation
        enum SubMenu_Product
        {
            ExitSubMenu = 0,
            AddProduct,
            DelProduct,
            UpdateProduct,
            ViewProduct,
            ViewProductToCart,
            ViewList
        }
        // enum for sub menue: order operation
        enum SubMenu_Order
        {
            ExitSubMenu = 0,
            View,
            ViewAll,
            GetTracking,
            UpdateOrderDelivery,
            UpdateOrderSheep
        }
        // enum for sub menue: cart operation
        enum SubMenu_Cart
        {
            ExitSubMenu = 0,
            AddItemToCart,
            UpdateCart,
            ApproveCart
        }
        private static MainMenuCode menuCode;
        private static SubMenu_Order subMenu_Order;
        private static SubMenu_Product subMenu_Product;
        private static SubMenu_Cart subMenu_cart;
        static BO.Cart cart = new Cart();
        private static IBl bl = new BlImplementation.Bl();   

        /// <summary>
        /// main program for test BL
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            bool success = false;
            bool exit = false;
            do
            {
                Console.WriteLine("\nPlease select one of the following options to test:\n" +
                "  1) Product operation\n" +
                "  2) Order operation\n" +
                "  3) Cart operation\n" +
                "  0) To exit from menu\n");
                success = MainMenuCode.TryParse(Console.ReadLine(), out menuCode);
                if (success)
                {
                    switch (menuCode)
                    {
                        case MainMenuCode.Cart:
                            CartMenu();
                            break;
                        case MainMenuCode.Order:
                            OrderMenu();
                            break;
                        case MainMenuCode.Product:
                            ProductMenu();
                            break;
                        case MainMenuCode.Exit:
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid number:\n Please enter one of the numbers shown in the menu\n");
                            break;
                    }
                }
                else
                    Console.WriteLine("Please entry only a intiger Number\n");
                Console.WriteLine("press any key to continue...");
                Console.ReadKey();
                Console.Clear();

            } while (!exit);

        }
        /// <summary>
        /// chack operation of order in BlImplementation
        /// </summary>
        static void OrderMenu()
        {
            bool success = false;
            bool exit = false;
            int id;
            do
            {
                Console.WriteLine(
                      "Select operation to test:\n" +
                      "  1) To show an order\n" +
                      "  2) To Show all order list\n" +
                      "  3) To get order tracking\n" +
                      "  4) To update order delivery\n" +
                      "  5) To update order sheep\n" +
                      "  0) To exit from sub menu\n"
                       );
                success = SubMenu_Order.TryParse(Console.ReadLine(), out subMenu_Order);
                if (success)
                {
                    switch (subMenu_Order)
                    {
                        case SubMenu_Order.ExitSubMenu:
                            exit = true;
                            break;
                        case SubMenu_Order.View:
                            Console.WriteLine("Please enter order id");
                            success = int.TryParse(Console.ReadLine(), out id);
                            if (success)
                            {
                                BO.Order order = new BO.Order();
                                try
                                {
                                    order = bl.Order.Get(id);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                    break;
                                }
                                Console.WriteLine(order);
                            }
                            else
                                Console.WriteLine("id must be an intinuger number\n"); 
                            break;
                        case SubMenu_Order.ViewAll:
                            IEnumerable<BO.OrderForList> ordersForList = new List<BO.OrderForList>();
                            try
                            {
                                ordersForList = bl.Order.GetList();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                break;
                            }
                            foreach (BO.OrderForList order in ordersForList)
                                Console.WriteLine(order);
                            break;
                        case SubMenu_Order.GetTracking:
                            Console.WriteLine("Please enter order id");
                            success = int.TryParse(Console.ReadLine(), out id);
                            if (success)
                            {
                                BO.OrderTracking orderTracking = new BO.OrderTracking();
                                try
                                {
                                    orderTracking = bl.Order.GetTracking(id);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                    break;
                                }
                                Console.WriteLine(orderTracking);
                            }
                            else
                                Console.WriteLine("id must be an intinuger number\n");
                            break;
                        case SubMenu_Order.UpdateOrderDelivery:
                            Console.WriteLine("Please enter order id");
                            success = int.TryParse(Console.ReadLine(), out id);
                            if (success)
                            {
                                BO.Order order = new BO.Order();
                                try
                                {
                                    order = bl.Order.UpdateOrderDelivery(id);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                    break;
                                }
                                Console.WriteLine(order);
                            }
                            else
                                Console.WriteLine("id must be an intinuger number\n");
                            break;
                        case SubMenu_Order.UpdateOrderSheep:
                            Console.WriteLine("Please enter order id");
                            success = int.TryParse(Console.ReadLine(), out id);
                            if (success)
                            {
                                BO.Order order = new BO.Order();
                                try
                                {
                                    order = bl.Order.UpdateOrderSheep(id);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                    break;
                                }
                                Console.WriteLine(order);
                            }
                            else
                                Console.WriteLine("id must be an intinuger number\n");
                            break;
                        default:
                            Console.WriteLine("Invalid value, please try again\n");
                            break;
                    }                
                }
                else
                    Console.WriteLine("Invalid value, please try again\n");
                Console.WriteLine("press any key to continue...");
                Console.ReadKey();
                Console.Clear();

            } while (!exit);
        
        }
        /// <summary>
        /// chack operation of cart in BlImplementation
        /// </summary>
        /// <exception cref="InvalidInputFormatException"></exception>
        static void CartMenu()
        {
            string? name, adress, email;
            int productId, amount;
            //Creates a cart entity with input from the user
            Console.WriteLine("Please enter the customer's name");
            name = Console.ReadLine();
            Console.WriteLine("Please enter the customer's Email");
            email = Console.ReadLine();
            Console.WriteLine("Please enter the customer's adress");
            adress = Console.ReadLine();
            cart.CustomerName = name;
            cart.CustomerAdress = adress;
            cart.CustomerEmail = email;
            cart.Items = new List<OrderItem>();
            cart.TotalPrice = 0;

            bool success = false;
            bool exit = false;
            do
            {
                Console.WriteLine(
                      "Select operation to test:\n" +
                      "  1) To add item to cart\n" +
                      "  2) To update the cart\n" +
                      "  3) To approve the cart of customer\n" +
                      "  0) To exit from sub menu\n"
                       );
                try
                {
                    success = SubMenu_Cart.TryParse(Console.ReadLine(), out subMenu_cart);
                    if (success)
                    {
                        switch (subMenu_cart)
                        {
                            case SubMenu_Cart.ApproveCart:
                                Console.WriteLine("Please enter the customer's name");
                                name = Console.ReadLine();
                                Console.WriteLine("Please enter the customer's email");
                                email = Console.ReadLine();
                                Console.WriteLine("Please enter the customer's adress");
                                adress = Console.ReadLine();
                                bl.Cart.Approve(cart, name, email, adress);
                                Console.WriteLine("The ordr has been orderd succsesufy");
                                break;
                            case SubMenu_Cart.AddItemToCart:
                                Console.WriteLine("Please enter the product id");
                                success = int.TryParse(Console.ReadLine(), out productId);
                                if (success)
                                {
                                    cart = bl.Cart.Add(cart, productId);
                                    Console.WriteLine(cart);
                                }
                                else
                                    throw new InvalidInputFormatException("id must be an intinuger number");  
                                break;
                            case SubMenu_Cart.UpdateCart:
                                Console.WriteLine("Please enter the product id");
                                success = int.TryParse(Console.ReadLine(), out productId);
                                if (!success)
                                    throw new InvalidInputFormatException("id must be an intinuger number");
                                Console.WriteLine("Please enter the new amount");
                                success = int.TryParse(Console.ReadLine(), out amount);
                                if (!success)
                                    throw new InvalidInputFormatException("amount must be an intinuger number");
                                cart = bl.Cart.Update(cart, productId, amount);
                                Console.WriteLine(cart);
                                break;
                            case SubMenu_Cart.ExitSubMenu:
                                exit = true;
                                break;
                            default: throw new InvalidInputFormatException("Invalid number:\nPlease enter one of the numbers shown in the menu \n");
                        }
                    }
                    else
                        throw new InvalidInputFormatException("Please entry only a intiger Number\n");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
               
            } while (!exit);
        }
        /// <summary>
        /// chack operation of Product in BlImplementation
        /// </summary>
        static void ProductMenu()
        {
            //all variable need for all operation
            bool success = false;
            bool exit = false;
            int id;
            string name;
            double price;
            BO.Category category;
            int amount;
            BO.Product product = new();
            IEnumerable<BO.ProductForList> productsList = new List<BO.ProductForList>();
            do
            {
                Console.WriteLine(
                      "Select operation to test:\n" +
                      "  1) To add a Product\n" +
                      "  2) To del a Product\n" +
                      "  3) To update a Product\n" +
                      "  4) To show a product - Admin screan\n" +
                      "  5) To show a product - Cart screan\n" +
                      "  6) To show list of all product\n" +
                      "  0) To exit from sub menu\n"
                       );
                try
                {
                    success = SubMenu_Product.TryParse(Console.ReadLine(), out subMenu_Product);
                    if (success)
                    {
                        switch (subMenu_Product)
                        {
                            case SubMenu_Product.ExitSubMenu:
                                exit = true;
                                break;
                            case SubMenu_Product.AddProduct:
                                //requst data of new product to add
                                Console.WriteLine("Enter Id number of new Product");
                                success = int.TryParse(Console.ReadLine(), out id);
                                if (!success)
                                    throw new InvalidInputFormatException("Please entry only a intiger Number\n");
                                Console.WriteLine("Enter the customer name:");
                                name = Console.ReadLine();
                                Console.WriteLine("Enter the price:");
                                success = double.TryParse(Console.ReadLine(), out price);
                                if (!success)
                                    throw new InvalidInputFormatException("Please entry only a double Number\n");
                                Console.WriteLine("Enter category:");
                                success = Category.TryParse(Console.ReadLine(), out category);
                                if (!success)
                                    throw new InvalidInputFormatException("Please entry only a Category name\n");
                                Console.WriteLine("Enter the amount in stock:");
                                success = int.TryParse(Console.ReadLine(), out amount);
                                if (!success)
                                    throw new InvalidInputFormatException("Please entry only a intiger Number\n");

                                //creat a new product and try to add it to database
                                product.Id = id;
                                product.Name = name;
                                product.Price = price;
                                product.Category = category;
                                product.InStock = amount;
                                bl.Product.Add(product);
                                break;
                            case SubMenu_Product.DelProduct:
                                Console.WriteLine("Enter Product Id:");
                                success = int.TryParse(Console.ReadLine(), out id);
                                if (!success)
                                    throw new InvalidInputFormatException("id must be an intinuger number");
                                bl.Product.Delete(id);

                                //get list of product to show the product is deleted from data surce
                                productsList = bl.Product.GetList();
                                foreach (var product_ in productsList)
                                {
                                    Console.WriteLine(product_);
                                }
                                break;
                            case SubMenu_Product.UpdateProduct:
                                Console.WriteLine("Please enter Product id");
                                success = int.TryParse(Console.ReadLine(), out id);
                                if (!success)
                                    throw new InvalidInputFormatException("id must be an intinuger number");

                                //get the original item to keep the old value of failde that user wan't to update
                                BO.Product oldProduct = bl.Product.Get(id);

                                Console.WriteLine("enter new data to updated in Product (only in failde yo want to update):");

                                name = null; //to check after if the user put a value for update
                                Console.WriteLine("Name:");
                                name = Console.ReadLine();
                                if (name == null || name == "\n")
                                    name = oldProduct.Name;

                                price = double.MinValue; //to check after if the user put a value for update
                                Console.WriteLine("Price:");
                                success = double.TryParse(Console.ReadLine(), out price);
                                if (!success)
                                    price = oldProduct.Price;

                                //if not entried a new value, keep the old
                                Console.WriteLine("Category:");
                                success = Category.TryParse(Console.ReadLine(), out category);
                                if (!success)
                                    category = oldProduct.Category;

                                Console.WriteLine("InStock:");
                                success = int.TryParse(Console.ReadLine(), out amount);
                                if (!success)
                                    amount = oldProduct.InStock;

                                BO.Product upProduct = new Product()
                                {
                                    Id = id,
                                    Name = name,
                                    Price = price,
                                    Category = category,
                                    InStock = amount
                                };
                                bl.Product.Update(upProduct);

                                //get list of product to show the product was updated
                                Console.WriteLine("The product was updated:");
                                Console.WriteLine(product);
                                break;
                            case SubMenu_Product.ViewProduct:
                                Console.WriteLine("Please enter Product id");
                                success = int.TryParse(Console.ReadLine(), out id);
                                if (!success)
                                    throw new InvalidInputFormatException("Please entry only a intiger Number\n");
                                product = bl.Product.Get(id);
                                Console.WriteLine(product);
                                break;
                            case SubMenu_Product.ViewProductToCart:
                                Console.WriteLine("Please enter product id");
                                success = int.TryParse(Console.ReadLine(), out id);
                                if (!success)
                                    throw new InvalidInputFormatException("Please entry only a intiger Number\n");
       
                                BO.ProductItem item = new();
                                //cart is a static var of program class, the user suposte to select this case only after adedd data to the cart
                                //else, the logic lyer will throw an exception 
                                item = bl.Product.Get(id, cart);
                                Console.WriteLine("Product Item in cart:");
                                Console.WriteLine(item);
                                break;
                            case SubMenu_Product.ViewList:
                                IEnumerable<BO.ProductForList> allProduct = bl.Product.GetList();
                                foreach (var p in allProduct)
                                {
                                    Console.WriteLine(p);
                                }
                                break;
                            default:
                                throw new InvalidInputFormatException("Invalid number:\n Please enter one of the numbers shown in the menu \n");
                                break;

                        }
                    }
                    else
                        throw new InvalidInputFormatException("Please entry only a intiger Number\n");

                }
                catch (Exception e)
                {

                    Console.WriteLine(e);
                }
                Console.WriteLine("press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            } while (!exit);
        }

    }
}


