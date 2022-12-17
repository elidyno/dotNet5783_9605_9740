using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;
namespace Dal;

internal sealed class DalList : IDal
{
    public static IDal instance { get; } = new DalList();

    private DalList() { }
    public IOrder Order =>  new DalOrder();

    public IOrderItem OrderItem =>  new DalOrderItem();

    public IProduct Product =>  new DalProduct();
}
