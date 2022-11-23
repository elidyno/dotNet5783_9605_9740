using BlApi;


namespace BlImplementation
{
    sealed public class Bl : IBl
    {
        public IProduct Product => new Product();

        public IOrder Order => throw new NotImplementedException();

        public ICart Cart => throw new NotImplementedException();
    }
}
