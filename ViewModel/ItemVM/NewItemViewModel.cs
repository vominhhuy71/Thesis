using InventoryManagement.Model;
using InventoryManagement.View;
using System;
using System.Windows;
using System.Windows.Input;

namespace InventoryManagement.ViewModel
{
    public class NewItemViewModel : ViewModelBase
    {
        #region Fields
        private Item _item;

        public event EventHandler OnReloadItems;
        RelayCommand _save;
        #endregion

        #region Constructor
        public NewItemViewModel()
        {
            _item = new Item();
            ReceivedDate = DateTime.Now;
        }

        #endregion

        #region Item Properties
        public string Name
        {
            get
            {
                if (!string.IsNullOrEmpty(_item.Name))
                {
                    return _item.Name;

                }

                return "";
            }
            set
            {
                _item.Name = value;

                OnPropertyChanged("Name");
            }
        }
        public string Type
        {
            get
            {
                if (!string.IsNullOrEmpty(_item.Type))
                {
                    return _item.Type;

                }

                return "";
            }
            set
            {
                _item.Type = value;

                OnPropertyChanged("Type");
            }
        }

        public string Location
        {
            get
            {
                if (!string.IsNullOrEmpty(_item.Location))
                {
                    return _item.Location;

                }

                return "";
            }
            set
            {
                _item.Location = value;

                OnPropertyChanged("Location");
            }
        }

        public int Quantity
        {
            get
            {
                return _item.Quantity;
            }
            set
            {
                _item.Quantity = value;

                OnPropertyChanged("Quantity");
            }

        }

        public DateTime ReceivedDate
        {
            get
            {
                return _item.ReceivedDate;
            }
            set
            {
                _item.ReceivedDate = value;

                OnPropertyChanged("ReceivedDate");
            }

        }
        #endregion


        #region Commands
        public ICommand SaveItem
        {
            get
            {
                if (_save == null)
                {
                    _save = new RelayCommand(o => SaveItemToServer());
                };
                return _save;
            }
        }


        private RelayCommand setReorder;

        public ICommand SetReorder
        {
            get
            {
                if (setReorder == null)
                {
                    setReorder = new RelayCommand(o => OpenReorderDialog());
                }

                return setReorder;
            }
        }
        #endregion

        #region Event
        private async void SaveItemToServer()
{
            Boolean validate = ValidateNewItem();
            if (!validate)
            {
                return;
            }

            Boolean result = await Global.AddNewItem(_item);
            if (result)
            {
                MessageBox.Show("Save successfully!");
                _item = new Item();
                Name = "";
                Location = "";
                ReceivedDate = DateTime.Now;
                Quantity = 0;
                Type = "";

                OnReloadItems(this, new EventArgs());

            }

        }

        private void OpenReorderDialog()
        {
            ReorderView reorderView = new ReorderView();
            ReorderViewModel reorderViewModel = new ReorderViewModel();
            reorderView.DataContext = reorderViewModel;
            reorderViewModel.OnRequestClose += value =>
            {
                _item.Reorder = value;
                reorderView.Close();
            };
            reorderView.ShowDialog();
        }
        #endregion

        private bool ValidateNewItem()
        {
            if (String.IsNullOrEmpty(Name) || String.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Invalid Name");
                return false;
            }
            if (String.IsNullOrEmpty(Location) || String.IsNullOrWhiteSpace(Location))
            {
                MessageBox.Show("Invalid Location");
                return false;
            }
            if (String.IsNullOrEmpty(Type) || String.IsNullOrWhiteSpace(Type))
            {
                MessageBox.Show("Invalid Type");
                return false;
            }
            return true;
        }
        
    }
}
