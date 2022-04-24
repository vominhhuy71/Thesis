using System;
using System.Collections.Generic;

namespace InventoryManagement.Model
{
    public enum OrderStatus
    {
        DELIVERING,
        FINISHED,
        CANCELED,
    }

    public enum OrderType
    {
        NEW,
        REORDER
    }

    public class Order
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string SupplierId { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus Status { get; set; }

        public OrderType Type { get; set; }

        public List<ItemOrdered> Items { get; set; }

    }

    public class OrderViewModelClass
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Supplier Supplier { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus Status { get; set; }

        public OrderType Type { get; set; }

        public List<ItemOrderedDetail> Items { get; set; }

    }

    public class ItemOrdered
    {
        public string ItemId { get; set; }

        public int Quantity { get; set; }
    }

    public class ItemOrderedDetail
    {
        public Item Item { get; set; }

        public int Quantity { get; set; }
    }
}
