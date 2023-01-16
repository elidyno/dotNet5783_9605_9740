using BO;

namespace BlApi
{
    public interface ICart
    {
        public Cart Add(Cart cart, int productId);
        public Cart Update(Cart cart, int productId, int newAmount);
        public int Approve(Cart cart, string customerName, string customerEmail, string customerAdress);
        public Cart Sub(Cart cart, int productId);
        public Cart Remove(Cart cart, int productId);
        public BO.Cart RemoveAll(BO.Cart cart);
    }
}