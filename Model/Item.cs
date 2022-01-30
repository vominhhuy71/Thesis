using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Model
{
    public class Item
    {
        public string Id { get; set; } 

        public string Name { get; set; }

        public string Type { get; set; }

        public string Location { get; set; }

        public int Quantity { get; set; }
        
        public List<Reorder> Reorders { get; set; }

        public List<Order> Orders { get; set; }

        public DateTime ReceivedDate { get; set; }
    }
}
