using BlApi;
using BO;
using Dal;
using DalApi;
using DO;
using System.Collections.Generic;

namespace BlImplementation
{
    /// <summary>
    /// Icraud method of product and some other method. implementaion of IProduct
    /// </summary>
    internal class Product : BlApi.IProduct
    {
        DalApi.IDal Dal = new Dal.DalList();
        /// <summary>
        /// add a Bl product => chack logical valid of data and add to Dal data surce
        /// </summary>
        /// <param name="product"></param>
        /// <exception cref="BO.InvalidValueException"></exception>
        /// <exception cref="DataRequestFailedException"></exception>
        public void Add(BO.Product product)
        {
            //Validity checks of input format
            if (product.Id <= 0)
                throw new BO.InvalidValueException("Id must be greater than zero");
            if (product.Id < 100000 || product.Id >= 1000000)
                throw new BO.InvalidValueException("Id must be a 6 digit number");
            if (product.Name == null)
                throw new BO.InvalidValueException("Name can't be empthy be greater than zero");
            if (product.Price <= 0)
                throw new BO.InvalidValueException("Price must be greater than zero");
            if (product.InStock < 0)
                throw new BO.InvalidValueException("InStock Value must be greater than zero");

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

                throw new DataRequestFailedException(e.Message);
            }
        }

        /// <summary>
        /// delete a Bl product => chack logical valid of data and delete from Dal data surce
        /// </summary>
        /// <param name="productI"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Delete(int productId)
        {
            //Validity checks of input format
            if (productId <= 0)
                throw new BO.InvalidValueException("Id must be greater than zero");
            if (productId < 100000 || productId >= 1000000)
                throw new BO.InvalidValueException("Id must be a 6 digit number");
            //chack if product id exsist in orderItem List
            if (IsHasBeenOrderd(productId))
                throw new CantBeDeletedException("The product exist in Item Order List");
            try
            {
                Dal.Product.Delete(productId);
            }
            catch (Exception e)
            {

                throw new DataRequestFailedException(e.Message);
            }
        }

        /// <summary>
        /// get an product from dak and create a lojicial product and return it
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>BO.Product</returns>
        /// <exception cref="InvalidValueException"></exception>
        /// <exception cref="DataRequestFailedException"></exception>
        public BO.Product Get(int productId)
        {
            //Validity checks of input format
            if (productId <= 0)
                throw new BO.InvalidValueException("Id must be greater than zero");
            if (productId < 100000 || productId >= 1000000)
                throw new BO.InvalidValueException("Id must be a 6 digit number");
            DO.Product dalProduct = new DO.Product();
            try
            {
                dalProduct = Dal.Product.Get(product => product?.Id == productId);
            }
            catch (Exception e)
            {

                throw new DataRequestFailedException(e.Message);
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

        /// <summary>
        /// get a product from Dal and create a logicial ProductItem to show un Costomer cart
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="cart"></param>
        /// <returns>BO.ProductItem</returns>
        /// <exception cref="InvalidValueException"></exception>
        /// <exception cref="DataRequestFailedException"></exception>
        public BO.ProductItem Get(int productId, BO.Cart cart)
        {
            //Validity checks of input format
            if (productId <= 0)
                throw new BO.InvalidValueException("Id must be greater than zero");
            if (productId < 100000 || productId >= 1000000)
                throw new BO.InvalidValueException("Id must be a 6 digit number");
            DO.Product dalProduct = new DO.Product();
            try
            {
                dalProduct = Dal.Product.Get(product => product?.Id == productId);
            }
            catch (Exception e)
            {

                throw new DataRequestFailedException(e.Message);
            }
            //calculate the logicial data and return a logicial product for cart screan (customer)
            bool inStock_ = dalProduct.InStock > 0;
            int amount_ = 0;
            foreach (BO.OrderItem item in cart.Items)
            {
                if (item.Id == productId)
                    amount_++;
            }
            BO.ProductItem productItem = new BO.ProductItem()
            {
                Id = dalProduct.Id,
                Name = dalProduct.Name,
                Price = dalProduct.Price,
                Category = (BO.Category)dalProduct.Category,
                InStock = inStock_,
                Amount = amount_
            };
            return productItem;
        }

        /// <summary>
        /// get list of product from Dal and create a logicial productForList
        /// </summary>
        /// <returns>BO.ProductItem</returns>
        /// <exception cref="DataRequestFailedException"></exception>
        public IEnumerable<BO.ProductForList> GetList(Func<ProductForList, bool>? select_ = null)
        {
            try
            {
                IEnumerable<DO.Product?> dalProducts = Dal.Product.GetList();
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
                if (select_ != null)
                    result.RemoveAll(X => !select_(X));

                return result;
            }
            catch (Exception e)
            {

                throw new DataRequestFailedException(e.Message);
            }
        }

        /// <summary>
        /// get as a parameter a logicial Product,
        /// chack validation of data and send a DO product to update in Dal
        /// </summary>
        /// <param name="product"></param>
        /// <exception cref="BO.InvalidValueException"></exception>
        /// <exception cref="DataRequestFailedException"></exception>
        public void Update(BO.Product product)
        {
            //Validity checks of input format
            if (product.Id <= 0)
                throw new BO.InvalidValueException("Id must be greater than zero");
            if (product.Id < 100000 || product.Id >= 1000000)
                throw new BO.InvalidValueException("Id must be a 6 digit number");
            if (product.Name == null)
                throw new BO.InvalidValueException("Name can't be empthy be greater than zero");
            if (product.Price <= 0)
                throw new BO.InvalidValueException("Price must be greater than zero");
            if (product.InStock < 0)
                throw new BO.InvalidValueException("InStock Value must be greater than zero");

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
            catch (Exception e)
            {

                throw new DataRequestFailedException(e.Message);
            }

        }

        /// <summary>
        /// chack if Has the product been ordered?
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>false or true</returns>
        public bool IsHasBeenOrderd(int productId)
        {
            return Dal.OrderItem.GetList().Any(x => x?.ProductId == productId);
                
        }
    }
}