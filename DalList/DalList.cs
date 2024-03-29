﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;
namespace Dal;

/// <summary>
/// statment of implementation of insance of craud method in dal list configration
/// </summary>
internal sealed class DalList : IDal
{
    
    public static IDal Instance { get; } = new DalList();

    //static DalList() { }
    private DalList() { }
    public IOrder Order =>  new DalOrder();

    public IOrderItem OrderItem =>  new DalOrderItem();

    public IProduct Product =>  new DalProduct();
}
