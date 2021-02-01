using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAssistantServer.Models.ShoppingList
{
    public class ProductPriceAvailModel
    {
        public int Id { get; set; }
        public string Producer { get; set; }
        public double Weight { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Bulk_product { get; set; }
        public double Price { get; set; }
        public bool Availability { get; set; }

        public ProductPriceAvailModel() { }

    }
}
