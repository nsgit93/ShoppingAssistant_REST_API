using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ShoppingAssistantServer.Entities
{
    public class Productstore
    {
        public int Id_product { get; set; }
        public int Id_store { get; set; }
        public double Price { get; set; }
        public bool Availability { get; set; }
        /*public Productstore(int idp, int ids, double price, bool avail)
        {
            Id_product = idp;
            Id_store = ids;
            Price = price;
            Availability = avail;
        }*/
    }
}
