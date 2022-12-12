
using DalApi;
using DO;
using System.Security.Cryptography.X509Certificates;

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
            if (p.Id == DataSource._productList[i]?.Id)// לבדוק האם צריך לעשות את הבדיקה הזו בשכבת הנתונים
                throw new Exception("The Product Id alredy exist");
        }
        DataSource._productList.Add(p);

        return p.Id;
    }
    /// <summary>
    /// Returns a requested product according to the conditions
    /// </summary>
    /// <param name="select_"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public Product Get(Func<Product?, bool>? select_)
    {
        return DataSource._productList.Find(x => select_(x)) ??
             throw new NotFoundException("The requested product does not exist");
    }
    ///// <summary>
    ///// Receives a product ID number and returns it
    ///// </summary>
    ///// <param name="productId"></param>
    ///// <returns></returns>
    ///// <exception cref="Exception"></exception>
    //public Product Get(int productId)
    //{
    //    return DataSource._productList.Find(x => x?.Id == productId) ??
    //     throw new NotFoundException("Product Id not exist");
    //}

    /// <summary>
    /// Returns all products in the store
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Product?> GetList(Func<Product?, bool>? select_ = null)
    {
        List<Product?> products = new List<Product?>();
        if (select_ == null)
            products = DataSource._productList.ToList<Product?>();
        else
            products = DataSource._productList.FindAll(x => select_(x));
        return products;
           
    }
    /// <summary>
    /// Receives a product ID number and deletes the product
    /// </summary>
    /// <param name="productId"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(int productId)
    {
        int delIndex = DataSource._productList.FindIndex(x => x?.Id == productId);
        if (delIndex == -1)
            throw new NotFoundException("Product Id not exist");
        else
            DataSource._productList.RemoveAt(delIndex);
    }
    /// <summary>
    /// Receives a product with preferred details and updates it
    /// </summary>
    /// <param name="p"></param>
    /// <exception cref="Exception"></exception>
    public void Update(Product p)
    {
        int updateIndex = DataSource._productList.FindIndex(x => x?.Id == p.Id);
        if (updateIndex != -1)
            DataSource._productList[updateIndex] = p;
        else
            throw new NotFoundException("Product Id not exist");
    }
  

}