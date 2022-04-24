using InventoryManagement.Model;
using InventoryManagement.ViewModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace InventoryManagement.View
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        private GridViewColumnHeader listViewSortCol = null;
        public HomeView()
        {
            InitializeComponent();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ItemList.ItemsSource);
            //view.Filter = ItemFilter;

        }

        private void listViewDoubleClick_Handler( object sender, MouseButtonEventArgs e )
        {
            dynamic o = ((FrameworkElement)e.OriginalSource).DataContext;
            if (o.GetType() != typeof(Model.Item)) return;

            Item selectedItem = (Item)o;

            HomeViewModel currentDataContext = (HomeViewModel)DataContext;

            if (selectedItem != null)
            {
                currentDataContext.ItemSelected.Execute(selectedItem);
            }
        }


        private void GridViewColumnHeader_Click( object sender, RoutedEventArgs e )
        {
            GridViewColumnHeader column = (e.OriginalSource as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                ItemList.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column)
            {
                newDir = ListSortDirection.Descending;
            }


            listViewSortCol = column;
            ItemList.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void ListView_SizeChanged( object sender, SizeChangedEventArgs e )
        {
            ListView listView = sender as ListView;
            GridView gridView = listView.View as GridView;

            var width = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;

            var column1 = 0.20;
            var column2 = 0.20;
            var column3 = 0.20;
            var column4 = 0.20;
            var column5 = 0.20;


            gridView.Columns[0].Width = width * column1;
            gridView.Columns[1].Width = width * column2;
            gridView.Columns[2].Width = width * column3;
            gridView.Columns[3].Width = width * column4;
            gridView.Columns[4].Width = width * column5;

        }


    }
}
