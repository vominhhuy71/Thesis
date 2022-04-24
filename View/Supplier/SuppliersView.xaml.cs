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
    /// Interaction logic for SuppliersView.xaml
    /// </summary>
    public partial class SuppliersView : UserControl
    {
        private GridViewColumnHeader listViewSortCol = null;

        public SuppliersView()
        {
            InitializeComponent();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ItemList.ItemsSource);
            //view.Filter = SupplierFilter;
        }
        private void listViewDoubleClick_Handler( object sender, MouseButtonEventArgs e )
        {
            dynamic o = ((FrameworkElement)e.OriginalSource).DataContext;
            if (o.GetType() != typeof(Model.Supplier)) return;

            Supplier selectedItem = (Supplier)((FrameworkElement)e.OriginalSource).DataContext;

            SuppliersViewModel currentDataContext = (SuppliersViewModel)DataContext;

            if (selectedItem != null)
            {
                currentDataContext.SupplierSelected.Execute(selectedItem);
            }
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
    }
}
