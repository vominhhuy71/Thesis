using System;

namespace InventoryManagement.Model
{

    public class Item
    {

        public string Id { get; set; }


        public string Name { get; set; }


        public string Type { get; set; }


        public string Location { get; set; }


        public int Quantity { get; set; }


        public DateTime ReceivedDate { get; set; }


        public Reorder Reorder { get; set; }

    }

}
