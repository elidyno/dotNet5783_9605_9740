using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public double TotalPrice { get; set; }
        public override string ToString() => $@"
        Order item Id: {Id}
        Product Name: {ProductName}
        Product Id: {ProductId}
        Price: {Price}
        Amount: {Amount}
        Total Price: {TotalPrice}";
    }
}

