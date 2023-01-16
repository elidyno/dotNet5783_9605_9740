using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Cart
    {
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerAdress { get; set; }
        public List<OrderItem?>? Items { get; set; }
        public double? TotalPrice { get; set; }
        public override string ToString()
        {
            var query = from item in Items
                        select item.ToString();

            var concatenatedString = string.Join("", query);

            //string toString;

            return $"      Name: {CustomerName}\n" +
            $"      Email: {CustomerEmail}\n" +
            $"      Adress: {CustomerAdress}\n" +
            $"      Items: {concatenatedString}\n" +
            $"      TotalPrice: {TotalPrice}";

        }
    }
}
