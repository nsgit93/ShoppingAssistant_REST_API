using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAssistantServer.Entities
{
	public class Stores
    {
		public int Id { get; set; }
		public string Address { get; set; }
		public string Geographic_coordinates { get; set; }
		public string Name { get; set; }

	}
}
