using BlApi;

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
            catch(Exception e)
            {
                throw new BO.DataRequestFailedException(e.Message);
            }
            //If the product does not exist in the cart, then add a new product
            if (!cart.Items.Exists(x => x.ProductId == productId))
            {
                if(dataProduct.InStock > 0)
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
