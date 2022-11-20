
using DalApi;
using DO;
using System.Diagnostics;
using System.Xml.Linq;

namespace Dal;
//A class that links between the product class (DO file) and the Data class (which is linked to collections in Data) through methods
internal class DalProduct : IProduct
{
    /// <summary>
    /// due to deficult to initilize the data surce
    /// and i not wanted to cencel the Config class
    /// as Dan Zilbershtain suggested
    //i found a solution: asaiment an tem int with the lenth of the arries 
    /// </summary>
    public DalProduct()
    {
        //only for initilize the DataSurce
        int initilizeDataSurce = DataSource._productList.Count;
    }
    /// <summary>
    /// Receives a new product and returns its ID number
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int Add(Product p)
    {
        //if (DataSource._productList.Length == DataSource.Config._productIndexer)      Unnecessary ??
        //    throw new Exception("no place in list to add");
        for (int i = 0; i < DataSource._productList.Count; i++)
        {
            if (p.Id == DataSource._productList[i].Id)
                throw new Exception("The Product Id alredy exist");
        }
        DataSource._productList.Add(p);

        return p.Id;
    }
    /// <summary>
    /// Receives a product ID number and returns it
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Product Get(int productId)
    {
        bool found = false;
        int i;
        for (i = 0; i < DataSource._productList.Count; i++)
        {
            if (productId == DataSource._productList[i].Id)
            {
                found = true;
                break;
            }

        }
        if (i == DataSource._productList.Count && !found)
            throw new Exception("the product id not exist in list");
        Product returnProduct = DataSource._productList[i];
        return returnProduct;
    }

    /// <summary>
    /// Returns all products in the store
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Product> GetList()
    {
        int index = DataSource._productList.Count;
        Product[] products = new Product[index];
        for (int i = 0; i < index; i++)
        {
            products[i] = DataSource._productList[i];
        }

        return products;
    }
    /// <summary>
    /// Receives a product ID number and deletes the product
    /// </summary>
    /// <param name="productId"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(int productId)
    {
        int delIndex = int.MinValue;
        for (int i = 0; i < DataSource._productList.Count; i++)
        {
            if (productId == DataSource._productList[i].Id)
            {
                delIndex = i;
                break;
            }
        }
        if (delIndex == int.MinValue)
            throw new Exception("The product not exist in list");
       
        DataSource._productList.RemoveAt(delIndex);

    }
    /// <summary>
    /// Receives a product with preferred details and updates it
    /// </summary>
    /// <param name="p"></param>
    /// <exception cref="Exception"></exception>
    public void Update(Product p)
    {
        int i;
        for (i = 0; i < DataSource._productList.Count; i++)
        {
            if (p.Id == DataSource._productList[i].Id)
            {
                DataSource._productList[i] = p;
                return;
            }
        }

        throw new Exception("The product not exist in list");
    }

}