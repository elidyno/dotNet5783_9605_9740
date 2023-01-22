using DalApi;
using DO;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Linq;
using System.Xml.Linq;
using Dal;


namespace DataSurceInitialize
{
    /// <summary>
    /// 1. A class that creates objects of the entity type (in the DalFasade folder) in Data source arrays
    /// 2. You initialize them with the add method and use additional data
    /// 3. Links between all the DalProduct... and the entities created in it
    /// </summary>
    public class Initialize
    {
        const string s_orders = "orders";
        const string s_products = "products";
        const string s_orderItems = "orderItems";


        #region Data surce arrays
        public static List<Product?> _productList = new List<Product?>();
        public static List<Order?> _orderList = new List<Order?>();
        public static List<OrderItem?> _orderItemList = new List<OrderItem?>();
        #endregion
       

        public Initialize()
        {
            _orderList = InitializeOrders.GetInitializeOrders;
            XMLTools.SaveListToXMLSerializer(_orderList, s_orders);
            _productList = InitializeProducts.GetInitializeProducts;
            DataSurceInitialize.XMLTools.SaveListToXMLSerializer(_productList, s_products);
            _orderItemList = InitializeOrderItem.GetInitializeOrderItems;
            DataSurceInitialize.XMLTools.SaveListToXMLSerializer(_orderItemList, s_orderItems);
        }
       

        public static string[][] _productNamesList = new string[5][]
       {
        new string[4] {"Elegant shoes (black, size:45, men)", "Elegant shoes (brown, size:42, men)", "Sport shoes (blue, size:43, men)", "Work shoes (gray, size:41, men)"},
        new string[4] {"High heels shoes (red, size:37, women)", "Women's boots (black, size:39, women)", "Flip flops (white, size:38 men)", "Sport shoes (pink, size:37, women)" },
        new string[4] {"Elegant shoes (black, size:27, boys)", "Moccasins (brown, size:28, boys)", "Sport shoes (blue, size:30, boys)", "futball shoes (blue, size:22, boys)" },
        new string[4] {"Girl's event shoes (red, size:20, girls)", "Girl's boots (pink, size:25, girl)", "Flip flops (white, size:24, girl)", "Sport shoes (pink, size:29, girl)"},
        new string[4] { "Shoe polish (black, mat)", "shoe pad (size:40, ortoped)", "Shoe polish (brown, mat)", "sport socks (white, size:43)"}
       };
        public static double[][] _productPriceList = new double[5][]
        {
        new double[4] {399, 399, 299,499},
        new double[4] {399, 499, 299,299},
        new double[4] {199, 199, 149,99},
        new double[4] {199, 199, 149,99},
        new double[4] {40, 120, 40, 30 }
        };

        public static class CustomersData
        {
            public static string[] firstNames = new string[] { "Ei", "Levi", "Beni", "Dani", "Shlomi", "Meni", "Israel", "Moshe", "Yossi", "Kobi" };
            public static string[] lastNames = new string[] { "Levi", "Kohen", "Tamari", "Nahon", "Netanyahu", "Adas", "Lipsker", "Fridman", "Lapid", "Galon" };
            public static string[] Adress = new string[]
            {
            "Tehena 13, Jerusalem", "Yaffo 205, Jerusalem",  "Etrog 20, Jerusalem", "Lulav 5, Jerusalem",  "Tapuach 7, Jerusalem",
            "Hatzvi 15, Jerusalem",  "Yaffo 15, Jerusalem",  "Etrog 2, Jerusalem",  "Lulav 30, Jerusalem",  "Malchey Israel  14, Jerusalem"
            };
        };

    }
}