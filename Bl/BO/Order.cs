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
            string str = "";
            foreach (var item in Items)
            {
                if (item.Id != Items[Items.Count -1].Id)
                    str += item.ToString() + "\n";
                else
                    str += item.ToString();
            }
                
            return $"      Order Id: {Id}\n" +
            $"      Name: {CustomerName}\n" +
            $"      Email: {CustomerEmail}\n" +
            $"      Adress: {CustomerAdress}\n" +
            $"      Order date: {OrderDate}\n" +
            $"      Ship Date: {ShipDate}\n" +
            $"      Delivery Date: {DeliveryDate}\n" +
            $"      Status: {status}\n" +
            $"      Items: {str}\n" +
            $"      Total Price: {TotalPrice}\n";
        }
    }
}