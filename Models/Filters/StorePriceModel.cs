using ShoppingAssistantServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAssistantServer.Models.Filters
{
    public class StorePriceModel
    {
        public string storename { get; set; }
        public string storeaddress { get; set; }
        public double price { get; set; }

        public StorePriceModel() { }
        
        public StorePriceModel(string name, string address, double price)
        {
            this.storename = name;
            this.storeaddress = address;
            this.price = price;
        }

    }
}
