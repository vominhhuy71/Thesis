using InventoryManagement.Model;
using InventoryManagement.View;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace InventoryManagement.ViewModel
{
    public class SuppliersViewModel : ViewModelBase
    {
        #region Fields
        private ObservableCollection<Supplier> _supplierList;
        public ObservableCollection<Supplier> SuppliersList
        {
            get { return _supplierList; }
            set
            {
                _supplierList = value;
                OnPropertyChanged("SuppliersList");
            }
        }
        public Item SelectedSupplier { get; set; }

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
                FilterSuppliers();
            }
        }
        #endregion

        #region Constructor
        public SuppliersViewModel()
        {
            SuppliersList = new ObservableCollection<Supplier>();
            Load_Suppliers();

            Global.InitializeDispatchTimer(DispatchTimer_LoadData);

        }
        #endregion

        #region Function

        private void FilterSuppliers()
        {
            if (string.IsNullOrEmpty(Filter))
            {
                SuppliersList = Global.Suppliers;
            }
            else
            {
                ObservableCollection<Supplier> foundSuppliers = new ObservableCollection<Supplier>();
                foreach (var supplier in Global.Suppliers)
                {
                    if (supplier.Name.Contains(Filter)) foundSuppliers.Add(supplier);
                }
                SuppliersList = foundSuppliers;
            }
        }

        private async void Load_Suppliers()
        {
            await Global.Load_Suppliers();
            SuppliersList = Global.Suppliers;

            Action action = OnLastReload;
            if (action != null)
            {
                action();
            }
        }

        private void OpenSupplierDialog( object _object )
        {
            SupplierDetail supplierDetailView = new SupplierDetail();
            SupplierDetailViewModel supplierDetailViewModel = new SupplierDetailViewModel((Supplier)_object);
            supplierDetailView.DataContext = supplierDetailViewModel;
            supplierDetailViewModel.OnRequestClose += () =>
           {
               supplierDetailView.Close();
               Load_Suppliers();

           };
            supplierDetailView.ShowDialog();
        }

        private void OpenNewSupplierDialog()
        {
            NewSupplierView newSupplierView = new NewSupplierView();
            NewSupplierViewModel newSupplierViewModel = new NewSupplierViewModel();
            newSupplierView.DataContext = newSupplierViewModel;
            newSupplierViewModel.OnRequestClose += () =>
           {
               newSupplierView.Close();
               Load_Suppliers();
           };
            newSupplierView.ShowDialog();
        }


        public void DispatchTimer_LoadData( object sender, EventArgs e )
        {
            Load_Suppliers();
        }
        #endregion

        #region RelayCommand
        RelayCommand _SupplierSelected;
        public ICommand SupplierSelected
        {
            get
            {
                if (_SupplierSelected == null)
                {
                    _SupplierSelected = new RelayCommand(o => OpenSupplierDialog(o));
                }
                return _SupplierSelected;
            }
        }

        RelayCommand _newSupplier;
        public ICommand NewSupplier
        {
            get
            {
                if (_newSupplier == null)
                {
                    _newSupplier = new RelayCommand(o => OpenNewSupplierDialog());
                }
                return _newSupplier;
            }
        }

        #endregion
    }
}
