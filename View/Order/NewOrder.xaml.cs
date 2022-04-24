using InventoryManagement.Model;
using InventoryManagement.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InventoryManagement.View
{
    /// <summary>
    /// Interaction logic for NewOrder.xaml
    /// </summary>
    public partial class NewOrder : Window
    {

        public NewOrder()
        {
            InitializeComponent();
        }

        private void ListView_SizeChanged( object sender, SizeChangedEventArgs e )
        {
            ListView listView = sender as ListView;
            GridView gridView = listView.View as GridView;

            var width = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;

            var column1 = 0.30;
            var column2 = 0.35;
            var column3 = 0.35;


            gridView.Columns[0].Width = width * column1;
            gridView.Columns[1].Width = width * column2;
            gridView.Columns[2].Width = width * column3;

        }

        private void listViewDoubleClick_Handler( object sender, MouseButtonEventArgs e )
        {
            ItemOrderedDetail selectedItem = (ItemOrderedDetail)((FrameworkElement)e.OriginalSource).DataContext;

            NewOrderViewModel currentDataContext = (NewOrderViewModel)DataContext;

            if (selectedItem != null)
            {
                currentDataContext.RemoveSelectedItemButton.Execute(selectedItem);
            }
        }

    }
}
