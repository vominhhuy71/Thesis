using InventoryManagement.Model;
using InventoryManagement.ViewModel;
using System;
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
            view.Filter = ItemFilter;
        }
        private void listViewDoubleClick_Handler(object sender, MouseButtonEventArgs e)
        {
            ((HomeViewModel)DataContext).ItemSelected.Execute(((ListViewItem)sender).Content);
        }

        private bool ItemFilter(object item)
        {
            if (String.IsNullOrEmpty(SearchBox.Text))
                return true;
            else
                return ((item as Item).Name.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(ItemList.ItemsSource).Refresh();
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
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
