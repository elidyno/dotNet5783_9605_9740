using DO;

namespace BO
{
    public class Order
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerAdress { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        //public DateTime PaymentDate { get; set; }
        public double TotalPrice { get; set; }
        public List<OrderItem?>? Items { get; set; }
        public Status? status { get; set; }
        public override string ToString()
        {
            var query = from item in Items
                        select item.ToString();

            var concatenatedString = string.Join("\n", query);
         
                
            return $"      Order Id: {Id}\n" +
            $"      Name: {CustomerName}\n" +
            $"      Email: {CustomerEmail}\n" +
            $"      Adress: {CustomerAdress}\n" +
            $"      Order date: {OrderDate}\n" +
            $"      Ship Date: {ShipDate}\n" +
            $"      Delivery Date: {DeliveryDate}\n" +
            $"      Status: {status}\n" +
            $"      Items: {concatenatedString}\n" +
            $"      Total Price: {TotalPrice}\n";
        }
    }
}