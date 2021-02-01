using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAssistantServer.Entities
{
 /*   CREATE TABLE products(
id INT NOT NULL primary key IDENTITY (10000, 1) ,
	producer VARCHAR(50) NOT NULL,
    weight FLOAT NOT NULL,
    name VARCHAR(50) NOT NULL,
    description VARCHAR(100) ,	
	category VARCHAR(50) NOT NULL,
    bulk_product BIT NOT NULL DEFAULT 1
) 
 */
    public class Products
    {
        public int Id { get; set; }
        public string Producer { get; set; }
        public double Weight { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool Bulk_Product { get; set; }
        /*
        public Products(int id, string producer, double weight, string name, string description, string category, bool bulk_Product)
        {
            Id = id;
            Producer = producer;
            Weight = weight;
            Name = name;
            Description = description;
            Category = category;
            Bulk_Product = bulk_Product;
        }*/
    }
}
