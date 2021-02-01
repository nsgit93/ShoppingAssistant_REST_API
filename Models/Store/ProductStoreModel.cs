using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAssistantServer.Models.Store
{
    public class ProductStoreModel
    {
        public int Id_product { get; set; }
        public int Id_store { get; set; }
        public double Price { get; set; }
        public bool Availability { get; set; }
    }
}
