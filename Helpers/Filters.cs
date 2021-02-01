using ShoppingAssistantServer.Entities;
using ShoppingAssistantServer.Models.ShoppingList;
using ShoppingAssistantServer.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAssistantServer.Helpers
{
    public static class Filters
    {

        public static List<Stores> FilterByRadius(string userCoordinates, Double radius, IEnumerable<Stores> stores)
        {
            Console.WriteLine("----------->Filter by radius: " + radius);
            List<Stores> rez = new List<Stores>();
            string[] coordsUser = userCoordinates.Split(',');
            double userLat = double.Parse(coordsUser[0], CultureInfo.InvariantCulture);
            double userLong = double.Parse(coordsUser[1], CultureInfo.InvariantCulture);
            Point userLocation = new Point(userLat, userLong);
            foreach (Stores store in stores)
            {
                Console.WriteLine("--------->Calculating distance from user to store " + store.Name + " " + store.Address);
                string storeCoord = store.Geographic_coordinates;
                string[] coords = storeCoord.Split(',');

                //get store coords
                Double storeLat = double.Parse(coords[0], CultureInfo.InvariantCulture);
                Double storeLong = double.Parse(coords[1], CultureInfo.InvariantCulture);

                Point storeCoordinates = new Point(storeLat, storeLong);

                if (Calculator2D.GetDistance(userLocation, storeCoordinates) <= radius)
                {

                    rez.Add(store);
                }
            }
            Console.WriteLine("---------->>>>Matching store for desired radius: "+rez.Count);
            return rez;
        }

        public static bool IsOpen(string fullDate, Stores store, Storeschedules program)
        {
            Console.WriteLine(">>>>>>>>>>>>>>>Check if store " + store.Name + " is open at " + fullDate);
            //fullDate format: DD:MM:YY:HH:MM 

            if (fullDate.Equals("closed"))
                return false;

            string[] tokens = fullDate.Split(":");
            int day, month, year;

            day = Int32.Parse(tokens[0]);
            month = Int32.Parse(tokens[1]);
            year = Int32.Parse(tokens[2]);

            string currentTime = tokens[3] + ":" + tokens[4];

            DateTime dateValue = new DateTime(year, month, day);
            string DayOfWeek = dateValue.ToString("dddd");

            //verify if the store is open (for DayOfWeek)
            string interval = (string)program.GetType().GetProperty(DayOfWeek).GetValue(program, null);
            if (interval.Equals("closed"))
                return false;
            string[] openClose = interval.Split("-");

            TimeSpan openTime = TimeSpan.Parse(openClose[0]);
            TimeSpan closeTime = TimeSpan.Parse(openClose[1]);
            TimeSpan now = TimeSpan.Parse(currentTime);

            if (now >= openTime && now <= closeTime)
            {
                Console.WriteLine("Store is open " + store.Name);
                return true;
            }

            return false;
        }

        public static IEnumerable<Tuple<string, string, double>> FilterByPrice(IEnumerable<Stores> stores, IEnumerable<ProductModel> products, IEnumerable<Productstore> productstores)
        {
            Console.WriteLine(">>>>>>>>>>>>>>>Filter by price");
            List<Tuple<string, string, double>> prices = new List<Tuple<string, string, double>>();
            foreach (Stores store in stores)
            {
                Console.WriteLine("........verifying store " + store.Name + " " + store.Address);
                int numberOfAvailableProductsForEachStore = 0;
                double currentPrice = 0;

                foreach (ProductModel product in products)
                {
                    Console.WriteLine(".....Product: " + product.Name);
                    foreach (Productstore ps in productstores)
                    {
                        if (ps.Id_product == product.Id && store.Id == ps.Id_store && ps.Availability == true)
                        {
                            numberOfAvailableProductsForEachStore++;
                            if (product.Bulk_Product == true)
                            {
                                Console.WriteLine(".......Bulk product: " + product.Name+" quantity: "+product.Quantity);
                                currentPrice += product.Quantity * ps.Price * product.Weight;
                            }
                            else
                            {
                                Console.WriteLine(".......Not bulk product: " + product.Name + " quantity: " + product.Quantity);
                                currentPrice += product.Quantity * ps.Price;
                                
                            }
                        }
                    }
                }
                Console.WriteLine(".........store " + store.Name + " " + store.Address + " has " + numberOfAvailableProductsForEachStore + " available products");
                Console.WriteLine("......... " + products.ToArray().Length + " needed products");
                if (numberOfAvailableProductsForEachStore == products.ToArray().Length)
                {
                    prices.Add(new Tuple<string, string, double>(store.Name, store.Address, currentPrice));
                }
            }
            Console.WriteLine("Number of offers: " + prices.Count);
            return prices.OrderBy(x => x.Item3).ToList();
        }


    }
}
