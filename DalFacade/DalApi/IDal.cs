using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace DalApi;

/// <summary>
///  Instances of Croud method for Dal object
/// </summary>
public interface IDal
{
    public IOrder Order { get;}
    public IOrderItem OrderItem { get;}
    public IProduct Product { get;}

}
