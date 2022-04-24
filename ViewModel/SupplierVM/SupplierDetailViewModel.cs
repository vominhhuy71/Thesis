using InventoryManagement.Model;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace InventoryManagement.ViewModel
{
    public class SupplierDetailViewModel : ViewModelBase
    {
        public event Action OnRequestClose = delegate { };

        #region Fields
        public Supplier Supplier { get; set; }
        #endregion

        #region Constructors
        public SupplierDetailViewModel()
        {

        }

        public SupplierDetailViewModel( Supplier _supplier )
        {
            Supplier = _supplier;
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
                    _saveChange = new RelayCommand(o => SaveEdit(o));
                }
                return _saveChange;
            }
        }

        RelayCommand _delete;
        public ICommand Delete
        {
            get
            {
                if (_delete == null)
                {
                    _delete = new RelayCommand(o => DeleteSupplier(o));
                }
                return _delete;
            }
        }
        #endregion

        #region Function

        public async void SaveEdit( object o )
        {
            if (!ValidateSupplier())
            {
                return;
            }
            Boolean result = await Global.UpdateSupplier(Supplier);
            if (result)
            {
                MessageBox.Show("Supplier is updated!");
                OnRequestClose?.Invoke();
            }
        }

        public async void DeleteSupplier( object o )
        {
            Boolean result = await Global.DeleteSupplier(Supplier.SupplierId);
            if (result)
            {
                MessageBox.Show("Supplier is deleted!");
                OnRequestClose?.Invoke();

            }
        }
        #endregion

        #region Validation
        bool IsValidEmail()
        {
            string email = Supplier.Email;
            if (email == null) return false;

            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }


        bool IsValidPhone()
        {
            string phone = Supplier.Phone;
            const string regex = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

            if (phone != null) return Regex.IsMatch(phone, regex);
            else return false;
        }

        bool IsNotEmpty( string value )
        {

            if (String.IsNullOrEmpty(value) && String.IsNullOrWhiteSpace(value)) return false;
            return true;
        }

        bool ValidateSupplier()
        {

            if (!IsValidEmail())
            {
                MessageBox.Show("Invalid Email!");
                return false;
            }

            if (!IsValidPhone())
            {
                MessageBox.Show("Invalid Phone!");
                return false;
            }


            if (!IsNotEmpty(Supplier.Name))
            {
                MessageBox.Show("Invalid Name!");
                return false;
            }

            if (!IsNotEmpty(Supplier.Address))
            {
                MessageBox.Show("Invalid Address!");
                return false;
            }


            return true;
        }
        #endregion
    }
}
