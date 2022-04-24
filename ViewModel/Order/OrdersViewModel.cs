using InventoryManagement.Model;
using InventoryManagement.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace InventoryManagement.ViewModel
{
    public class OrdersViewModel : ViewModelBase
    {
        #region Fields
        public OrderViewModelClass SelectedOrder { get; set; }

        private ObservableCollection<OrderViewModelClass> _ordersList;
        public ObservableCollection<OrderViewModelClass> OrdersList
        {
            get { return _ordersList; }
            set
            {
                _ordersList = value;
                OnPropertyChanged("OrdersList");
            }
        }

        public event Action OnLastReload = delegate { };

        private string _filter { get; set; }
        public string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
                FilterOrders();
            }
        }
        #endregion

        #region Constructor
        public OrdersViewModel()
        {
            Load_Orders();

            OrdersList = new ObservableCollection<OrderViewModelClass>();

            Global.InitializeDispatchTimer(DispatchTimer_LoadData);

        }
        #endregion

        #region Parser

        private OrderViewModelClass parseOrder( Order order )
        {
            OrderViewModelClass orderViewModelClass = new OrderViewModelClass()
            {
                Id = order.Id,
                Name = order.Name,
                OrderDate = order.OrderDate,
                Status = order.Status,
                Type = order.Type,
                Items = GetItems(order.Items),
                Supplier = GetSupplier(order.SupplierId)
            };

            return orderViewModelClass;
        }
        #endregion

        #region Functions
        private void FilterOrders()
        {
            if (string.IsNullOrEmpty(Filter))
            {
                ObservableCollection<OrderViewModelClass> orders = new ObservableCollection<OrderViewModelClass>();
                foreach (Order o in Global.Orders)
                {

                    orders.Add(parseOrder(o));
                }

                OrdersList = orders;
            }
            else
            {
                ObservableCollection<OrderViewModelClass> foundOrders = new ObservableCollection<OrderViewModelClass>();
                foreach (var order in Global.Orders)
                {
                    if (order.Name.Contains(Filter)) foundOrders.Add(parseOrder(order));
                }
                OrdersList = foundOrders;
            }
        }

        private async void Load_Orders()
        {
            await Global.Load_Orders();

            ObservableCollection<OrderViewModelClass> orders = new ObservableCollection<OrderViewModelClass>();
            foreach (Order o in Global.Orders)
            {

                orders.Add(parseOrder(o));
            }

            OrdersList = orders;

            OnLastReload?.Invoke();
        }

        private Supplier GetSupplier( string SupplierId )
        {
            for (int i = 0; i < Global.Suppliers.Count(); i++)
            {
                if (Global.Suppliers[i].SupplierId == SupplierId) return Global.Suppliers[i];
            }

            return new Supplier();
        }

        private List<ItemOrderedDetail> GetItems( List<ItemOrdered> Items )
        {
            List<ItemOrderedDetail> ItemsDetail = new List<ItemOrderedDetail>();
            for (int i = 0; i < Global.Items.Count(); i++)
            {
                for (int j = 0; j < Items.Count(); j++)
                {
                    if (Items[j].ItemId == Global.Items[i].Id)
                    {
                        ItemsDetail.Add(new ItemOrderedDetail { Item = Global.Items[i], Quantity = Items[j].Quantity });
                    }

                }

            }

            return ItemsDetail;
        }

        private void OpenOrderDialog( dynamic _object )
        {
            OrderDetail orderDetailView = new OrderDetail();
            OrderDetailViewModel orderDetailViewModel = new OrderDetailViewModel(_object.Id);
            orderDetailView.DataContext = orderDetailViewModel;
            orderDetailViewModel.OnRequestClose += () =>
           {
               orderDetailView.Close();
               Load_Orders();
           };
            orderDetailView.ShowDialog();

        }

        private void OpenNewOrderDialog()
        {
            View.NewOrder newOrder = new NewOrder();
            NewOrderViewModel newOrderViewModel = new NewOrderViewModel();
            newOrder.DataContext = newOrderViewModel;
            newOrderViewModel.OnRequestClose += () =>
            {
                newOrder.Close();
                Load_Orders();
            };
            newOrder.ShowDialog();
        }

        public void DispatchTimer_LoadData( object sender, EventArgs e )
        {
            Load_Orders();
        }
        #endregion

        #region ICommand
        RelayCommand _orderSelected;
        public ICommand OrderSelected
        {
            get
            {
                if (_orderSelected == null)
                {
                    _orderSelected = new RelayCommand(o => OpenOrderDialog(o));
                }
                return _orderSelected;
            }
        }
        RelayCommand _newOrder;
        public ICommand NewOrder
        {
            get
            {
                if (_newOrder == null)
                {
                    _newOrder = new RelayCommand(o => OpenNewOrderDialog());
                }
                return _newOrder;
            }
        }
        #endregion
    }
}
