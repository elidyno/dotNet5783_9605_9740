
using DO;
using System.Diagnostics;
using System.Xml.Linq;

namespace Dal;
//A class that links between the product class (DO file) and the Data class (which is linked to collections in Data) through methods
public class DalProduct
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
        int initilizeDataSurce = DataSource._productList.Length;
    }
    /// <summary>
    /// Receives a new product and returns its ID number
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int AddProduct(Product p)
    {
        if (DataSource._productList.Length == DataSource.Config._productIndexer)
            throw new Exception("no place in list to add");
        for (int i = 0; i < DataSource.Config._productIndexer; i++)
        {
            if (p.Id == DataSource._productList[i].Id)
                throw new Exception("The Product Id alredy exist");
        }
        DataSource._productList[DataSource.Config._productIndexer++] = p;

        return p.Id;
    }
    /// <summary>
    /// Receives a product ID number and returns it
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Product GetProduct(int productId)
    {
        bool found = false;
        int i;
        int index = DataSource.Config._productIndexer;
        for (i = 0; i < index; i++)
        {
            if (productId == DataSource._productList[i].Id)
            {
                found = true;
                break;
            }

        }
        if (i == index && !found)
            throw new Exception("the product id not exist in list");
        Product returnProduct = DataSource._productList[i];
        return returnProduct;
    }

    /// <summary>
    /// Returns all products in the store
    /// </summary>
    /// <returns></returns>
    public Product[] GetProductsList()
    {
        int index = DataSource.Config._productIndexer;
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
    public void DelateProduct(int productId)
    {
        int delIndex = int.MinValue;
        for (int i = 0; i < DataSource.Config._productIndexer; i++)
        {
            if (productId == DataSource._productList[i].Id)
            {
                delIndex = i;
                break;
            }
        }
        if (delIndex == int.MinValue)
            throw new Exception("The product not exist in list");

        //move back all items that was after the deleted item
        --DataSource.Config._productIndexer;
        for (int i = delIndex; i < DataSource.Config._productIndexer; i++)
        {
            DataSource._productList[i] = DataSource._productList[i + 1];
        }
    }
    /// <summary>
    /// Receives a product with preferred details and updates it
    /// </summary>
    /// <param name="p"></param>
    /// <exception cref="Exception"></exception>
    public void UpdateProduct(Product p)
    {
        int i;
        for (i = 0; i < DataSource.Config._productIndexer; i++)
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