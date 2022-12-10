namespace DO;
// <A struct that represents fields of product details in the store>

public struct Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }

    public Category? Category { get; set; }
    public int InStock { get; set; }

    public override string ToString() => $@"
    Product Id : {Id},
    Name :       {Name},
    Category:    {Category},
    Price:       {Price},
    Amount in stock: {InStock}
";

}

