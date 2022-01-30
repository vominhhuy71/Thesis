using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Model
{
    public class Reorder
    {

        public int SupplierId { get; set; }

        /// <summary>
        /// Reorder rightaway if the quantity reach minimum value
        /// </summary>
        public int MinQuantity { get; set; }

    }
}
