using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    /// Interaction logic for LogView.xaml
    /// </summary>
    public partial class LogView : UserControl
    {
        private GridViewColumnHeader listViewSortCol = null;
        public LogView()
        {
            InitializeComponent();
            ItemList.Items.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
        }

        private void ListView_SizeChanged( object sender, SizeChangedEventArgs e )
        {
            ListView listView = sender as ListView;
            GridView gridView = listView.View as GridView;

            var width = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;

            var column1 = 0.30;
            var column2 = 0.10;
            var column3 = 0.10;
            var column4 = 0.20;
            var column5 = 0.30;


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
