using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAssistantServer.Entities
{
    public class Storeschedules
    {
		public int Id_store { get; set; }
		public string Monday { get; set; }
		public string Tuesday { get; set; }
		public string Wednesday { get; set; }
		public string Thursday { get; set; }
		public string Friday { get; set; }
		public string Saturday { get; set; }
		public string Sunday { get; set; }

        /*public Storeprograms(int id, string monday, string tuesday, string wednesday, string thursday, string friday, string saturday, string sunday)
        {
            Id = id;
            Monday = monday;
            Tuesday = tuesday;
            Wednesday = wednesday;
            Thursday = thursday;
            Friday = friday;
            Saturday = saturday;
            Sunday = sunday;
        }*/
    }
}
