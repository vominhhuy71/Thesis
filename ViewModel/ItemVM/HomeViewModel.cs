using InventoryManagement.Model;
using InventoryManagement.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InventoryManagement.ViewModel
{
    
    class HomeViewModel: ViewModelBase
    {
        #region Constructor
        public HomeViewModel()
        {
            ItemList = LoadData();
        }

        #endregion

        #region Fields
        public Item SelectedItem { get; set; }

        public ObservableCollection<Item> ItemList { get; set; }

        #endregion

        #region RelayCommand
        RelayCommand _ItemSelected;
        public ICommand ItemSelected
        {
            get
            {
                if (_ItemSelected == null)
                {
                    _ItemSelected = new RelayCommand(o=>OpenItemDialog(o));
                }
                return _ItemSelected;
            }
        }

        RelayCommand _newItem;
        public ICommand NewItem
        {
            get
            {
                if (_newItem == null)
                {
                    _newItem = new RelayCommand(o => OpenNewItemDialog());
                }
                return _newItem;
            }
        }

        #endregion

        #region Helper Function
        private void OpenItemDialog(object _object)
        {
            ItemView itemView = new ItemView();
            ItemViewModel itemViewModel = new ItemViewModel((Item)_object);
            itemView.DataContext = itemViewModel;
            itemView.ShowDialog();

        }

        private void OpenNewItemDialog()
        {
            NewItemView newItemView = new NewItemView();
            NewItemViewModel newItemViewModel = new NewItemViewModel();
            newItemView.DataContext = newItemViewModel;
            newItemView.ShowDialog();
        }

        public ObservableCollection<Item> LoadData()
        {
            DateTime localDate = DateTime.Now;
            ObservableCollection<Item> items = new ObservableCollection<Item>();
            items.Add(new Item { Id = "10001", Name = "Item1", Location = "A1-1", Quantity = 10, Type = "Book", ReceivedDate = localDate });
            items.Add(new Item { Id = "20001", Name = "Item2", Location = "A1-2", Quantity = 5, Type = "Pen", ReceivedDate = localDate });
            items.Add(new Item { Id = "10002", Name = "Item3", Location = "A1-3", Quantity = 8, Type = "Book", ReceivedDate = localDate });
            items.Add(new Item { Id = "20002", Name = "Item4", Location = "A1-4", Quantity = 7, Type = "Uncategorized", ReceivedDate = localDate });

            //var client = new WebClient();
            //string response = client.DownloadString("https://inventory-93a0b-default-rtdb.europe-west1.firebasedatabase.app/product.json");
            //ObservableCollection<Item> items = JsonConvert.DeserializeObject<ObservableCollection<Item>>(response) ;

            return items;
        }
        #endregion
    }
}
