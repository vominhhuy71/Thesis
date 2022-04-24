using InventoryManagement.Model;
using InventoryManagement.View;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace InventoryManagement.ViewModel
{
    public class LogsViewModel : ViewModelBase
    {
        #region Fields
        private ObservableCollection<Log> _logsList;
        public ObservableCollection<Log> LogsList
        {
            get { return _logsList; }
            set
            {
                _logsList = value;
                OnPropertyChanged("LogsList");
            }
        }

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
                Filterlogs();
            }
        }
        #endregion

        #region Constructor
        public LogsViewModel()
        {
            LogsList = new ObservableCollection<Log>();
            Load_logs();

            Global.InitializeDispatchTimer(DispatchTimer_LoadData);

        }
        #endregion

        #region Function

        private void Filterlogs()
        {
            if (string.IsNullOrEmpty(Filter))
            {
                LogsList = Global.Logs;
            }
            else
            {
                ObservableCollection<Log> foundlogs = new ObservableCollection<Log>();
                foreach (var log in Global.Logs)
                {
                    if (log.Name.Contains(Filter)) foundlogs.Add(log);
                }
                LogsList = foundlogs;
            }
        }

        private async void Load_logs()
        {
            await Global.Load_Logs();
            LogsList = Global.Logs;

            Action action = OnLastReload;
            if (action != null)
            {
                action();
            }
        }



        public void DispatchTimer_LoadData( object sender, EventArgs e )
        {
            Load_logs();
        }
        #endregion

    }
}
