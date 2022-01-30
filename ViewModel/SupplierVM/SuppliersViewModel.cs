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
    class SuppliersViewModel
    {
        #region Fields
        public ObservableCollection<Supplier> SupplierList { get; set; }
        public Item SelectedSupplier { get; set; }
        #endregion

        #region Constructor
        public SuppliersViewModel()
        {
            SupplierList = Load_Supplier();
        }
        #endregion

        #region Function
        private ObservableCollection<Model.Supplier> Load_Supplier()
        {
            ObservableCollection<Supplier> suppliers = new ObservableCollection<Supplier>();
            suppliers.Add(new Supplier { Name = "Supplier A", Address = "StreetName 1", Email = "supplierA@test.com", Phone = "0123456789" });
            suppliers.Add(new Supplier { Name = "Supplier B", Address = "StreetName 2", Email = "supplierB@test.com", Phone = "0123456789" });
            suppliers.Add(new Supplier { Name = "Supplier C", Address = "StreetName 3", Email = "supplierC@test.com", Phone = "0123456789" });
            suppliers.Add(new Supplier { Name = "Supplier D", Address = "StreetName 4", Email = "supplierD@test.com", Phone = "0123456789" });
            suppliers.Add(new Supplier { Name = "Supplier E", Address = "StreetName 5", Email = "supplierE@test.com", Phone = "0123456789" });
            return suppliers;
        }

        private void OpenSupplierDialog(object _object)
        {
            //ItemView itemView = new ItemView();
            //ItemViewModel itemViewModel = new ItemViewModel((Item)_object);
            //itemView.DataContext = itemViewModel;
            //itemView.ShowDialog();

            SupplierDetail supplierDetailView = new SupplierDetail();
            SupplierDetailViewModel supplierDetailViewModel = new SupplierDetailViewModel((Supplier)_object);
            supplierDetailView.DataContext = supplierDetailViewModel;
            supplierDetailView.ShowDialog();
        }

        private void OpenNewSupplierDialog()
        {
            NewSupplierView newSupplierView = new NewSupplierView();
            newSupplierView.ShowDialog();
            Load_Supplier();
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
