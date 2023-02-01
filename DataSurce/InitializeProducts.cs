using Dal;
using DataSurceInitialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal
{
    /// <summary>
    /// initialize the data for product
    /// </summary>
    internal class InitializeProducts
    {
        internal static readonly Random rand = new Random(DateTime.Now.Millisecond);
        internal static List<DO.Product?> products = new List<DO.Product?>();
        public static List<DO.Product?> GetInitializeProducts = initializeProducts();
        internal static List<DO.Product?> initializeProducts()
        {
            int _rand = rand.Next(10, 20);
            for (int i = 0; i < _rand; i++)
            {
                // thake a difren tick of second to each item and make sure that had 6 dgits
                int tmpId = ((byte)DateTime.Now.Ticks) + 100000;
                int tmpValCategory = rand.Next(0, 5);

                int tmpIndexInDataList = rand.Next(0, 4);
                string tmpName = Initialize._productNamesList[tmpValCategory][tmpIndexInDataList];
                double tmpPrice = Initialize._productPriceList[tmpValCategory][tmpIndexInDataList];
                DO.Category tmpCategory = (DO.Category)tmpValCategory;
                int tmpOmunt = 0;

                if (i < 0.95 * _rand - 1)
                {
                    tmpOmunt = rand.Next(1, 15);
                }
                DO.Product newProduct = new DO.Product
                {
                    Id = tmpId,
                    Name = tmpName,
                    Category = tmpCategory,
                    Price = tmpPrice,
                    InStock = tmpOmunt
                };

                products.Add(newProduct);
            }

            return products;
        }

       
    }
}
