using InventoryManagement.Model;
using InventoryManagement.View;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace InventoryManagement.ViewModel
{

    public class HomeViewModel : ViewModelBase
    {

        #region Fields
        public NewItemView newItemView { get; set; }

        private ObservableCollection<Item> _itemList { get; set; }
        public ObservableCollection<Item> ItemList
        {
            get
            {
                return _itemList;
            }
            set
            {
                _itemList = value;
                OnPropertyChanged("ItemList");
            }
        }
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
                FilterItems();
            }
        }


        public event Action OnLastReload = delegate { };
        #endregion

        #region Constructor

        public HomeViewModel()
        {
            ItemList = new ObservableCollection<Item>();

            LoadData();

            newItemView = new NewItemView();
            NewItemViewModel newItemViewModel = new NewItemViewModel();
            newItemView.DataContext = newItemViewModel;
            newItemViewModel.OnReloadItems += ( s, e ) => LoadData();

            Global.InitializeDispatchTimer(DispatchTimer_LoadData);
        }

        #endregion

        #region RelayCommand
        RelayCommand _ItemSelected;
        public ICommand ItemSelected
        {
            get
            {
                if (_ItemSelected == null)
                {
                    _ItemSelected = new RelayCommand(o => OpenItemDialog(o));
                }
                return _ItemSelected;
            }
        }
        #endregion

        #region Helper Function
        private void OpenItemDialog( dynamic _object )
        {
            ItemView itemView = new ItemView();
            ItemViewModel itemViewModel = new ItemViewModel(_object.Id);
            itemView.DataContext = itemViewModel;
            itemViewModel.OnRequestSave += () => LoadData();
            itemView.ShowDialog();

        }

        private void FilterItems()
        {
            if (string.IsNullOrEmpty(Filter))
            {
                ItemList = Global.Items;
            }
            else
            {
                ObservableCollection<Item> foundItems = new ObservableCollection<Item>();
                foreach (var item in Global.Items)
                {
                    if (item.Name.Contains(Filter)) foundItems.Add(item);
                }
                ItemList = foundItems;
            }
        }

        public async void LoadData()
        {
            await Global.Load_Items();
            Action action = OnLastReload;
            if (action != null)
            {
                action();
            }

            ItemList = Global.Items;

        }

        public void DispatchTimer_LoadData( object sender, EventArgs e )
        {
            LoadData();
        }
        #endregion
    }
}
