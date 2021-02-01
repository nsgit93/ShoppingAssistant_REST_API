using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingAssistantServer.Entities;
using ShoppingAssistantServer.Helpers;
using ShoppingAssistantServer.Models.Filters;
using ShoppingAssistantServer.Models.ShoppingList;

namespace ShoppingAssistantServer.Services
{
    public interface IStoreService
    {
        IEnumerable<Stores> GetAll();
        IEnumerable<Stores> GetStoresByAdmin(int id);
        Storeschedules GetStoreSchedule(int id);
        IEnumerable<Stores> FilterBySchedule(string UserDateTime);
        IEnumerable<Stores> FilterByStraightLineDistance(string userCoordinates, double radius, IEnumerable<Stores> stores);
        IEnumerable<Stores> FilterByGMapsDistance(string userCoordinates, IEnumerable<Stores> stores);
        IEnumerable<Tuple<string,string,double>> FilterByPrice(IEnumerable<Stores> stores, IEnumerable<ProductModel> products);
        IEnumerable<Tuple<string,string, double>> Filter(string userCoordinates, double radius, string user_time, IEnumerable<ProductModel> products);
        Stores CreateStore(Stores store, int idUser);
        void CreateSchedule(Storeschedules schedule);
        void UpdateStore(Stores newStore);
        void UpdateSchedule(Storeschedules newSchedule);
        void CreateProductStore(Productstore productstore);
        void UpdateProductStore(Productstore productstore);

    }
    public class StoreService : IStoreService
    {
        private DataContext _context;

        public StoreService(DataContext context)
        {
            Console.WriteLine("Store service DI injection for DataContext -> ", context);
            _context = context;
        }

        public IEnumerable<Stores> GetAll()
        {
            return _context.Stores.ToList();
        }

        public IEnumerable<Stores> GetStoresByAdmin(int id)
        {
            var storeadmins = (from sa in _context.StoreAdmin
                               where sa.Id_admin == id
                               select sa).ToList();
            List<Stores> stores = new List<Stores>();
            for (int i = 0; i < storeadmins.Count; i++)
            {
                var store = (from s in _context.Stores
                             where s.Id == storeadmins[i].Id_store
                             select s).SingleOrDefault();
                stores.Add(store);
            }
            return stores;
        }

        public Storeschedules GetStoreSchedule(int id)
        {
            return (from ss in _context.Storeschedules
                    where ss.Id_store == id
                    select ss).ToList().SingleOrDefault();
        }


        public IEnumerable<Stores> FilterBySchedule(string UserDateTime)
        {
            Console.WriteLine("Filter by schedule: ", UserDateTime);
            List<Stores> result = new List<Stores>();
            GetAll().ToList().ForEach(store =>
                {
                    //get program for current store
                    var program = from p in _context.Storeschedules where p.Id_store == store.Id select p;
                    Console.WriteLine(program);
                    var prog = program.SingleOrDefault();
                    if(prog != null)
                        if (Filters.IsOpen(UserDateTime, store, prog))
                            result.Add(store);
                });
            return result;
        }

        public IEnumerable<Stores> FilterByStraightLineDistance(string userCoordinates, double radius, IEnumerable<Stores> stores)
        {
            return Filters.FilterByRadius(userCoordinates, radius, stores);
        }

        public IEnumerable<Stores> FilterByGMapsDistance(string userCoordinates, IEnumerable<Stores> stores)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tuple<string, string, double>> FilterByPrice(IEnumerable<Stores> stores, IEnumerable<ProductModel> products)
        {
            IEnumerable<Productstore> productstores = _context.Productstore.ToList();
            return Filters.FilterByPrice(stores, products, productstores);
        }

        public IEnumerable<Tuple<string, string, double>> Filter(string userCoordinates, double radius, string user_time, IEnumerable<ProductModel> products)
        {
            products.ToList().ForEach(prod =>
            {
                if (prod.Description.Equals("") && prod.Producer.Equals(""))
                {
                    var pr = from p in _context.Products 
                             where p.Name == prod.Name 
                             select p;
                    if(pr.SingleOrDefault() != null)
                    {
                        prod.Id = pr.First().Id;
                        prod.Weight = pr.First().Weight;
                    }
                    
                }
                else if (prod.Producer.Equals(""))
                {
                    var pr = from p in _context.Products 
                             where p.Name == prod.Name & p.Description == prod.Description 
                             select p;
                    if(pr.SingleOrDefault() != null)
                    {
                        prod.Id = pr.First().Id;
                        prod.Weight = pr.First().Weight;
                    }
                    
                }
                else if (prod.Description.Equals(""))
                {
                    var pr = from p in _context.Products
                             where p.Name == prod.Name & p.Producer == prod.Producer
                             select p;
                    if(pr.SingleOrDefault() != null)
                    {
                        prod.Id = pr.First().Id;
                        prod.Weight = pr.First().Weight;
                    }
                    
                }
                else
                {
                    var pr = from p in _context.Products
                             where p.Name == prod.Name & p.Producer == prod.Producer & p.Description == prod.Description
                             select p;
                    if(pr.First() != null)
                    {
                        prod.Id = pr.First().Id;
                        prod.Weight = pr.First().Weight;
                    }
                   
                }

            });
            return FilterByPrice(
                    FilterByStraightLineDistance(
                            userCoordinates,
                            radius,
                            FilterBySchedule(
                                    user_time
                                )
                        ),
                    products
                    );
                    
        }

        public Stores CreateStore(Stores store, int idUser)
        {
            //throw new NotImplementedException();
            Console.WriteLine(store.Address);
            Console.WriteLine(store.Geographic_coordinates);
            Console.WriteLine(store.Name);
            _context.Stores.Add(store);
            _context.SaveChanges();
            var createdStore = (from s in _context.Stores
                    orderby s.Id descending
                    select s).First();
            StoreAdmin storeAdmin = new StoreAdmin(createdStore.Id, idUser);
            Console.WriteLine("Store admin: {0}", idUser);
            Console.WriteLine("Store id: {0}", createdStore.Id);
            _context.StoreAdmin.Add(storeAdmin);
            _context.SaveChanges();
            return createdStore;
        }

        public void CreateSchedule(Storeschedules schedule)
        {
            //throw new NotImplementedException();
            _context.Storeschedules.Add(schedule);
            _context.SaveChanges();
        }

        public void UpdateStore(Stores newStore)
        {
            //throw new NotImplementedException();
            _context.Stores.Update(newStore);
            _context.SaveChanges();
        }

        public void UpdateSchedule(Storeschedules newSchedule)
        {
            //throw new NotImplementedException();
            _context.Storeschedules.Update(newSchedule);
            _context.SaveChanges();
        }

        public void CreateProductStore(Productstore productstore)
        {
            _context.Add(productstore);
            _context.SaveChanges();
        }

        public void UpdateProductStore(Productstore productstore)
        {

            var prodst = (from ps in _context.Productstore
             where ps.Id_product == productstore.Id_product && ps.Id_store == productstore.Id_store
             select ps).SingleOrDefault();
            prodst.Availability = productstore.Availability;
            prodst.Price = productstore.Price;
            _context.SaveChanges();
        }
    }
}
