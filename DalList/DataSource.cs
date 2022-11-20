﻿

using DO;
namespace Dal
{
    /// <summary>
    /// 1. A class that creates objects of the entity type (in the DalFasade folder) in Data source arrays
    /// 2. You initialize them with the add method and use additional data
    /// 3. Links between all the DalProduct... and the entities created in it
    /// </summary>
    internal static class DataSource
    {
        internal static readonly Random rand = new Random(DateTime.Now.Millisecond);
        #region Data surce arrays
        internal static List<Product> _productList = new List<Product>();
        internal static List<Order> _orderList = new List<Order>();
        internal static List<OrderItem> _orderItemList = new List<OrderItem>();
        #endregion
        #region constractor
        /// <summary>
        /// constractor call s_initialize() to start the data with utems alredy in the arries
        /// </summary>
        static DataSource()
        {
            s_initialize();
        }
        #endregion

        #region s_initialize

        private static void s_initialize()
        {
            DataSource.addFirstProducts();
            DataSource.addFirstOrders();
            DataSource.addFirstItemOrders();
        }
        #endregion
        #region Config class
        /// <summary>
        /// indexeres for availble place in arries
        /// </summary>
        internal static class Config
        {
          
            /// <summary>
            /// Running id number for order and order item
            /// </summary>
            private static int _orderRunningId = 100;
            private static int _orderItemRunningId = 100;

            /// <summary>
            /// properties to get the running numbers and advance the number by 1
            /// </summary>
            public static int OrderRunningId { get => _orderRunningId++; }
            public static int OrderItemRunningId { get => _orderItemRunningId++; }
        }
        #endregion
        #region add method
        /// <summary>
        /// Initializes the object of the Product class that it created
        /// </summary>
        private static void addFirstProducts()
        {
            int _rand = rand.Next(10, 16);
            for (int i = 0; i < _rand; i++)
            {
                // thake a difren tick of second to each item and make sure that had 6 dgits
                int tmpId = ((byte)DateTime.Now.Ticks) + 100000;
                int tmpValCategory = rand.Next(0, 3);

                int tmpIndexinDataList = rand.Next(0, 3);
                string tmpName = _productNamesList[tmpValCategory][tmpIndexinDataList];
                double tmpPrice = _productPriceList[tmpValCategory][tmpIndexinDataList];
                ShoesCategories tmpCategory = (ShoesCategories)tmpValCategory;
                int tmpOmunt = 0;

                if (i < 0.95 * _rand - 1)
                {
                    tmpOmunt = rand.Next(1, 15);
                }
                Product newProduct = new Product
                {
                    Id = tmpId,
                    Name = tmpName,
                    Category = tmpCategory,
                    Price = tmpPrice,
                    InStock = tmpOmunt
                };

                _productList.Add(newProduct);
            }

        }
        /// <summary>
        /// Initializes the object of the order class it created
        /// </summary>
        private static void addFirstOrders()
        {
            int _rand = rand.Next(20, 30);
            for (int i = 0; i < _rand; i++)
            {
                int tmpId = Config.OrderRunningId;
                int firstNameIndex = rand.Next(0, 9);
                int lastNameIndex = rand.Next(0, 9);

                string tmpName = CustomersData.firstNames[firstNameIndex] + " " + CustomersData.lastNames[lastNameIndex];
                string tmpEmail = CustomersData.firstNames[firstNameIndex] + CustomersData.lastNames[lastNameIndex] + "@gmail.com";
                string tmpAdress = CustomersData.Adress[lastNameIndex];

                DateTime tmpOrderDate = DateTime.MinValue;
                DateTime tmpShipDate = DateTime.MinValue;
                DateTime tmpDelivertDate = DateTime.MinValue;

                if (i <= 0.8 * _rand - 1)
                {
                    if (i <= 0.6 * _rand - 1)
                    {
                        tmpDelivertDate = DateTime.Now.AddDays(-((rand.NextDouble() + 0.1) * 5));
                        tmpShipDate = tmpDelivertDate.AddDays(-((rand.NextDouble() + 0.01) * 5));
                        tmpOrderDate = tmpShipDate.AddDays(-((rand.NextDouble() + 0.01) * 2));

                    }
                    else
                    {
                        tmpShipDate = DateTime.Now.AddDays(-((rand.NextDouble() + 0.1) * 5));
                        tmpOrderDate = tmpShipDate.AddDays(-((rand.NextDouble() + 0.01) * 2));
                    }
                }
                else
                    tmpOrderDate = DateTime.Now.AddDays(-((rand.NextDouble() + 0.1) * 5));

                Order newOrder = new Order()
                {
                    Id = tmpId,
                    CustomerName = tmpName,
                    CustomerEmail = tmpEmail,
                    CustomerAdress = tmpAdress,
                    OrderDate = tmpOrderDate,
                    ShipDate = tmpShipDate,
                    DeliveryDate = tmpDelivertDate
                };
                _orderList.Add(newOrder);
            }
            
        }
        /// <summary>
        /// Initializes the object of the created itemorder class
        /// </summary>
        private static void addFirstItemOrders()
        {
            //all items belonging to a specific order 
            // to be sure that we have at lest 40 item ordered
            int[] itemsForOneOrder = new int[_orderList.Count];

            int _rand;              // will be commented soon
            int _randProduct;
            int tmpId;
            int tmpOrderId;
            int tmpProductId;
            double tmpPrice;
            int tmpOmunt;
            for (int i = 0; i < _orderList.Count; i++)
            {
                _rand = rand.Next(1, 4);
                if (_rand == 4)
                    itemsForOneOrder[i] = int.MaxValue; // flag that this order has alredy four items
                for (int j = 0; j < _rand; j++)
                {
                    _randProduct = rand.Next(1, _productList.Count) - 1;
                    tmpId = Config.OrderItemRunningId;
                    tmpOrderId = _orderList[i].Id;
                    tmpProductId = _productList[_randProduct].Id;
                    tmpPrice = _productList[_randProduct].Price;
                    tmpOmunt = rand.Next(1, 3); // piople not buying mor of 3 same Shosse per 
                    OrderItem newOrderItem = new OrderItem()
                    {
                        Id = tmpId,
                        OrderId = tmpOrderId,
                        ProductId = tmpProductId,
                        Price = tmpPrice,
                        Amount = tmpOmunt
                    };
                    _orderItemList.Add(newOrderItem);

                }
            }

            //if sum of item order lass from 40, we add items to order that have only one order
            if (_orderItemList.Count < 40)
            {
                for (int i = 0; i <_orderList.Count && _orderItemList.Count < 40; i++)
                {
                    if (itemsForOneOrder[i] < int.MaxValue)
                    {
                        _randProduct = rand.Next(1, _productList.Count) - 1;
                        tmpId = Config.OrderItemRunningId;
                        tmpOrderId = _orderList[i].Id;
                        tmpProductId = _productList[_randProduct].Id;
                        tmpPrice = _productList[_randProduct].Price;
                        tmpOmunt = rand.Next(1, 3); // piople not buying mor of 3 same Shosse per 
                        OrderItem newOrderItem = new OrderItem()
                        {
                            Id = tmpId,
                            OrderId = tmpOrderId,
                            ProductId = tmpProductId,
                            Price = tmpPrice,
                            Amount = tmpOmunt
                        };
                        _orderItemList.Add(newOrderItem);

                    }

                }
            }
        }
        #endregion
        #region Additional data

        public static string[][] _productNamesList = new string[4][]
        {
        new string[4] {"Elegant shoes (black, men)", "Elegant shoes (brown, men)", "Sport shoes (blue, men)","Work shoes (gray, men)"},
        new string[4] {"High heels shoes (red, women)", "Women's boots (black, women)", "Flip flops (white, men)", "Sport shoes (pink, women)" },
        new string[4] {"Elegant shoes (black, boys)", "Moccasins (brown, boys)", "Sport shoes (blue, boys)","futball shoes (blue, boys)" },
        new string[4] {"Girl's event shoes (red, girls)", "Girl's boots (pink, girl)", "Flip flops (white, girl)","Sport shoes (pink, girl)"}
        };
        public static double[][] _productPriceList = new double[4][]
        {
        new double[4] {399, 399, 299,499},
        new double[4] {399, 499, 299,299},
        new double[4] {199, 199, 149,99},
        new double[4] {199, 199, 149,99}
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



//using DO;
//namespace Dal;

//internal static  class DataSurce
//{
//    internal static readonly Random rand = new Random(DateTime.Now.Millisecond);

//    #region Config class
//    internal static class Config
//    {
//        /// <summary>
//        /// indexeres for availble place in arries
//        /// </summary>
//        internal static int _productIndexer = 0;
//        internal static int _orderIndexer = 0;
//        internal static int _orderItemIndexer = 0;
//        /// <summary>
//        /// Running id number for order and order item
//        /// </summary>
//        private static int _orderRunningId = 100;
//        private static int _orderItemRunningId = 100;

//        /// <summary>
//        /// properties to get the running numbers and advance the number by 1
//        /// </summary>
//        public static int OrderRunningId { get => ++_orderRunningId; }
//        public static int OrderItemRunningId { get => ++_orderItemRunningId; }

//    }
//    #endregion
//    #region Data surce arries

//    internal static Product[] _productList = new Product[50];
//    internal static Order[] _orderList = new Order[100];
//    internal static OrderItem[] _orderItemList = new OrderItem[200];
//    #endregion
//    #region s_initialize


//    private static void s_initialize()
//    {
//        DataSurce.addFirstProducts();
//        DataSurce.addFirstOrders();
//        DataSurce.addFirstItemOrders();
//    }
//    #endregion

//    /// <summary>
//    /// constractor call s_initialize() to start the data with utems alredy in the arries
//    /// </summary>
//    static DataSurce()
//    {
//        DataSurce.s_initialize();
//    }
//    #region add method
//    private static void addFirstProducts()
//    {
//        int _rand = rand.Next(10, 16);
//        for (int i = 0; i < _rand; i++)
//        {
//            int tmpId = (byte)DateTime.Now.Ticks;
//            int tmpValCategory = rand.Next(0, 3);
//            int tmpIndexinDataList = rand.Next(0, 3);
//            string tmpName = _productNamesList[tmpValCategory][tmpIndexinDataList];
//            double tmpPrice = _productPriceList[tmpValCategory][tmpIndexinDataList];
//            ShoesCategories tmpCategory = (ShoesCategories)tmpValCategory;
//            int tmpOmunt = 0;
//            if (i < 0.95 * _rand - 1)
//                tmpOmunt = rand.Next(1, 15);
//            _productList[i] = new Product()
//            { Id = tmpId, Name = tmpName, Category = tmpCategory, Price = tmpPrice, InStock = tmpOmunt };
//        }
//        Config._productIndexer = _rand;

//    }

//    private static void addFirstOrders()
//    {
//        int _rand = rand.Next(20, 30);
//        for (int i = 0; i < _rand; i++)
//        {
//            int tmpId = Config.OrderRunningId;
//            int firstNameIndex = rand.Next(0, 9);
//            int lastNameIndex = rand.Next(0, 9);
//            string tmpName = CustomersData.firstNames[firstNameIndex] + " " + CustomersData.lastNames[lastNameIndex];
//            string tmpEmail = CustomersData.firstNames[firstNameIndex] + CustomersData.lastNames[lastNameIndex] + "@gmail.com";
//            string tmpAdress = CustomersData.Adress[lastNameIndex];
//            DateTime tmpOrderDate = DateTime.MinValue;
//            DateTime tmpShipDate = DateTime.MinValue; ;
//            DateTime tmpDelivertDate = DateTime.MinValue;
//            TimeSpan timeSpan = TimeSpan.Zero;
//            if (i <= 0.8 * _rand - 1)
//            {
//                if (i <= 0.6 * _rand - 1)
//                {
//                    tmpDelivertDate = DateTime.Now.AddDays(-((rand.NextDouble() + 0.1) * 5));
//                    tmpShipDate = tmpDelivertDate.AddDays(-((rand.NextDouble() + 0.01) * 5));
//                    tmpOrderDate = tmpShipDate.AddDays(-((rand.NextDouble() + 0.01) * 2));

//                }
//                else
//                {
//                    tmpShipDate = DateTime.Now.AddDays(-((rand.NextDouble() + 0.1) * 5));
//                    tmpOrderDate = tmpShipDate.AddDays(-((rand.NextDouble() + 0.01) * 2));
//                }
//            }
//            else
//                tmpOrderDate = DateTime.Now.AddDays(-((rand.NextDouble() + 0.1) * 5));

//            _orderList[i] = new Order()
//            {
//                Id = tmpId,
//                CustomerName = tmpName,
//                CustomerEmail = tmpEmail,
//                CustomerAdress = tmpAdress,
//                OrderDate = tmpOrderDate,
//                ShipDate = tmpShipDate,
//                DeliveryDate = tmpDelivertDate
//            };
//        }
//        Config._orderIndexer = _rand;
//    }

//    private static void addFirstItemOrders()
//    {
//        int[] itemsForOneOrder = new int[Config._orderIndexer];
//        int _rand;
//        int _randProduct;
//        int tmpId;
//        int tmpOrderId;
//        int tmpProductId;
//        double tmpPrice;
//        int tmpOmunt;
//        for (int i = 0; i < Config._orderIndexer; i++)
//        {
//            _rand = rand.Next(1, 4);
//            if (_rand == 4)
//                itemsForOneOrder[i] = int.MaxValue; // flag that this order has alredy four items
//            for (int j = 0; j < _rand; j++)
//            {
//                _randProduct = rand.Next(1, Config._productIndexer) - 1;
//                tmpId = Config.OrderItemRunningId;
//                tmpOrderId = _orderList[i].Id;
//                tmpProductId = _productList[_randProduct].Id;
//                tmpPrice = _productList[_randProduct].Price;
//                tmpOmunt = rand.Next(1, 3); // piople not buying mor of 3 same Shosse per 
//                _orderItemList[Config._orderItemIndexer++] = new OrderItem()
//                {
//                    Id = tmpId,
//                    OrderId = tmpOrderId,
//                    ProductId = tmpProductId,
//                    Price = tmpPrice,
//                    Amount = tmpOmunt
//                };
//            }
//        }

//        //if sum of item order lass from 40, we add items to order that have only one order
//        if (Config._orderItemIndexer < 40)
//        {
//            for (int i = 0; i < Config._orderIndexer && Config._orderItemIndexer < 40; i++)
//            {
//                if (itemsForOneOrder[i] < int.MaxValue)
//                {
//                    _randProduct = rand.Next(1, Config._productIndexer) - 1;
//                    tmpId = Config.OrderItemRunningId;
//                    tmpOrderId = _orderList[i].Id;
//                    tmpProductId = _productList[_randProduct].Id;
//                    tmpPrice = _productList[_randProduct].Price;
//                    tmpOmunt = rand.Next(1, 3); // piople not buying mor of 3 same Shosse per 
//                    _orderItemList[Config._orderItemIndexer++] = new OrderItem()
//                    {
//                        Id = tmpId,
//                        OrderId = tmpOrderId,
//                        ProductId = tmpProductId,
//                        Price = tmpPrice,
//                        Amount = tmpOmunt
//                    };
//                }

//            }
//        }
//    }

//    #endregion
//    #region Additional data

//    public static string[][] _productNamesList = new string[4][]
//    {
//        new string[4] {"Elegant shoes (black, men)", "Elegant shoes (brown, men)", "Sport shoes (blue, men)","Work shoes (gray, men)"},
//        new string[4] {"High heels shoes (red, women)", "Women's boots (black, women)", "Flip flops (white, men)", "Sport shoes (pink, women)" },
//        new string[4] {"Elegant shoes (black, boys)", "Moccasins (brown, boys)", "Sport shoes (blue, boys)","futball shoes (blue, boys)" },
//        new string[4] {"Girl's event shoes (red, girls)", "Girl's boots (pink, girl)", "Flip flops (white, girl)","Sport shoes (pink, girl)"}
//    };
//    public static double[][] _productPriceList = new double[4][]
//    {
//        new double[4] {399, 399, 299,499},
//        new double[4] {399, 499, 299,299},
//        new double[4] {199, 199, 149,99},
//        new double[4] {199, 199, 149,99}
//    };

//    public static class CustomersData
//    {
//        public static string[] firstNames = new string[] { "Ei", "Levi", "Beni", "Dani", "Shlomi", "Meni", "Israel", "Moshe", "Yossi", "Kobi" };
//        public static string[] lastNames = new string[] { "Levi", "Kohen", "Tamari", "Nahon", "Netanyahu", "Adas", "Lipsker", "Fridman", "Lapid", "Galon" };
//        public static string[] Adress = new string[]
//        {
//            "Tehena 13, Jerusalm", "Yaffo 205, Jerusalm",  "Etrog 20, Jerusalm", "Lulav 5, Jerusalm",  "Tapuach 7, Jerusalm",
//            "Hatzvi 15, Jerusalm",  "Yaffo 15, Jerusalm",  "Etrog 2, Jerusalm",  "Lulav 30, Jerusalm",  "Malchey Israel  14, Jerusalm"
//        };
//    };

//    #endregion

//}
