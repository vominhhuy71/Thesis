using InventoryManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InventoryManagement.ViewModel
{
    class NewItemViewModel:ViewModelBase
    {
        #region Fields
        private Item _item;
        RelayCommand _save;
        #endregion

        #region Constructor

        public NewItemViewModel()
        {
            _item = new Item();
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
        #endregion

        #region Event
        private void SaveItemToServer()
        {
            //MessageBox.Show(_item.Name + " " +_item.Type+ " "+ _item.Quantity );
        }
        #endregion
    }
}
