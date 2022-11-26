using BlApi;
using Dal;
using DalApi;
using System.Collections.Generic;

namespace BlImplementation
{
    internal class Product : BlApi.IProduct
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
            if (!IsExist(product.Id))
                throw new ArgumentException("The Product not exist");
            if (IsBelongToItemOrder(product.Id))
                throw new ArgumentException("The product exist in Item Order List");
            Dal.Product.Delete(product.Id);
        }

        public BO.Product Get(int productId)
        {
            if (productId <= 0)
                throw new ArgumentException("id <= 0");
            DO.Product dalProduct = new DO.Product();
            try
            {
                dalProduct = Dal.Product.Get(productId);    
            }
            catch (Exception)
            {

                throw;
            }
            BO.Product result = new BO.Product()
            {
                Id = dalProduct.Id,
                Name = dalProduct.Name,
                Price = dalProduct.Price,
                Category = (BO.Category)dalProduct.Category,
                InStock = dalProduct.InStock,
            };
            return result;
        }

        public BO.ProductItem Get(int productId, Cart cart)
        {
            if (productId <= 0)
                throw new ArgumentException("id <= 0");
            DO.Product dalProduct = new DO.Product();
            try
            {
                dalProduct = Dal.Product.Get(productId);
            }
            catch (Exception)
            {

                throw;
            }
            bool inStock_ = dalProduct.InStock > 0;
            BO.ProductItem productItem = new BO.ProductItem()
            {
                Id = dalProduct.Id,
                Name = dalProduct.Name,
                Price = dalProduct.Price,
                Category = (BO.Category)dalProduct.Category,
                InStock = inStock_,
                Amount = dalProduct.InStock
            };
            return productItem;
        }

        public IEnumerable<BO.ProductForList> GetList()
        {
            try
            {
                IEnumerable <DO.Product> dalProducts = Dal.Product.GetList();
                List<BO.ProductForList> result = new List<BO.ProductForList>();

                foreach (DO.Product product in dalProducts)
                {
                    result.Add(new BO.ProductForList()
                    { 
                        Id = product.Id,
                        Name = product.Name,
                        Category = (BO.Category)product.Category,
                        Price = product.Price
                    });
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Update(BO.Product product)
        {
            if(product.Id <= 0)
                throw new Exception("The Product not exist");
            DO.Product item = new DO.Product()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Category = (DO.Category)product.Category,
                InStock = product.InStock
            };
            try
            {
                Dal.Product.Update(item);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool IsBelongToItemOrder(int productId)
        {
            bool exist = false;

            exist = Dal.OrderItem.GetList().Any(x => x.ProductId == productId);
            return exist;
        }
        public bool IsExist(int productId)
        {
            bool exist = false;
            exist = Dal.Product.GetList().Any(x => x.Id == productId);
            return exist;
        }
    }
}
