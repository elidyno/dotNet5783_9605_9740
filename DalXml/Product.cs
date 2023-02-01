using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal
{
    /// <summary>
    /// implementation of craut method in dal list configration for product
    /// </summary>
    internal class Product : IProduct
    {
        const string s_products = "products";

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int Add(DO.Product product)
        {
            XElement productRootElement = XMLTools.LoadListFromXMLElement(s_products);

            XElement? product_ = (from p in productRootElement.Elements()
                                where Convert.ToInt32(p.Element("Id").Value) == product.Id
                                select p).FirstOrDefault();
            if (product_ != null)
                throw new DO.AlreadyExistsException();
            XElement EXProduct = new XElement("Product",
                                     new XElement("Id", product.Id),
                                     new XElement("Name", product.Name),
                                     new XElement("Category", product.Category),
                                     new XElement("Price", product.Price),
                                     new XElement("InStock", product.InStock)

                                );
            productRootElement.Add(EXProduct);
            XMLTools.SaveListToXMLElement(productRootElement, s_products);

            return product.Id;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Delete(int productId)
        {
            XElement productRootElement = XMLTools.LoadListFromXMLElement(s_products);

            XElement? product_ = (from p in productRootElement.Elements()
                                  where Convert.ToInt32(p.Element("Id").Value) == productId
                                  select p).FirstOrDefault() ?? throw new DO.NotFoundException();
            product_.Remove();
            XMLTools.SaveListToXMLElement(productRootElement, s_products);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DO.Product Get(Func<DO.Product?, bool>? select_)
        {
            XElement productRootElement = XMLTools.LoadListFromXMLElement(s_products);
            return (DO.Product)(from p in productRootElement.Elements()
                    let doProduct = CreateProductFromXElement(p)
                    where select_(doProduct)
                    select doProduct).FirstOrDefault();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DO.Product?> GetList(Func<DO.Product?, bool>? select_ = null)
        {
            XElement productRootElement = XMLTools.LoadListFromXMLElement(s_products);

            if (select_ != null)
            {
                return from p in productRootElement.Elements()
                       let doProduct = CreateProductFromXElement(p)
                       where select_(doProduct)
                       select (DO.Product?)doProduct;
            }

            else
            {
                return (IEnumerable<DO.Product?>)(from p in productRootElement.Elements()
                       select CreateProductFromXElement(p));
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Update(DO.Product product)
        {
            XElement productRootElement = XMLTools.LoadListFromXMLElement(s_products);

            Delete(product.Id);
            Add(product);

        }

        [MethodImpl(MethodImplOptions.Synchronized)]      //needless?
        static DO.Product? CreateProductFromXElement(XElement p)
        {
            return new DO.Product()
            {
                Id = Convert.ToInt32(p.Element("Id").Value),
                Name = (string?)p.Element("Name"),
                Category = ConcvetXElementTOCategory(p.Element("Category")),
                InStock = Convert.ToInt32(p.Element("InStock").Value),
                Price = Convert.ToDouble(p.Element("Price").Value)
            };
        }

        [MethodImpl(MethodImplOptions.Synchronized)]          //neddless?
        static DO.Category? ConcvetXElementTOCategory(XElement XE_Category)
        {
            string? s_category = XE_Category.Value.ToString();
            bool success = Enum.TryParse<DO.Category>(s_category, out var category);
            if (!success)
                throw new DO.NullException();
            return category;
        }
    }
}
