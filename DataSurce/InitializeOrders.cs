using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dal.Initialize;

namespace Dal
{
    internal class InitializeOrders
    {
        const string s_orders = "orders";
        internal static readonly Random rand = new Random(DateTime.Now.Millisecond);
        internal static List<DO.Order?> products = new List<DO.Order?>();
        public static IEnumerable<DO.Order?> GetInitializeOrders = initializeOrders();

        private static IEnumerable<DO.Order> initializeOrders()
        {
            int _rand = rand.Next(20, 30);
            for (int i = 0; i < _rand; i++)
            {
                
                int tmpId = Config.OrderRunningId;
                int firstNameIndex = rand.Next(0, 10);
                int lastNameIndex = rand.Next(0, 10);

                string tmpName = CustomersData.firstNames[firstNameIndex] + " " + CustomersData.lastNames[lastNameIndex];
                string tmpEmail = CustomersData.firstNames[firstNameIndex] + CustomersData.lastNames[lastNameIndex] + "@gmail.com";
                string tmpAdress = CustomersData.Adress[lastNameIndex];

                DateTime? tmpOrderDate = null;
                DateTime? tmpShipDate = null;
                DateTime? tmpDelivertDate = null;

                if (i <= 0.8 * _rand - 1)
                {
                    if (i <= 0.6 * _rand - 1)
                    {
                        tmpDelivertDate = DateTime.Now.AddDays(-((rand.NextDouble() + 0.1) * 5));
                        tmpShipDate = tmpDelivertDate.Value.AddDays(-((rand.NextDouble() + 0.01) * 5));
                        tmpOrderDate = tmpShipDate.Value.AddDays(-((rand.NextDouble() + 0.01) * 2));

                    }
                    else
                    {
                        tmpShipDate = DateTime.Now.AddDays(-((rand.NextDouble() + 0.1) * 5));
                        tmpOrderDate = tmpShipDate.Value.AddDays(-((rand.NextDouble() + 0.01) * 2));
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
    }
}
