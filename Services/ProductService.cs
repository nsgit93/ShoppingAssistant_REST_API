using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingAssistantServer.Entities;
using ShoppingAssistantServer.Helpers;

namespace ShoppingAssistantServer.Services
{
    public interface IProductService
    {
        IEnumerable<Products> GetAll();
        IEnumerable<Productstore> GetAllProductsStores();
        IEnumerable<Products> GetProductsByNameSubstring(string nameSubstr);
        IEnumerable<Products> GetProductsByFullName(string name);
        IEnumerable<Products> GetProductsByStoreId(int storeId);
        Productstore GetProductPriceAvail(int product_id, int store_id);
        Products CreateProduct(Products product);
    }
    public class ProductService : IProductService
    {
        private DataContext _context;

        public ProductService(DataContext context)
        {
            Console.WriteLine("Product service DI injection for DataContext -> ", context);
            _context = context;
        }

        public Products CreateProduct(Products product)
        {
            //throw new NotImplementedException();
            if (!product.Description.Equals(""))
            {
                var found = from p in _context.Products
                            where p.Name == product.Name && p.Description == product.Description
                            select p;
                if(found.SingleOrDefault() != null)
                {
                    return found.SingleOrDefault();
                }
                else
                {
                    _context.Add(product);
                    _context.SaveChanges();
                    return (from p in _context.Products
                            orderby p.Id descending
                            select p).First();
                }
            }
            else
            {
                var found = from p in _context.Products
                            where p.Name == product.Name
                            select p;
                if (found.SingleOrDefault() != null)
                {
                    return found.SingleOrDefault();
                }
                else
                {
                    _context.Add(product);
                    _context.SaveChanges();
                    return (from p in _context.Products
                            orderby p.Id descending
                            select p).First();
                }
            }
        }

        public IEnumerable<Products> GetAll()
        {
            return _context.Products.ToList();
        }

        public IEnumerable<Productstore> GetAllProductsStores()
        {
            return _context.Productstore.ToList();
        }

        public Productstore GetProductPriceAvail(int product_id, int store_id)
        {
            return (from ps in _context.Productstore
                    where ps.Id_product == product_id && ps.Id_store == store_id
                    select ps).SingleOrDefault();
        }

        public IEnumerable<Products> GetProductsByFullName(string name)
        {
            return (from p in _context.Products where p.Name == name select p).ToList();
        }

        public IEnumerable<Products> GetProductsByNameSubstring(string nameSubstr)
        {
            return (from p in _context.Products where p.Name.StartsWith(nameSubstr) select p).ToList();
        }

        public IEnumerable<Products> GetProductsByStoreId(int storeId)
        {
            var productstores = (from ps in _context.Productstore
                                 where ps.Id_store == storeId
                                 select ps).ToList();

            List<Products> products = new List<Products>();
            for(int i = 0; i < productstores.Count; i++)
            {
                var found = (from p in _context.Products
                             where p.Id == productstores[i].Id_product
                             select p).SingleOrDefault();
                if(found != null)
                {
                    products.Add(found);
                }
            }
            return products;
        }
    }
}
