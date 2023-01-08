using BO;
namespace BlApi
{
    public interface IProduct
    {
        public void Add(Product product);
        public void Update(Product product);
        public void Delete(int productId);
        public Product Get(int productId);
        public ProductItem Get(int productId, Cart cart);
        public IEnumerable<ProductForList?> GetList(Func<ProductForList, bool>? select_ = null);
        public IEnumerable<ProductItem> GetItemList(BO.Cart cart, Func<ProductItem, bool>? select_ = null);
    }
}

