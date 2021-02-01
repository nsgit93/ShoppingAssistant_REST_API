using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAssistantServer.Models.ShoppingList
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Producer { get; set; }
        public double Weight { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool Bulk_Product { get; set; }
        public double Quantity { get; set; }

        public ProductModel()
        {
            Name = "";
            Description = "";
            Producer = "";
        }
    }
}
