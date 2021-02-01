using ShoppingAssistantServer.Entities;
using ShoppingAssistantServer.Models.ShoppingList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAssistantServer.Models.Filters
{
    public class ShoppingListPriceModel
    {
        public List<ProductModel> Products { get; }
        public List<Stores> Stores { get; }
        public ShoppingListPriceModel()
        {

        }
        public ShoppingListPriceModel(List<Stores> stores, List<ProductModel> products)
        {
            Stores = stores;
            Products = products;
        }
    }
}
