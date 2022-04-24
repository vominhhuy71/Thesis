using InventoryManagement.Model;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace InventoryManagement.ViewModel
{
    public class NewSupplierViewModel
    {
        #region Fields
        public event Action OnRequestClose = delegate { };

        public Supplier Supplier { get; set; }
        #endregion

        #region Constructor
        public NewSupplierViewModel()
        {
            Supplier = new Supplier();
        }
        #endregion

        #region ICommand
        RelayCommand _saveSupplier;
        public ICommand SaveSupplier
        {
            get
            {
                if (_saveSupplier == null)
                {
                    _saveSupplier = new RelayCommand(o => SaveNewSupplier(o));
                }
                return _saveSupplier;
            }
        }
        #endregion

        #region Functions

        private async void SaveNewSupplier( object o )
        {
            if (!ValidateSupplier())
            {
                return;
            }
            Boolean result = await Global.AddNewSupplier(Supplier);
            if (result)
            {
                MessageBox.Show("Supplier is added!");
                OnRequestClose?.Invoke();
            }
        }
        #endregion

        #region Validation
        bool IsValidEmail()
        {
            string email = Supplier.Email;
            if(email == null) return false;

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


        bool IsValidPhone( )
        {
            string phone = Supplier.Phone;
            const string regex = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

            if (phone != null) return Regex.IsMatch(phone, regex);
            else return false;
        }

        bool IsNotEmpty ( string value)
        {
            
            if(String.IsNullOrEmpty(value) && String.IsNullOrWhiteSpace(value)) return false;
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

            if(!IsNotEmpty(Supplier.Address))
            {
                MessageBox.Show("Invalid Address!");
                return false;
            }

            
            return true;
        }
        #endregion
    }
}
