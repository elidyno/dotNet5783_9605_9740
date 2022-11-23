using BlApi;

namespace BlImplementation
{
    internal class Product : IProduct
    {
        DalApi.IDal Dal = new Dal.DalList();   
        public void Add(BO.Product product)
        {
            if (product.Id <= 0)
                throw new ArgumentException("nid to define exception");
            
            
            
            throw new NotImplementedException();
        }

        public void Delete(BO.Product product)
        {
            throw new NotImplementedException();
        }

        public BO.Product Get(int productId)
        {
            throw new NotImplementedException();
        }

        public BO.ProductItem Get(int productId, Cart cart)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BO.ProductForList> GetList()
        {
            throw new NotImplementedException();
        }

        public void Update(BO.Product product)
        {
            throw new NotImplementedException();
        }
    }
}
