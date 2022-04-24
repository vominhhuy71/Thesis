using System.ComponentModel;

namespace InventoryManagement.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region Constructor
        protected ViewModelBase()
        {

        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged( string propertyName )
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
