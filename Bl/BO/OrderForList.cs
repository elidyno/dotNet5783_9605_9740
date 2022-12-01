using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class OrderForList
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int AmountOfItems { get; set; }
        public double TotalPrice { get; set; }
        public Status status { get; set; }
        public override string ToString() => $@"
        Order Id: {Id},
        Name: {CustomerName},
        Amount of items: {AmountOfItems},
        Total Price: {TotalPrice},
        Status: {status}";

    }
}