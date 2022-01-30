using InventoryManagement.Model;
using InventoryManagement.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InventoryManagement.ViewModel
{
    class OrdersViewModel
    {
        #region Fields
        public Order SelectedOrder { get; set; }

        public ObservableCollection<Order> OrdersList { get; set; }
        #endregion

        #region Constructor
        public OrdersViewModel()
        {
            OrdersList = Load_Orders();
        }
        #endregion

        #region Function
        private ObservableCollection<Order> Load_Orders()
        {
            return Globals.Orders;
        }

        private void OpenOrderDialog(object _object)
        {
            OrderDetail orderDetailView = new OrderDetail();
            OrderDetailViewModel orderDetailViewModel = new OrderDetailViewModel((Order)_object);
            orderDetailView.DataContext = orderDetailViewModel;
            orderDetailView.ShowDialog();
        }

        private void OpenNewOrderDialog()
        {
            View.NewOrder newOrder = new NewOrder();
            NewOrderViewModel newOrderViewModel = new NewOrderViewModel();
            newOrder.DataContext = newOrderViewModel;
            newOrder.ShowDialog();
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
