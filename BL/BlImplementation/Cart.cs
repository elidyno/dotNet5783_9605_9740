using BlApi;
using BO;
using System.ComponentModel.DataAnnotations;

namespace BlImplementation
{
    internal class Cart : ICart
    {
        public BO.Cart Add(BO.Cart cart, int productId)
        {
            throw new NotImplementedException();
        }

        public void Approve(BO.Cart cart, string CustomerName, string CustomerEmail, string CustomerAdress)
        {
            //check validation of Customer data parameters
            if (CustomerName == null)
                throw new InvalidValueException("Name of customer can't be empthy");
            if (CustomerAdress == null)
                throw new InvalidValueException("adress of customer can't be empthy");
            if (CustomerEmail == null)
                throw new InvalidValueException("Email of customer can't be empthy");
            if (!new EmailAddressAttribute().IsValid(CustomerEmail))
                throw new InvalidEmailFormatException();



        }

        public BO.Cart Update(BO.Cart cart, int productId, int newAmount)
        {
            throw new NotImplementedException();
        }
    }
}
