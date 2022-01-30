using InventoryManagement.Model;
using InventoryManagement.View;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InventoryManagement.ViewModel
{
    class ItemViewModel: ViewModelBase
    {
        #region Constructors
        public ItemViewModel()
        {

        }

        public ItemViewModel(Item item)
        {
            _item = item;
        }
        #endregion

        #region Fields
        private Item _item { get; set; }

        public Item Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
                OnPropertyChanged("Item");
            }
        }

        #endregion

        #region RelayCommand 
        RelayCommand _editReorder;
        public ICommand EditReorder 
        { 
            get
            {
                if(_editReorder == null)
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

        private async Task SendReorder()
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("huy.vminh@gmail.com");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("huy.vminh71@gmail.com");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
           

        }

        async private void SaveItemFunc(object _object)
        {
            Window window = (Window)_object;
            if (_item.Quantity < 5)
            {
                await SendReorder();
               
            }
            if (window != null)
            {
                window.Close();
            }
            
        }

        private void DeleteItemFunc(object _object)
        {
            Window window = (Window)_object;
            if (window != null)
            {
                window.Close();
            }


        }

        private void OpenReorderDialog()
        {
            ReorderView reorderView = new ReorderView();
            ReorderViewModel reorderViewModel = new ReorderViewModel();
            reorderView.DataContext = reorderViewModel;
            reorderView.ShowDialog();
        }

        private void OpenOrderDialog()
        {
            View.NewOrder newOrder = new View.NewOrder();
            newOrder.ShowDialog();
        }

        #endregion
    }
}
