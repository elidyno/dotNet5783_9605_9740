using BlApi;

namespace BlImplementation
{
    internal class Product : IProduct
    {
        DalApi.IDal Dal = new Dal.DalList();   
        public void Add(BO.Product product)
        {
            //Validity checks of input format
            if (product.Id <= 0)
                throw new ArgumentException("nid to define exception");
            if (product.Name == null)
                throw new ArgumentException("nid to define exception");
            if (product.Price <= 0)
                throw new ArgumentException("nid to define exception");
            if (product.InStock < 0)
                throw new ArgumentException("nid to define exception");

            //add a Dal Product to DalList
            DO.Product DoProduct = new DO.Product()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                InStock = product.InStock,
                Category = (DO.Category)product.Category
            };
            try
            {
                int i = Dal.Product.Add(DoProduct);
            }
            catch (Exception e)
            {

                throw e;
            }
        
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
