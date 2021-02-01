using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAssistantServer.Entities
{
    public class Admin
    {
        public int Id_user { get; set; }

        public Admin() { }

        public Admin(int id)
        {
            Id_user = id;
        }

    }
}
