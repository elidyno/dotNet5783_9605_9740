﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Cart
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAdress { get; set; }
        public List<OrderItem> Items { get; set; }
        public double TotalPrice { get; set; }
        public override string ToString()
        {
            string tmp = "";
            foreach (var item in Items)
                tmp += item.ToString();
            string toString;
            toString = ($@"
        Name: {CustomerName},
        Email: {CustomerEmail},
        Adress: {CustomerAdress}");
            toString += tmp + ($@"
        TotalPrice: {TotalPrice}");
            return toString;
        }
    }
}