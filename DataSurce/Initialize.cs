using DalApi;
using DO;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Linq;


namespace Dal
{
    /// <summary>
    /// 1. A class that creates objects of the entity type (in the DalFasade folder) in Data source arrays
    /// 2. You initialize them with the add method and use additional data
    /// 3. Links between all the DalProduct... and the entities created in it
    /// </summary>
    internal static class Initialize
    {
       
        #region Data surce arrays
        internal static List<Product?> _productList = new List<Product?>();
        internal static List<Order?> _orderList = new List<Order?>();
        internal static List<OrderItem?> _orderItemList = new List<OrderItem?>();
        #endregion
        #region constractor
        

        static Initialize()
        {
            DataSource.addFirstProducts();
            initializeOrders();
            DataSource.addFirstItemOrders();
        }
        #endregion
        #region Config class
        /// <summary>
        /// indexeres for availble place in arries
        /// </summary>

        
        #region add method
        /// <summary>
        /// Initializes the object of the Product class that it created
        /// </summary>
       
        /// <summary>
        /// Initializes the object of the order class it created
        /// </summary>
        
        /// <summary>
        /// Initializes the object of the created itemorder class
        /// </summary>
        private static void addFirstItemOrders()
        {
            //all items belonging to a specific order 
            // to be sure that we have at lest 40 item ordered
            int[] itemsForOneOrder = new int[_orderList.Count];

            int _rand;              // will be commented soon
            //int _randProduct;
            int tmpId;
            int tmpOrderId;
            int tmpProductId;
            double tmpPrice;
            int tmpOmunt;
            //for (int i = 0; i < _orderList.Count; i++)
            //{
            //    _rand = rand.Next(1, 5);
            //    if (_rand == 4)
            //        itemsForOneOrder[i] = int.MaxValue; // flag that this order has alredy four items
            //    for (int j = 0; j < _rand; j++)
            //    {
            //        _randProduct = rand.Next(1, _productList.Count) - 1;
            //        tmpId = Config.OrderItemRunningId;
            //        tmpOrderId = ((Order)_orderList[i]).Id; 
            //        tmpProductId = ((Product)_productList[_randProduct]).Id;
            //        tmpPrice = ((Product)_productList[_randProduct]).Price;
            //        tmpOmunt = rand.Next(1, 3); // piople not buying mor of 3 same Shosse per 
            //        OrderItem newOrderItem = new OrderItem()
            //        {
            //            Id = tmpId,
            //            OrderId = tmpOrderId,
            //            ProductId = tmpProductId,
            //            Price = tmpPrice,
            //            Amount = tmpOmunt
            //        };
            //        _orderItemList.Add(newOrderItem);

            //    }
            //}
            //----------------------
            var query = from order in _orderList
                        where rand.Next(1, 5) != 4
                        from i in Enumerable.Range(0, rand.Next(1, 5))
                        let _randProduct = rand.Next(1, _productList.Count) - 1
                        let product = (Product?)_productList[_randProduct]
                        select new OrderItem()
                        {
                            Id = Config.OrderItemRunningId,
                            OrderId = order?.Id ?? 0,
                            ProductId = product?.Id ?? 0,
                            Price = product?.Price ?? 0,
                            Amount = rand.Next(1, 3)
                        };

            query.ToList().ForEach(oi => _orderItemList.Add(oi));

            //--------------------------------
            //if sum of item order lass from 40, we add items to order that have only one order

            //if (_orderItemList.Count < 40)
            //{
            //    for (int i = 0; i <_orderList.Count && _orderItemList.Count < 40; i++)
            //    {
            //        if (itemsForOneOrder[i] < int.MaxValue)
            //        {
            //            _randProduct = rand.Next(1, _productList.Count) - 1;
            //            tmpId = Config.OrderItemRunningId;
            //            tmpOrderId = ((Order)_orderList[i]).Id;
            //            tmpProductId = ((Product)_productList[_randProduct]).Id;
            //            tmpPrice = ((Product)_productList[_randProduct]).Price;
            //            tmpOmunt = rand.Next(1, 3); // piople not buying mor of 3 same Shosse per 
            //            OrderItem newOrderItem = new OrderItem()
            //            {
            //                Id = tmpId,
            //                OrderId = tmpOrderId,
            //                ProductId = tmpProductId,
            //                Price = tmpPrice,
            //                Amount = tmpOmunt
            //            };
            //            _orderItemList.Add(newOrderItem);

            //        }

            //    }
            //}
            //------------------------------------------------
            if (_orderItemList.Count < 40)
            {
                _orderList
                .Where((o, i) => itemsForOneOrder[i] < int.MaxValue)
                .TakeWhile(o => _orderItemList.Count < 40)
                .Select(o =>
                {
                    int _randProduct = rand.Next(1, _productList.Count) - 1;
                    var product = (Product?)_productList[_randProduct];
                    return new OrderItem()
                    {
                        Id = Config.OrderItemRunningId,
                        OrderId = o?.Id ?? 0,
                        ProductId = product?.Id ?? 0,
                        Price = product?.Price ?? 0,
                        Amount = rand.Next(1, 3)
                    };
                })
                .ToList()
                .ForEach(oi => _orderItemList.Add(oi));
            }
            //--------------------------------------------
        }
        #endregion
        #region Additional data

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

        #endregion

    }
}

