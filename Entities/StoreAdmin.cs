using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAssistantServer.Entities
{
    public class StoreAdmin
    {
        public int Id_store { get; set; }
        public int Id_admin { get; set; }

        public StoreAdmin() { }

        public StoreAdmin(int idStore, int idAdmin)
        {
            Id_store = idStore;
            Id_admin = idAdmin;
        }

    }
}
