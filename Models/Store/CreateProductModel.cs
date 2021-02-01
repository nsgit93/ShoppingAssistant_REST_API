using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAssistantServer.Models.Store
{
    public class CreateProductModel
    {
        public int Id_store { get; set; }
        public string Producer { get; set; }
        public double Weight { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool Bulk_product { get; set; }
        public string Quantity { get; set; }
        public double Price { get; set; }
        public bool Availability { get; set; }
    }
}
