using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dal;


namespace DataSurceInitialize
{
    internal class InitializeOrders
    {
        internal static readonly Random rand = new Random(DateTime.Now.Millisecond);
        internal static List<DO.Order?> orders = new List<DO.Order?>();
        public static List<DO.Order?> GetInitializeOrders = initializeOrders();
        public static RunninId runninId = new RunninId();

        private static List<DO.Order?> initializeOrders()
        {
            int _rand = rand.Next(20, 30);
            for (int i = 0; i < _rand; i++)
            {

                int tmpId = (int)runninId.OrderId;
                int firstNameIndex = rand.Next(0, 10);
                int lastNameIndex = rand.Next(0, 10);

                string tmpName = Initialize.CustomersData.firstNames[firstNameIndex] + " " + Initialize.CustomersData.lastNames[lastNameIndex];
                string tmpEmail = Initialize.CustomersData.firstNames[firstNameIndex] + Initialize.CustomersData.lastNames[lastNameIndex] + "@gmail.com";
                string tmpAdress = Initialize.CustomersData.Adress[lastNameIndex];

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

                DO.Order newOrder = new DO.Order()
                {
                    Id = tmpId,
                    CustomerName = tmpName,
                    CustomerEmail = tmpEmail,
                    CustomerAdress = tmpAdress,
                    OrderDate = tmpOrderDate,
                    ShipDate = tmpShipDate,
                    DeliveryDate = tmpDelivertDate
                };
                orders.Add(newOrder);
            }

            return orders;

        }
    }
}
