using System.Windows;
using System.Windows.Controls;

namespace InventoryManagement.View
{
    /// <summary>
    /// Interaction logic for OrderDetail.xaml
    /// </summary>
    public partial class OrderDetail : Window
    {
        public OrderDetail()
        {
            InitializeComponent();
        }

        private void ListView_SizeChanged( object sender, SizeChangedEventArgs e )
        {
            ListView listView = sender as ListView;
            GridView gridView = listView.View as GridView;

            var width = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;

            var column1 = 0.50;
            var column2 = 0.50;

            gridView.Columns[0].Width = width * column1;
            gridView.Columns[1].Width = width * column2;


        }

    }
}
