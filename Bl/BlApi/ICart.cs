using BO;

namespace BlApi
{
    public interface ICart
    {
        public Cart Add(Cart cart, int productId);
        public Cart Update(Cart cart, int productId, int newAmount);
        public void Approve(Cart cart, string customerName, string customerEmail, string customerAdress);
    }
}