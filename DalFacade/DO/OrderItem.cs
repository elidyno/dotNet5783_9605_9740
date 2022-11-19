namespace DO;
// <A class that represents fields of item order details>

public struct OrderItem
{

    public int Id { get; set; }
    public int ProductId { get; set; }
    public int OrderId { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }

    public override string ToString() => $@"
    Id: {Id},
    Product Id: {ProductId},
    Order Id: {OrderId},
    Price: {Price},
    Amount: {Amount}
";

}

