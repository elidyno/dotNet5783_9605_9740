﻿using BlApi;
using BO;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BlImplementation
{
    /// <summary>
    /// Icraud method of product and some other method. implementaion of IProduct
    /// </summary>
    internal class Product : BlApi.IProduct
    {
        DalApi.IDal? dal = DalApi.Factory.Get();



        /// <summary>
        /// add a Bl product => chack logical valid of data and add to Dal data surce
        /// </summary>
        /// <param name="product"></param>
        /// <exception cref="BO.InvalidValueException"></exception>
        /// <exception cref="DataRequestFailedException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
                int i = dal?.Product.Add(DoProduct) ?? throw new NullableException();
            }
            catch (Exception e)
            {

                throw new DataRequestFailedException(e.Message);
            }
        }

        /// <summary>
        /// delete a Bl product => chack logical valid of data and delete from dal data surce
        /// </summary>
        /// <param name="productI"></param>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
                dal?.Product.Delete(productId);
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
        [MethodImpl(MethodImplOptions.Synchronized)]
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
                dalProduct = dal?.Product.Get(product => product?.Id == productId) ?? throw new NullableException(); 
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
        [MethodImpl(MethodImplOptions.Synchronized)]
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
                dalProduct = dal?.Product.Get(product => product?.Id == productId) ?? throw new NullableException();
            }
            catch (Exception e)
            {

                throw new DataRequestFailedException(e.Message);
            }
            //calculate the logicial data and return a logicial product for cart screan (customer)
            bool inStock_ = dalProduct.InStock > 0;
            int amount_ = 0;
            if(cart.Items != null)
            {
                //var item = cart.Items.FirstOrDefault(i => i?.Id == productId);
                //amount_ = item != null ? item.Amount : 0;
                foreach (BO.OrderItem item in cart.Items)
                {
                  if (item?.ProductId == productId)
                        amount_ = item.Amount;
                }
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
        /// get list of product from Dal and create a logicial list of productForList
        /// </summary>
        /// <returns> IEnumerable<BO.ProductForList> </returns>
        /// <exception cref="DataRequestFailedException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BO.ProductForList> GetList(Func<ProductForList, bool>? select_ = null)
        {
            try
            {
                IEnumerable<DO.Product?> dalProducts = dal.Product.GetList();
                List<BO.ProductForList> result = new List<BO.ProductForList>();

                var resultItems = dalProducts.Select(product => new BO.ProductForList
                {
                    Id = product?.Id ?? 0,
                    Name = product?.Name,
                    Category = (BO.Category)(product?.Category ?? 0),
                    Price = product?.Price ?? 0
                }).Where(p => p.Id != 0).ToList();

                result = resultItems;
                //-----------
                //foreach (DO.Product product in dalProducts)
                //{
                //    result.Add(new BO.ProductForList()
                //    {
                //        Id = product.Id,
                //        Name = product.Name,
                //        Category = (BO.Category)product.Category,
                //        Price = product.Price
                //    });
                //}
                //-------------
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
        /// get list of product from Dal and create a logicial list of productItem
        /// </summary>
        /// <param name="select_"></param>
        /// <returns> IEnumerable<ProductItem> </returns>
        /// <exception cref="DataRequestFailedException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ProductItem?> GetItemList(BO.Cart cart, Func<ProductItem, bool>? select_ = null)
        {
            try
            {
                IEnumerable<DO.Product?> dalProducts = dal!.Product.GetList();
                List<BO.ProductItem?> result = new List<BO.ProductItem?>();
                
                //var resultItems = dalProducts.Select(product => Get(product?.Id ?? 0, cart));
                // Use ToList() to convert the results to a list and assign it to the result variable
                
                foreach (DO.Product product in dalProducts)
                {
                    result.Add(Get(product.Id, cart));
                }
                if (select_ != null)
                    result.RemoveAll(x => !select_(x));

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
        [MethodImpl(MethodImplOptions.Synchronized)]
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
                dal?.Product.Update(item);
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsHasBeenOrderd(int productId)
        {
            return dal?.OrderItem.GetList().Any(x => x?.ProductId == productId) ??  throw new NullableException(); 
                
        }

        //public void AddAmountInProductItem(BO.ProductItem? productItem)
        //{
        //    if(productItem != null)
        //        productItem!.Amount++;
        //    else
        //        throw new NullableException();
        //}
    }
}