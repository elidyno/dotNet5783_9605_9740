using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataSurceInitialize
{
    public class RunninId
    {
        private int orderId;
        private int orderItemId;

        const string configIdPath = @"..\xml\config.xml";
        XElement ConfigRoot = null;

        public int OrderId
        {
            get
            {
                ConfigRoot = XElement.Load(configIdPath);
                if (ConfigRoot != null)
                {
                    orderId = Convert.ToInt32(ConfigRoot.Element("orderRunningId").Value);
                    ConfigRoot.Element("orderRunningId").SetValue(++orderId);
                    ConfigRoot.Save(configIdPath);
                    return orderId;
                }

                else
                {
                    throw new ArgumentNullException("cant get an Order id");
                }

            }

            set { orderId = value; }
        }

        public int OrderItemId
        {
            get
            {
                ConfigRoot = XElement.Load(configIdPath);
                if (ConfigRoot != null)
                {
                    orderItemId = Convert.ToInt32(ConfigRoot.Element("orderItemRunningId").Value);
                    ConfigRoot.Element("orderItemRunningId").SetValue(++orderItemId);
                    ConfigRoot.Save(configIdPath);
                    return orderItemId;
                }

                else
                {
                    throw new ArgumentNullException("cant get an OrderItem id");
                }

            }
        }
    }
}