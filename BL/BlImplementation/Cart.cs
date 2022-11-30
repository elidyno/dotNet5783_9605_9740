using BlApi;
using BO;
using System.ComponentModel.DataAnnotations;

namespace BlImplementation
{
    internal class Cart : ICart
    {
        DalApi.IDal Dal = new Dal.DalList();
        public BO.Cart Add(BO.Cart cart, int productId)
        {
            throw new NotImplementedException();
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
                

                bool exsist = productList.Exists(x => x.Id == item.ProductId);
                if(!exsist)
                    th

                if(item.ProductId != )
                
                totalPrice_+= item.Price;
            }


        }

        public BO.Cart Update(BO.Cart cart, int productId, int newAmount)
        {
            throw new NotImplementedException();
        }
    }
}
