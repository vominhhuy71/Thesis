namespace InventoryManagement.Model
{
    public class Reorder
    {

        public string SupplierId { get; set; }

        /// <summary>
        /// Reorder rightaway if the quantity reach minimum value
        /// </summary>
        public int MinQuantity { get; set; }

        public int Quantity { get; set; }
    }

    public class ReorderDB
    {
        public int minimum { get; set; }

        public string supplierId { get; set; }

        public int quantity { get; set; }

    }
}
