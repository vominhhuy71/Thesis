using InventoryManagement.Model;
using InventoryManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InventoryManagement.View
{
    /// <summary>
    /// Interaction logic for OrdersView.xaml
    /// </summary>
    public partial class OrdersView : UserControl
    {
        public OrdersView()
        {
            InitializeComponent();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ItemList.ItemsSource);
            view.Filter = OrderFilter;
        }
        private void listViewDoubleClick_Handler(object sender, MouseButtonEventArgs e)
        {
            ((OrdersViewModel)DataContext).OrderSelected.Execute(((ListViewItem)sender).Content);
        }

        private bool OrderFilter(object item)
        {
            if (String.IsNullOrEmpty(SearchBox.Text))
                return true;
            else
                return ((item as Order).Name.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(ItemList.ItemsSource).Refresh();
        }
    }
}
