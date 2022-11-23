using BlApi;

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
            throw new NotImplementedException();
        }

        public BO.Cart Update(BO.Cart cart, int productId, int newAmount)
        {
            throw new NotImplementedException();
        }
    }
}
