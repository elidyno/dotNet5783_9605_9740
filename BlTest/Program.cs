using BlApi;
using Dal;
using DalApi;
using BlImplementation;
using BO;


namespace BlTest
{
    internal class Program
    {
        // enum for main menu
        enum MainMenuCode { Exit, Product, Order, Cart }
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
        private static SubMenu_Order subMenu_Order;
        private static MainMenuCode menuCode;
        private static IBl bl = new BlImplementation.Bl();   //?

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
                            //call cart method
                            break;
                        case MainMenuCode.Order:
                            OrderMenu();
                            break;
                        case MainMenuCode.Product:
                            //call product method
                            break;
                        case MainMenuCode.Exit:
                            exit = true;
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
                            break;
                        default:
                            Console.WriteLine("Invalid value, please try again\n");
                            Console.WriteLine("press any key to continue...");
                            Console.ReadKey();
                            Console.Clear();
                            break;

                    }
                }
                else
                {
                    Console.WriteLine("Invalid value, please try again\n");
                    Console.WriteLine("press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }


            } while (!exit);
        }
    }
}

