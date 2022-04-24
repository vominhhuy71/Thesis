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
    /// Interaction logic for OrdersView.xaml
    /// </summary>
    public partial class OrdersView : UserControl
    {
        private GridViewColumnHeader listViewSortCol = null;

        public OrdersView()
        {
            InitializeComponent();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ItemList.ItemsSource);
            //view.Filter = OrderFilter;
        }

        private void listViewDoubleClick_Handler( object sender, MouseButtonEventArgs e )
        {
            dynamic o = ((FrameworkElement)e.OriginalSource).DataContext;
            if (o.GetType() != typeof(Model.OrderViewModelClass)) return;

            OrderViewModelClass selectedItem = (OrderViewModelClass)((FrameworkElement)e.OriginalSource).DataContext;

            OrdersViewModel currentDataContext = (OrdersViewModel)DataContext;

            if (selectedItem != null)
            {
                currentDataContext.OrderSelected.Execute(selectedItem);
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

        private void TextBox_TextChanged( object sender, TextChangedEventArgs e )
        {
            CollectionViewSource.GetDefaultView(ItemList.ItemsSource).Refresh();
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
