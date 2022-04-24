using InventoryManagement.Model;
using InventoryManagement.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace InventoryManagement.ViewModel
{
    public class ItemViewModel : ViewModelBase
    {
        #region Constructors

        public ItemViewModel()
        {
            _item = new Model.Item();

        }

        public ItemViewModel( string itemId )
        {
            LoadOrders();
            _item = new Model.Item();
            Orders = new ObservableCollection<Order>();
            _item = (Model.Item)Global.Items.Single(item => item.Id == itemId);
            _oldItem = new Model.Item{ Id=_item.Id, Location=_item.Location, Name=_item.Name, Quantity=_item.Quantity, ReceivedDate=_item.ReceivedDate, Reorder=_item.Reorder, Type=_item.Type};
        }
        #endregion

        #region Fields
        private Model.Item _oldItem { get; set; }

        private Model.Item _item { get; set; }

        public bool IsThereReorder
        {
            get
            {
                return _item.Reorder?.MinQuantity > 0;
            }
        }

        public Model.Item Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
                OnPropertyChanged("Item");
                OnPropertyChanged("IsThereReorder");

            }
        }

        public ObservableCollection<Order> Orders { get; set; }

        public event Action OnRequestSave;

        #endregion

        #region RelayCommand 
        RelayCommand _editReorder;
        public ICommand EditReorder
        {
            get
            {
                if (_editReorder == null)
                {
                    _editReorder = new RelayCommand(o => OpenReorderDialog());
                };
                return _editReorder;
            }
        }



        RelayCommand _newReorder;
        public ICommand NewReorder
        {
            get
            {
                if (_newReorder == null)
                {
                    _newReorder = new RelayCommand(o => OpenReorderDialog());
                };
                return _newReorder;
            }
        }

        RelayCommand _saveItem;
        public ICommand SaveItem
        {
            get
            {
                if (_saveItem == null)
                {
                    _saveItem = new RelayCommand(o => SaveItemFunc(o));
                };
                return _saveItem;
            }
        }

        RelayCommand _deleteItem;
        public ICommand DeleteItem
        {
            get
            {
                if (_deleteItem == null)
                {
                    _deleteItem = new RelayCommand(o => DeleteItemFunc(o));
                };
                return _deleteItem;
            }
        }

        RelayCommand _newOrder;
        public ICommand NewOrder
        {
            get
            {
                if (_newOrder == null)
                {
                    _newOrder = new RelayCommand(o => OpenOrderDialog());
                };
                return _newOrder;
            }
        }

        RelayCommand _viewOrder;
        public ICommand ViewOrder
        {
            get
            {
                if (_viewOrder == null)
                {
                    _viewOrder = new RelayCommand(o => OpenOrderDialog());
                };
                return _viewOrder;
            }
        }

        #endregion

        #region Functions

        async private void SaveItemFunc( object _object )
{
            Window window = (Window)_object;
            if (_item.Quantity <= _item.Reorder?.MinQuantity)
            {
                try
                {
                    Boolean foundReorder = false;
                    for (int i = 0; i < Global.Orders.Count; i++)
                    {
                        for (int j = 0; j < Global.Orders[i].Items.Count; j++)
                        {
                            if (Global.Orders[i].Items[j].ItemId == _item.Id && Global.Orders[i].Type == OrderType.REORDER && Global.Orders[i].Status == OrderStatus.DELIVERING)
                            {
                                MessageBox.Show("There is already an reorder for this Item on delivery!");
                                foundReorder = true;

                            }
                        }
                    }

                    if (!foundReorder)
                    {
                        List<ItemOrdered> itemOrdereds = new List<ItemOrdered>();
                        itemOrdereds.Add(new ItemOrdered { ItemId = _item.Id, Quantity = _item.Reorder.Quantity });
                        bool result = await Global.AddNewOrder(new Order { Name = $"Reorder for {_item.Name}", OrderDate = DateTime.Now, Items = itemOrdereds, Status = OrderStatus.DELIVERING, Type = OrderType.REORDER, SupplierId = _item.Reorder.SupplierId });
                        if (result)
                        {
                            Global.SendEmail(_item.Reorder.SupplierId, _item.Reorder.Quantity, _item.Name);
                            MessageBox.Show("Sent reorder email to supplier!");
                        }
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            await Global.UpdateItem(_oldItem, _item);

            if (window != null)
            {
                OnRequestSave();
                window.Close();
            }

        }

        private async void DeleteItemFunc( object _object )
        {
            await Global.DeleteItem(_item.Id);


            Window window = (Window)_object;
            if (window != null)
            {
                OnRequestSave();
                window.Close();
            }

        }

        private void OpenReorderDialog()
        {
            ReorderView reorderView = new ReorderView();
            ReorderViewModel reorderViewModel = IsThereReorder ? new ReorderViewModel(_item.Reorder) : new ReorderViewModel();
            reorderView.DataContext = reorderViewModel;
            reorderViewModel.OnRequestClose += async ( value ) =>
            {
                _item.Reorder = value;
                await Global.UpdateItem(_oldItem, _item);
                reorderView.Close();
                LoadOrders();
            };
            reorderViewModel.OnCloseReorder += async () =>
            {
                Item.Reorder = null;
                await Global.UpdateItem(_oldItem, _item);
                reorderView.Close();
                LoadOrders();
            };
            reorderView.ShowDialog();
        }

        private void OpenOrderDialog()
        {
            View.NewOrder newOrder = new View.NewOrder();
            newOrder.ShowDialog();
        }

        private async void LoadOrders()
        {
            Orders = null;
            Orders = await Global.Load_Orders();
        }

        #endregion
    }
}
