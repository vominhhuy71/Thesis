using InventoryManagement.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace InventoryManagement.ViewModel
{
    public class AddItemViewModel : ViewModelBase
    {
        #region Fields
        public ObservableCollection<Item> Items { get; set; }

        public Item SelectedItem { get; set; }

        public int Quantity { get; set; }

        public event Action<ItemOrderedDetail> OnRequestSave;
        #endregion

        #region Constructor
        public AddItemViewModel()
        {
            Items = Global.Items;
            Quantity = 0;
            SelectedItem = null;
        }
        #endregion

        #region ICommand
        RelayCommand _addItem;
        public ICommand AddItemButton
        {
            get
            {
                if (_addItem == null)
                {
                    _addItem = new RelayCommand(o => AddItemToDatabase(o));
                }
                return _addItem;
            }
        }
        #endregion


        #region Helpers
        private void AddItemToDatabase( object o )
        {
            OnRequestSave(new ItemOrderedDetail { Item = Global.Items.Single(item => item.Id == SelectedItem.Id), Quantity = Quantity });
        }
        #endregion
    }
}
