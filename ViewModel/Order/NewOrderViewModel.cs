using InventoryManagement.Model;
using InventoryManagement.View.Order;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace InventoryManagement.ViewModel
{
    public class NewOrderViewModel : ViewModelBase
    {
        #region Fields
        public event Action OnRequestClose = delegate { };

        public IEnumerable<OrderStatus> orderStatusEnum
        {
            get
            {
                return Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();
            }
        }

        public IEnumerable<OrderType> orderTypeEnum
        {
            get
            {
                return Enum.GetValues(typeof(OrderType)).Cast<OrderType>();
            }
        }

        public ObservableCollection<Supplier> Suppliers { get; set; }

        private ObservableCollection<ItemOrderedDetail> _items;
        public ObservableCollection<ItemOrderedDetail> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                OnPropertyChanged("Items");
            }
        }

        public ItemOrderedDetail SelectedItem { get; set; }
        public Supplier SelectedSupplier { get; set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        private DateTime _orderDate;
        public DateTime OrderDate
        {
            get { return _orderDate; }
            set { _orderDate = value; OnPropertyChanged("OrderDate"); }
        }


        private OrderStatus _status;
        public OrderStatus Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged("Status"); }
        }

        public OrderType _type;
        public OrderType Type
        {
            get { return _type; }
            set { _type = value; OnPropertyChanged("Type"); }
        }

        #endregion

        #region Constructors
        public NewOrderViewModel()
        {

            Suppliers = Global.Suppliers;
            _items = new ObservableCollection<ItemOrderedDetail>();
            _orderDate = DateTime.Now;
            _status = OrderStatus.DELIVERING;
            _type = OrderType.NEW;
        }


        #endregion

        #region Functions
        private void OpenAddItem( object o )
        {
            AddItem addItemView = new AddItem();
            AddItemViewModel addItemViewModel = new AddItemViewModel();
            addItemView.DataContext = addItemViewModel;
            addItemViewModel.OnRequestSave += ( value ) =>
            {
                var item = Items.SingleOrDefault(i => i.Item == value.Item);
                if (item != null)
                {
                    Items.Remove(item);
                    item.Quantity += value.Quantity;
                    Items.Add(item);
                }
                else
                {

                    Items.Add(value);

                }
                addItemView.Close();
            };
            addItemView.ShowDialog();

        }

        private void RemoveSelectedItem( object o )
        {
            Items.Remove(o as ItemOrderedDetail);
        }

        private async void SaveOrderToDatabase( object o )
        {
            Order order = new Order();
            order.Name = Name;
            order.SupplierId = SelectedSupplier.SupplierId;
            order.OrderDate = _orderDate;
            order.Type = _type;
            order.Status = _status;

            List<ItemOrdered> items = new List<ItemOrdered>();
            for (int i = 0; i < Items.Count; i++)
            {
                items.Add(new ItemOrdered { ItemId = Items[i].Item.Id, Quantity = Items[i].Quantity, });
            }

            order.Items = items;

            Boolean result = await Global.AddNewOrder(order);
            if (result)
            {
                MessageBox.Show("Success");
                OnRequestClose?.Invoke();
            };


        }

        #endregion

        #region ICommand
        RelayCommand _openAddItemDialog;
        public ICommand OpenAddItemDialog
        {
            get
            {
                if (_openAddItemDialog == null)
                {
                    _openAddItemDialog = new RelayCommand(o => OpenAddItem(o));
                }
                return _openAddItemDialog;
            }
        }


        RelayCommand _removeSelectedItem;
        public ICommand RemoveSelectedItemButton
        {
            get
            {
                if (_removeSelectedItem == null)
                {
                    _removeSelectedItem = new RelayCommand(o => RemoveSelectedItem(o));
                }
                return _removeSelectedItem;
            }
        }

        RelayCommand _saveOrder;
        public ICommand SaveOrder
        {
            get
            {
                if (_saveOrder == null)
                {
                    _saveOrder = new RelayCommand(o => SaveOrderToDatabase(o));
                }
                return _saveOrder;
            }
        }

        #endregion
    }
}
