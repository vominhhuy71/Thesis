using InventoryManagement.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace InventoryManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        async protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);



            await Task.Run(() => Globals.initializeInventory());
        }
    }
}
