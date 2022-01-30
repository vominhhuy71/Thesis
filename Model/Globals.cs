using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Google.Cloud.Firestore;
using System.Windows;

namespace InventoryManagement.Model
{
    public static class Globals
    {
        public static ObservableCollection<Order> Orders = new ObservableCollection<Order>();
        public static ObservableCollection<Reorder> Reorders = new ObservableCollection<Reorder>();
        public static ObservableCollection<Item> Items = new ObservableCollection<Item>();

        public static async void initializeInventory()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"inventory-93a0b-firebase-adminsdk-9vhm2-4938ea5892.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            FirestoreDb db = FirestoreDb.Create("inventory-93a0b");

            CollectionReference ordersRef = db.Collection("order");
            QuerySnapshot snapshot = await ordersRef.GetSnapshotAsync();

            foreach (DocumentSnapshot doc in snapshot.Documents)
            {
                Order order = new Order();
                order.Id = doc.Id;

                Dictionary<string, object> document = doc.ToDictionary();
                order.Name = (string) document["name"];
                order.SupplierId = (string) document["supplierId"];
                order.Quantity = int.Parse(document["quantity"].ToString());
                order.OrderDate = ((Timestamp) document["orderDate"]).ToDateTime();  
                order.ItemIds = parseObjectToListInt(document["itemIds"]);
                order.Type = (string) document["type"];
                order.Status = (string) document["status"];

               Orders.Add(order);
            }

        }

        #region Parsers
        private static List<int> parseObjectToListInt(object _object)
        {
            List<int> result = new List<int>();
            foreach (var item in (_object as IList<object>))
            {
                result.Add(int.Parse(item.ToString()));
            }

            return result;
        }

        #endregion

    }
}
