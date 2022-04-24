using InventoryManagement.Model;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InventoryManagement.ViewModel
{
    public class ManagerViewModel : ViewModelBase
    {
        public HomeViewModel homeViewModel { get; set; }

        public OrdersViewModel ordersViewModel { get; set; }

        public SuppliersViewModel suppliersViewModel { get; set; }

        public LogsViewModel logsViewModel { get; set; }

        private DateTime _lastRefresh { get; set; }
        public DateTime LastRefresh
        {
            get { return _lastRefresh; }
            set
            {
                _lastRefresh = value;
                OnPropertyChanged("LastRefresh");
            }
        }

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
                    _homeViewCommand = new RelayCommand(async o => await LoadView(o, "item"));
                }
                return _homeViewCommand;
            }
        }
        RelayCommand _logViewCommand;
        public ICommand logsViewCommand
        {
            get
            {
                if (_logViewCommand == null)
                {
                    _logViewCommand = new RelayCommand(async o => await LoadView(o, "log"));
                }
                return _logViewCommand;
            }
        }

        RelayCommand _ordersViewCommand;
        public ICommand ordersViewCommand
        {
            get
            {
                if (_ordersViewCommand == null)
                {
                    _ordersViewCommand = new RelayCommand(async o => await LoadView(o, "order"));
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
                    _suppliersViewCommand = new RelayCommand(async o => await LoadView(o, "supplier"));
                }
                return _suppliersViewCommand;
            }
        }

        public ManagerViewModel()
        {
            LastRefresh = DateTime.Now;

            homeViewModel = new HomeViewModel();
            homeViewModel.OnLastReload += () =>
            {
                LastRefresh = DateTime.Now;
            };
            CurrentView = homeViewModel;
        }

        public async Task LoadView( object o, string view )
        {
            if (view == null) return;
            if (view == "order")
            {

                await Global.Load_Orders();
                ordersViewModel = new OrdersViewModel();
                ordersViewModel.OnLastReload += () =>
                {
                    LastRefresh = DateTime.Now;

                };
                CurrentView = ordersViewModel;

            }
            else if (view == "supplier")
            {
                await Global.Load_Suppliers();
                suppliersViewModel = new SuppliersViewModel();
                suppliersViewModel.OnLastReload += () =>
                {
                    LastRefresh = DateTime.Now;

                };
                CurrentView = suppliersViewModel;
            }
            else if (view == "item")
            {
                await Global.Load_Items();
                homeViewModel = new HomeViewModel();
                homeViewModel.OnLastReload += () =>
                {
                    LastRefresh = DateTime.Now;
                };
                CurrentView = homeViewModel;
            }
            else if (view == "log")
            {
                await Global.Load_Logs();
                logsViewModel = new LogsViewModel();
                logsViewModel.OnLastReload += () => {
                    LastRefresh = DateTime.Now;
                };
                CurrentView= logsViewModel;
            }
        }

    }
}
