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
    /// Interaction logic for SuppliersView.xaml
    /// </summary>
    public partial class SuppliersView : UserControl
    {
        public SuppliersView()
        {
            InitializeComponent();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(SupplierList.ItemsSource);
            view.Filter = SupplierFilter;
        }
        private void listViewDoubleClick_Handler(object sender, MouseButtonEventArgs e)
        {
            ((SuppliersViewModel)DataContext).SupplierSelected.Execute(((ListViewItem)sender).Content);
        }

        private bool SupplierFilter(object supplier)
        {
            if (String.IsNullOrEmpty(SearchBox.Text))
                return true;
            else
                return ((supplier as Model.Supplier).Name.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(SupplierList.ItemsSource).Refresh();
        }
    }
}
