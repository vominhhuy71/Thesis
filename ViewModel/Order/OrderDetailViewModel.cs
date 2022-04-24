using InventoryManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace InventoryManagement.ViewModel
{
    public class OrderDetailViewModel
    {
        #region Fields
        public OrderViewModelClass Order { get; set; }

        public ObservableCollection<ItemOrderedDetail> itemOrderedDetails { get; set; }

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

        public bool IsAllowToBeSaved
        {
            get
            {
                if(!(Order is null) && Order.Status == OrderStatus.FINISHED)
                {
                    return false;
                }
                return true;
            }
        }

        #endregion

        #region Constructors
        public OrderDetailViewModel()
        {

        }
        public OrderDetailViewModel( string orderId )
        {
            Order order = Global.Orders.Single(o => o.Id == orderId);
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

            Order = orderViewModelClass;
            itemOrderedDetails = new ObservableCollection<ItemOrderedDetail>(Order.Items);
        }

        #endregion

        #region Functions
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

        private async void SaveOrder( object o )
        {
            Boolean result = await Global.UpdateOrder(Order.Id, Order.Status.ToString(), Order.Items);
            if (result)
            {
                MessageBox.Show("Order is updated!");
                OnRequestClose?.Invoke();

            }
        }

        private async void DeleteOrder( object o )
        {
            Boolean result = await Global.DeleteOrder(Order.Id);
            if (result)
            {
                MessageBox.Show("Order is deleted!");
                OnRequestClose?.Invoke();
            }
        }
        #endregion

        #region ICommand
        RelayCommand _saveChange;
        public ICommand SaveChange
        {
            get
            {
                if (_saveChange == null)
                {
                    _saveChange = new RelayCommand(o => SaveOrder(o));
                }
                return _saveChange;
            }
        }

        RelayCommand _deleteOrder;
        public ICommand Delete
        {
            get
            {
                if (_deleteOrder == null)
                {
                    _deleteOrder = new RelayCommand(o => DeleteOrder(o));
                }
                return _deleteOrder;
            }
        }
        #endregion
    }
}
