
namespace BO;

public class ProductForList
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public Category Category { get; set; }
   
    public override string ToString() => $@"
    Product Id : {Id},
    Name :       {Name},
    Category:    {Category},
    Price:       {Price},
";
}