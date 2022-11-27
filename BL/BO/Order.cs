using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BO
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAdress { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        //public DateTime PaymentDate { get; set; }
        public double TotalPrice { get; set; }
        public List<OrderItem> Items { get; set; }
        public Status status { get; set; }
        public override string ToString() => $@"
        Order Id: {Id},
        Name: {CustomerName},
        Email: {CustomerEmail},
        Adress: {CustomerAdress},
        Order date: {OrderDate},
        Ship Date: {ShipDate},
        Delivery Date: {DeliveryDate},
        Status: {status},
        Items: {Items},
        Total Price: {TotalPrice}";

    }
}
