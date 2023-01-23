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
            var query = from order in Initialize._orderList
                        where rand.Next(1, 5) != 4
                        from i in Enumerable.Range(0, rand.Next(1, 5))
                        let _randProduct = rand.Next(1, Initialize._productList.Count) - 1
                        let product = (Product?)Initialize._productList[_randProduct]
                        select new OrderItem()
                        {
                            Id = runninId.OrderItemId,
                            OrderId = order?.Id ?? 0,
                            ProductId = product?.Id ?? 0,
                            Price = product?.Price ?? 0,
                            Amount = rand.Next(1, 3)
                        };

            query.ToList().ForEach(oi => orderItems.Add(oi));

            #region capseled
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
            if (orderItems.Count < 40)
            {
                Initialize._orderList
                .Where((o, i) => itemsForOneOrder[i] < int.MaxValue)
                .TakeWhile(o => Initialize._orderItemList.Count < 40)
                .Select(o =>
                {
                    int _randProduct = rand.Next(1, Initialize._productList.Count) - 1;
                    var product = (Product?)Initialize._productList[_randProduct];
                    return new OrderItem()
                    {
                        Id = runninId.OrderItemId,
                        OrderId = o?.Id ?? 0,
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
