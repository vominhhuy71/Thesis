using InventoryManagement.Model;
using System.Windows;

namespace InventoryManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup( StartupEventArgs e )
        {
            InitializeDatabase();
            base.OnStartup(e);
        }

        async void InitializeDatabase()
        {
            bool tryConnection = Global.CheckConnectionToInternet();
            if (tryConnection)
            {
                await Global.initializeInventory();

            }
            else
            {
                MessageBox.Show("Not connect to the internet!");
            }
        }

    }
}
