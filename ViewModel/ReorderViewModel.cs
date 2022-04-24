using InventoryManagement.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace InventoryManagement.ViewModel
{
    public class ReorderViewModel : ViewModelBase
    {

        public event Action<Reorder> OnRequestClose;
        public event Action OnCloseReorder;

        private int _minimum;
        public int Minimum
        {
            get { return _minimum; }
            set
            {
                _minimum = value;
                OnPropertyChanged("Minimum");
            }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        private Supplier _ssupplier { get; set; }
        public Supplier SelectedSupplier
        {
            get { return _ssupplier; }
            set
            {
                _ssupplier = value;
                OnPropertyChanged("SelectedSupplier");
            }
        }



        private ObservableCollection<Supplier> _suppliers { get; set; }
        public ObservableCollection<Supplier> Suppliers
        {
            get { return _suppliers; }
            set
            {
                _suppliers = value;
                OnPropertyChanged("Suppliers");
            }
        }

        public ReorderViewModel()
        {

            Suppliers = Global.Suppliers;
            _minimum = 0;
            _ssupplier = null;
        }

        public ReorderViewModel( Reorder reorder )
        {

            Suppliers = Global.Suppliers;
            _minimum = reorder.MinQuantity;
            _quantity = reorder.Quantity;
            _ssupplier = (Supplier)Suppliers.Single(x => x.SupplierId == reorder.SupplierId);
        }

        private RelayCommand _onSave;
        public ICommand OnSave
        {
            get
            {
                if (_onSave == null)
                {
                    _onSave = new RelayCommand(o => OnSaveReorder());
                }

                return _onSave;
            }
        }

        private RelayCommand _onDelete;
        public ICommand OnDelete
        {
            get
            {
                if (_onDelete == null)
                {
                    _onDelete = new RelayCommand(o => OnDeleteReorder());
                }

                return _onDelete;
            }
        }

        public void OnDeleteReorder()
        {
            OnCloseReorder?.Invoke();
        }

        public void OnSaveReorder()
        {
            if(Minimum <=0)
            {
                MessageBox.Show("Invalid Minimum");
                return;
            }
            if (Quantity <= 0)
            {
                MessageBox.Show("Invalid Quantity");
                return ;
            }
            if(SelectedSupplier is null)
            {
                MessageBox.Show("Please select a supplier");
                return;
            }
            OnRequestClose(new Reorder { MinQuantity = Minimum, SupplierId = SelectedSupplier.SupplierId, Quantity = Quantity });
        }
    }
}
