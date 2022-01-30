using InventoryManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InventoryManagement.ViewModel
{
    class ManagerViewModel : ViewModelBase
    {   
        public HomeViewModel homeViewModel { get; set; }

        public OrdersViewModel ordersViewModel { get; set; }


        public SuppliersViewModel suppliersViewModel { get; set; }

        private object _currentView { get; set; }

        public object CurrentView
        {
            get
            {
                return _currentView;
            }
            set
            {
                _currentView = value;
                OnPropertyChanged("CurrentView");
            }
        }

        RelayCommand _homeViewCommand;
        public ICommand homeViewCommand
        {
            get
            {
                if (_homeViewCommand == null)
                {
                    _homeViewCommand = new RelayCommand(o =>
                    {
                        CurrentView = homeViewModel;
                    });
                }
                return _homeViewCommand;
            }
        }

        RelayCommand _ordersViewCommand;
        public ICommand ordersViewCommand
        {
            get
            {
                if (_ordersViewCommand == null)
                {
                    _ordersViewCommand = new RelayCommand(o =>
                    {
                        ordersViewModel = new OrdersViewModel();
                        CurrentView = ordersViewModel;
                    });
                }
                return _ordersViewCommand;
            }
        }


        RelayCommand _suppliersViewCommand;
        public ICommand suppliersViewCommand
        {
            get
            {
                if (_suppliersViewCommand == null)
                {
                    _suppliersViewCommand = new RelayCommand(o =>
                    {
                        suppliersViewModel = new SuppliersViewModel();
                        CurrentView = suppliersViewModel;
                    });
                }
                return _suppliersViewCommand;
            }
        }

        public ManagerViewModel()
        {
            homeViewModel = new HomeViewModel();         
            CurrentView = homeViewModel;
        }
    }
}
