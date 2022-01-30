using InventoryManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.ViewModel
{
    class SupplierDetailViewModel:ViewModelBase
    {
        #region Fields
        public Supplier Supplier { get; set; }
        #endregion

        #region Constructors
        public SupplierDetailViewModel()
        {

        }

        public SupplierDetailViewModel(Supplier _supplier)
        {
            Supplier = _supplier;
        }
        #endregion
    }
}
