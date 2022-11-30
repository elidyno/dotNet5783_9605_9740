﻿using BlApi;
using BO;
using DO;
using System.ComponentModel.DataAnnotations;

namespace BlImplementation
{
    internal class Cart : ICart
    {
        private DalApi.IDal Dal = new Dal.DalList(); //Using it we can access the data access classes

        public BO.Cart Add(BO.Cart cart, int productId)
        {
            DO.Product dataProduct = new DO.Product();
            try
            {
                dataProduct = Dal.Product.Get(productId);
            }
            catch (Exception e)
            {
                throw new BO.DataRequestFailedException(e.Message);
            }
            //If the product does not exist in the cart, then add a new product
            if (!cart.Items.Exists(x => x.ProductId == productId))
            {
                if (dataProduct.InStock > 0)
                {
                    BO.OrderItem orderItem = new BO.OrderItem()
                    {
                        ProductId = productId,
                        ProductName = dataProduct.Name,
                        Amount = 1,
                        Id = 0,
                        Price = dataProduct.Price,
                        TotalPrice = dataProduct.Price
                    };
                    cart.Items.Add(orderItem);
                    cart.TotalPrice += dataProduct.Price;

                }

            }
            //If the product is in the cart, update the amount and price
            else if (dataProduct.InStock > 0)
            {
                int i = cart.Items.FindIndex(x => x.ProductId == productId);
                cart.Items[i].Amount += 1;
                cart.Items[i].TotalPrice += cart.Items[i].Price;
                cart.TotalPrice += cart.Items[i].Price;
            }

            return cart;
        }
        public void Approve(BO.Cart cart, string customerName, string customerEmail, string customerAdress)
        {
            //check validation of Customer data parameters
            if (customerName == null)
                throw new InvalidValueException("Name of customer can't be empthy");
            if (customerAdress == null)
                throw new InvalidValueException("adress of customer can't be empthy");
            if (customerEmail == null)
                throw new InvalidValueException("Email of customer can't be empthy");
            if (!new EmailAddressAttribute().IsValid(customerEmail))
                throw new InvalidEmailFormatException();
            //check validation of cart parameter
            if (cart.CustomerEmail != customerEmail)
                throw new InvalidValueException("Email adress in cart not equal to EmailAdresss parameter");
            if (cart.CustomerAdress != customerAdress)
                throw new InvalidValueException("Adress in cart not equal to Adresss parameter");
            if (cart.CustomerName != customerName)
                throw new InvalidValueException("Customer name in cart not equal to Customer name parameter");
            double totalPrice_ = 0;
            DO.Product product_ = new();

            //chack validation of each ItemOrder in cart
            foreach (var item in cart.Items)
            {
                try
                {
                    product_ = Dal.Product.Get(item.ProductId);
                }
                catch (Exception e)
                { 
                    throw new DataRequestFailedException($"ERROR in {item.ProductName}:", e);
                }
                if (item.Amount <= 0)
                    throw new InvalidValueException(item.ProductName + " must be greater than zero");
                if (product_.InStock < item.Amount)
                    throw new AmountAndPriceException($"The product {item.ProductName} (ID:) {item.ProductId} is out of stock");
                if (item.Price != product_.Price)
                    throw new AmountAndPriceException($"price in cart of {item.ProductName} not match to price in Data Surce");
                if (item.TotalPrice != (item.Amount * product_.Price))
                    throw new AmountAndPriceException($"Total price of {item.ProductId} not match to Price and Amount in Cart");
                totalPrice_ += item.TotalPrice;
            }
            if (totalPrice_ != cart.TotalPrice)
                throw new AmountAndPriceException("Total price in cart not match to prices and Amont of all item in cart");
            
            //creat a new  dal order
            DO.Order order = new()
            {
                CustomerName = customerName,
                CustomerAdress = customerAdress,
                CustomerEmail = customerEmail,
                OrderDate = DateTime.Now,
                ShipDate = null,
                DeliveryDate = null
            };
            try
            {
                //try to add order to data sirce in Dal
                int orderId = Dal.Order.Add(order);
                //create orderItem in Dal and update amount of product
                DO.OrderItem orderItem = new();
                foreach (var item in cart.Items)
                {
                    //create an orderItem for dall and add it
                    orderItem.OrderId = orderId;
                    orderItem.ProductId = item.ProductId;
                    orderItem.Amount = item.Amount;
                    orderItem.Price = item.Price;
                    int orderItemId = Dal.OrderItem.Add(orderItem);
                    //update amount of product in Dak
                    product_ = Dal.Product.Get(item.ProductId);
                    product_.InStock -= item.Amount;
                    Dal.Product.Update(product_);
                }
            }
            catch (Exception e)
            {

                throw new DataRequestFailedException(e.Message);
            }
        }


        public BO.Cart Update(BO.Cart cart, int productId, int newAmount)
        {
            int i = cart.Items.FindIndex(x => x.ProductId == productId);
            if(i < 0)
            {
                throw new BO.DataRequestFailedException("knkn"); //?
            }
            //Adding items from an existing product
            else if (cart.Items[i].Amount < newAmount)
            {
                int additionalItems = newAmount - cart.Items[i].Amount;
                cart.Items[i].Amount = newAmount;
                cart.Items[i].TotalPrice += cart.Items[i].Price * additionalItems;
                cart.TotalPrice += cart.Items[i].Price  * additionalItems;
            }
            //Deleting a product from the cart
            else if (newAmount == 0)
            {
                cart.TotalPrice -= cart.Items[i].TotalPrice;
                cart.Items.RemoveAt(i);
            }
            //Reducing items from an existing product
            else
            {
                int reducingItems = cart.Items[i].Amount - newAmount;
                cart.Items[i].Amount = newAmount;
                cart.Items[i].TotalPrice -= cart.Items[i].Price * reducingItems;
                cart.TotalPrice -= cart.Items[i].Price * reducingItems;
            }
            return cart;
        }
    }
}
