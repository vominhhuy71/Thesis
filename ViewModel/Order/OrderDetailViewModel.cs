using InventoryManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.ViewModel
{
    class OrderDetailViewModel
    {
        #region Fields
        public Order Order { get; set; }
        
        public Supplier Supplier { get; set; }
        
        public Item Item { get; set; }

        public ObservableCollection<Supplier> Suppliers { get; set; }

        public ObservableCollection<Item> Items { get; set; }
        #endregion

        #region Constructors
        public OrderDetailViewModel()
        {

        }
        public OrderDetailViewModel(Order order)
        {
            Order = order;
            Suppliers = Load_Suppliers();
            Items = Load_Items();

            //Item = Items.SingleOrDefault(i => i.Name == Order.Item.Name);
            Supplier = Suppliers.SingleOrDefault(s => s.SupplierId == Order.SupplierId);
        }
        #endregion

        #region Functions
        private ObservableCollection<Supplier> Load_Suppliers()
        {
            ObservableCollection<Supplier> suppliers = new ObservableCollection<Supplier>();
            //suppliers.Add(new Supplier {SupplierId = 1, Name = "Supplier A", Address = "StreetName 1", Email = "supplierA@test.com", Phone = "0123456789" });
            //suppliers.Add(new Supplier {SupplierId = 2, Name = "Supplier B", Address = "StreetName 2", Email = "supplierB@test.com", Phone = "0123456789" });
            //suppliers.Add(new Supplier {SupplierId = 3, Name = "Supplier C", Address = "StreetName 3", Email = "supplierC@test.com", Phone = "0123456789" });
            //suppliers.Add(new Supplier {SupplierId = 4, Name = "Supplier D", Address = "StreetName 4", Email = "supplierD@test.com", Phone = "0123456789" });
            //suppliers.Add(new Supplier {SupplierId = 5, Name = "Supplier E", Address = "StreetName 5", Email = "supplierE@test.com", Phone = "0123456789" });
            return suppliers;
        }

        private ObservableCollection<Item> Load_Items()
        {
            ObservableCollection<Item> items = new ObservableCollection<Item>();
            items.Add(new Item { Id = "10001", Name = "Item1", Location = "A1-1", Quantity = 10, Type = "Book"});
            items.Add(new Item { Id = "20001", Name = "Item2", Location = "A1-2", Quantity = 5, Type = "Pen" });
            items.Add(new Item { Id = "10002", Name = "Item3", Location = "A1-3", Quantity = 8, Type = "Book" });
            items.Add(new Item { Id = "20002", Name = "Item4", Location = "A1-4", Quantity = 7, Type = "Uncategorized" });
            return items;
        }
        #endregion
    }
}
