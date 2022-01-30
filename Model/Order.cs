using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Model
{
    public class Order
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string SupplierId { get; set; }

        public int Quantity { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public List<int> ItemIds { get; set; }

    }
}
