namespace DO;
///A class that represents order details fields 

public struct Order
{
    public int Id { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerAdress { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? ShipDate { get; set; }
    public DateTime? DeliveryDate { get; set; }

    public override string ToString() => $@"
    Order Id: {Id},
    Customer Name: {CustomerName}
    Customer Email: {CustomerEmail}
    Customer Adress: {CustomerAdress}
    Order Date: {OrderDate},
    Ship Date: {ShipDate},
    Delivery Date: {DeliveryDate}
";

}

