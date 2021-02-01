using ShoppingAssistantServer.Entities;
using ShoppingAssistantServer.Models.ShoppingList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAssistantServer.Models.Filters
{
    public class FilterModel
    {
        public string UserCoordinates { get; set; }
        public string UserDateTime { get; set; }
        public double Radius { get; set; }
        public IEnumerable<ProductModel> ShoppingListContent { get; set; }

        public FilterModel()
        {

        }

        public FilterModel(string userCoord, string dateTime, double rad, IEnumerable<ProductModel> prod)
        {
            UserCoordinates = userCoord;
            UserDateTime = dateTime;
            Radius = rad;
            ShoppingListContent = prod;
        }
    }
}
