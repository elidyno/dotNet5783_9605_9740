using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataSurceInitialize
{
    internal class InitializeOrderItem
    {
        public static RunninId runninId = new RunninId();
        internal static readonly Random rand = new Random(DateTime.Now.Millisecond);
        internal static List<DO.OrderItem?> orderItems = new List<DO.OrderItem?>();
        public static List<DO.OrderItem?> GetInitializeOrderItems = initializeOrderItems();


        private static List<DO.OrderItem?> initializeOrderItems()
        {
            //all items belonging to a specific order 
            // to be sure that we have at lest 40 item ordered
            int[] itemsForOneOrder = new int[Initialize._orderList.Count]; 

            int _rand;              // will be commented soon
                                    //int _randProduct;
            int tmpId;
            int tmpOrderId;
            int tmpProductId;
            double tmpPrice;
            int tmpOmunt;

            //initialize arry that kepp the amount of itemf for heach order
            for (int i = 0; i < itemsForOneOrder.Length; i++)
            {
                itemsForOneOrder[i] = 0;
            }

            #region old code
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
            #endregion
            //for heach order create 1-4 order items
            var query = from order in Initialize._orderList
                        from i in Enumerable.Range(0, rand.Next(1, 5))
                        let _randProduct = rand.Next(1, Initialize._productList.Count) - 1
                        let product = (Product?)Initialize._productList[_randProduct]
                        let orderIndex = Initialize._orderList.IndexOf(order)//make a sgine how many items for heach order
                        let temp = ++itemsForOneOrder[orderIndex]
                        select new OrderItem()
                        {
                            Id = runninId.OrderItemId,
                            OrderId = order?.Id ?? 0,
                            ProductId = product?.Id ?? 0,
                            Price = product?.Price ?? 0,
                            Amount = rand.Next(1, 3)
                        };

            query.ToList().ForEach(oi => orderItems.Add(oi));

            #region old code
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
            #endregion
            //if have lass from 40 items, add items to order that have only 1 items
            if (orderItems.Count < 40)
            {
                Initialize._orderList
                .Where((order) => itemsForOneOrder[Initialize._orderList.IndexOf(order)] < 2)
                .TakeWhile(order => Initialize._orderItemList.Count < 40)
                .Select(order =>
                {
                    int _randProduct = rand.Next(1, Initialize._productList.Count) - 1;
                    var product = (Product?)Initialize._productList[_randProduct];
                    return new OrderItem()
                    {
                        Id = runninId.OrderItemId,
                        OrderId = order?.Id ?? 0,
                        ProductId = product?.Id ?? 0,
                        Price = product?.Price ?? 0,
                        Amount = rand.Next(1, 3)
                    };
                })
                .ToList()
                .ForEach(oi => orderItems.Add(oi));
            }

            return orderItems;
        }


    } 
}
