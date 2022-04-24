using Google.Cloud.Firestore;
using InventoryManagement.View;
using NETWORKLIST;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;

namespace InventoryManagement.Model
{
    public static class Global
    {
        public static ObservableCollection<Order> Orders = new ObservableCollection<Order>();
        public static ObservableCollection<Reorder> Reorders = new ObservableCollection<Reorder>();
        public static ObservableCollection<Item> Items = new ObservableCollection<Item>();
        public static ObservableCollection<Supplier> Suppliers = new ObservableCollection<Supplier>();
        public static ObservableCollection<Log> Logs = new ObservableCollection<Log>();
        public static DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();


        public static async Task initializeInventory()
        {
            
            Orders = await Load_Orders();
            Suppliers = await Load_Suppliers();
            Items = await Load_Items();
            Logs = await Load_Logs();

        }

        /// <summary>
        /// Get DbRef to Google Firestore
        /// </summary>
        /// <returns></returns>
        private static FirestoreDb GetDbRef()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"inventory-93a0b-firebase-adminsdk-9vhm2-4938ea5892.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            return FirestoreDb.Create("inventory-93a0b");
        }

        #region Load Database
        /// <summary>
        /// Load Logs from server
        /// </summary>
        /// <returns></returns>
        public static async Task<ObservableCollection<Log>> Load_Logs()
        {
            FirestoreDb db = GetDbRef();     
            CollectionReference logsRef = db.Collection("log");
            QuerySnapshot snapshots = await logsRef.GetSnapshotAsync();

            ObservableCollection<Log> logs = new ObservableCollection<Log>();

            if (snapshots.Documents.Count == 0)
            {
                return logs;
            }

            foreach (DocumentSnapshot doc in snapshots.Documents)
            {
                try
                {
                    Log log = new Log();

                    Dictionary<string, object> document = doc.ToDictionary();
                    log.Action = (LogAction)Enum.Parse(typeof(LogAction), (string)document["action"]);
                    log.Target = (LogTarget)Enum.Parse(typeof(LogTarget), (string)document["target"]);
                    log.Name = (string)document["name"];
                    log.Description = (string)document["description"];
                    log.Date = ((Timestamp)document["date"]).ToDateTime();

                    logs.Add(log);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            Logs = logs;
            return logs;
        }

        /// <summary>
        /// Load Orders from server
        /// </summary>
        /// <returns></returns>
        public static async Task<ObservableCollection<Order>> Load_Orders()
        {
            FirestoreDb db = GetDbRef();
            CollectionReference ordersRef = db.Collection("order");
            QuerySnapshot snapshots = await ordersRef.GetSnapshotAsync();

            ObservableCollection<Order> orders = new ObservableCollection<Order>();

            if(snapshots.Documents.Count == 0)
            {
                return orders;
            }

            foreach (DocumentSnapshot doc in snapshots.Documents)
            {
                try
                {
                    Order order = new Order();
                    order.Id = doc.Id;

                    Dictionary<string, object> document = doc.ToDictionary();
                    order.Name = (string)document["name"];
                    order.SupplierId = (string)document["supplierId"];
                    order.OrderDate = ((Timestamp)document["orderDate"]).ToDateTime();
                    order.Items = parseObjectToListString(document["items"]);
                    order.Type = (OrderType)Enum.Parse(typeof(OrderType), (string)document["type"]);
                    order.Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), (string)document["status"]);
                    orders.Add(order);

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }


            }

            Orders = orders;
            return orders;
        }

        /// <summary>
        /// Load Suppliers from server
        /// </summary>
        /// <returns></returns>
        public static async Task<ObservableCollection<Supplier>> Load_Suppliers()
        {
            FirestoreDb db = GetDbRef();

            CollectionReference suppliersRef = db.Collection("supplier");
            QuerySnapshot snapshots = await suppliersRef.GetSnapshotAsync();
            ObservableCollection<Supplier> suppliers = new ObservableCollection<Supplier>();

            if (snapshots.Documents.Count == 0)
            {
                return suppliers;
            }

            foreach (DocumentSnapshot doc in snapshots)
            {
                Supplier supplier = new Supplier();

                supplier.SupplierId = doc.Id;

                Dictionary<string, object> document = doc.ToDictionary();
                supplier.Name = (string)document["name"];
                supplier.Address = (string)document["address"];
                supplier.Phone = (string)document["phone"];
                supplier.Email = (string)document["email"];

                suppliers.Add(supplier);
            }

            Suppliers = suppliers;
            return suppliers;

        }

        /// <summary>
        /// Load Items from server
        /// </summary>
        /// <returns></returns>
        public static async Task<ObservableCollection<Item>> Load_Items()
        {
            FirestoreDb db = GetDbRef();

            CollectionReference itemsRef = db.Collection("item");
            QuerySnapshot snapshots = await itemsRef.GetSnapshotAsync();
            ObservableCollection<Item> items = new ObservableCollection<Item>();

            if (snapshots.Documents.Count == 0)
            {
                return items;
            }

            foreach (DocumentSnapshot doc in snapshots)
            {
                try
                {
                    Item item = new Item();

                    item.Id = doc.Id;

                    Dictionary<string, object> document = doc.ToDictionary();
                    item.Name = (string)document["name"];
                    item.Type = (string)document["type"];
                    item.Location = (string)document["location"];
                    item.Quantity = int.Parse(document["quantity"].ToString());
                    item.ReceivedDate = ((Timestamp)document["receivedDate"]).ToDateTime();
                    item.Reorder = document["reorder"] is null ? null : (Reorder)ParseReorder((document["reorder"]));
                    items.Add(item);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

            Items = items;
            return items;
        }
        #endregion

        #region Parsers
        private static List<ItemOrdered> parseObjectToListString( dynamic _object )
        {
            List<ItemOrdered> result = new List<ItemOrdered>();
            foreach (var item in _object as IList<dynamic>)
            {
                result.Add(new ItemOrdered { ItemId = (string)item["itemId"], Quantity = int.Parse(item["quantity"].ToString()) });
            }

            return result;
        }

        private static object ParseReorder( dynamic _object )
        {

            return new Reorder { MinQuantity = int.Parse(_object["minimum"].ToString()), SupplierId = _object["supplierId"].ToString(), Quantity = int.Parse(_object["quantity"].ToString()) };
        }

        private static object parseListToObject( List<ItemOrdered> itemOrdereds )
        {
            object result = new object();
            foreach (var itemOrdered in itemOrdereds)
            {

            }
            return result;
        }

        #endregion

        #region Save
        public static async void AddLog (Log log )
        {
            FirestoreDb db = GetDbRef();
            Dictionary<string, object> newLog = new Dictionary<string, dynamic>
            {
                {"name", log.Name},
                {"date", Timestamp.FromDateTime(DateTime.SpecifyKind(log.Date, DateTimeKind.Utc)) },
                {"description", log.Description },
                {"target", log.Target.ToString()},
                {"action", log.Action.ToString()},
            };

            try
            {
                DocumentReference addedDocRef = await db.Collection("log").AddAsync(newLog);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }       

        }


        public static async Task<Boolean> AddNewItem( Item item )
        {
            FirestoreDb db = GetDbRef();

            Dictionary<string, object> newItem = new Dictionary<string, object>
            {
                {"location", item.Location },
                {"name", item.Name},
                {"type", item.Type},
                {"quantity", item.Quantity },
                {"receivedDate", Timestamp.FromDateTime(DateTime.SpecifyKind(item.ReceivedDate, DateTimeKind.Utc))}
            };


            if (!(item.Reorder is null))
            {
                Dictionary<string, object> newReorder = new Dictionary<string, object>
                {
                    {"minimum", item.Reorder.MinQuantity },
                    {"supplierId" , item.Reorder.SupplierId },
                    {"quantity" , item.Reorder.Quantity }

                };
                newItem.Add("reorder", newReorder);

            }
            else
            {
                newItem.Add("reorder", null);
            }

            try
            {
                DocumentReference addedDocRef = await db.Collection("item").AddAsync(newItem);
                AddLog(new Log { Name = $"New Item: {item.Name}", Action=LogAction.ADD, Date=DateTime.Now, Target=LogTarget.ITEM, Description=$"Quantity: {item.Quantity}, Location: {item.Location}, Type: {item.Type}." });
                if (!(item.Reorder is null))
                {
                    AddLog(new Log { Name = $"Updated Item: {item.Name} Reorder", Action = LogAction.UPDATE, Date = DateTime.Now, Target = LogTarget.ITEM, Description = $"Quantity: {item.Reorder.Quantity}, MinQuantity: {item.Reorder.MinQuantity}, SupplierId: {item.Reorder.SupplierId}." });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            return true;
        }

        public static async Task<Boolean> AddNewOrder( Order order )
        {
            FirestoreDb db = GetDbRef();

            Dictionary<string, object> newOrder = new Dictionary<string, dynamic>
            {
                {"name", order.Name},
                {"supplierId", order.SupplierId },
                {"orderDate", Timestamp.FromDateTime(DateTime.SpecifyKind(order.OrderDate, DateTimeKind.Utc)) },
                {"type", order.Type.ToString()},
                {"status", order.Status.ToString()},

            };

            ArrayList items = new ArrayList();
            foreach (var item in order.Items)
            {
                Dictionary<string, object> orderedItem = new Dictionary<string, object>
                {
                    {"quantity", item.Quantity},
                    {"itemId", item.ItemId }
                };

                items.Add(orderedItem);
            }

            newOrder.Add("items", items);

            try
            {
                DocumentReference addedDocRef = await db.Collection("order").AddAsync(newOrder);
                AddLog(new Log { Name = $"New Order {order.Name}", Action = LogAction.ADD, Date = DateTime.Now, Target = LogTarget.ORDER, Description = $"New order with Id: {addedDocRef.Id}" });

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            return true;
        }

        public static async Task<Boolean> AddNewSupplier( Supplier supplier )
        {
            FirestoreDb db = GetDbRef();

            Dictionary<string, object> newSupplier = new Dictionary<string, object>
            {
                {"name", supplier.Name},
                {"address", supplier.Address},
                {"email", supplier.Email},
                {"phone", supplier.Phone},
            };
            try
            {
                DocumentReference addedDocRef = await db.Collection("supplier").AddAsync(newSupplier);
                AddLog(new Log { Name = $"New Supplier {supplier.Name}", Action = LogAction.ADD, Date = DateTime.Now, Target = LogTarget.SUPPLIER, Description = $"New supplier {supplier.Name} with id: {addedDocRef.Id}" });

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            return true;
        }

        #endregion

        #region Update
        public static async Task<Boolean> UpdateItem( Item oldValue, Item item )
        {
            FirestoreDb db = GetDbRef();

            DocumentReference itemRef = db.Collection("item").Document(item.Id);

            Dictionary<string, object> updateItem = new Dictionary<string, object>
            {
                {"location", item.Location },
                {"name", item.Name},
                {"type", item.Type},
                {"quantity", item.Quantity },
                {"receivedDate", Timestamp.FromDateTime(DateTime.SpecifyKind(item.ReceivedDate, DateTimeKind.Utc))}
            };


            if (!(item.Reorder is null))
            {
                Dictionary<string, object> updateReorder = new Dictionary<string, object>
                {
                    {"minimum", item.Reorder.MinQuantity },
                    {"supplierId" , item.Reorder.SupplierId },
                    {"quantity", item.Reorder.Quantity }
                };
                updateItem.Add("reorder", updateReorder);

            }
            else
            {
                updateItem.Add("reorder", null);
            }
            try
            {

                // Reorder rule added 
                if (!(item.Reorder is null) && (oldValue.Reorder is null))
                {
                    AddLog(new Log { Name = $"Item {item.Name} updated", Action = LogAction.UPDATE, Date = DateTime.Now, Target = LogTarget.ITEM, Description = $"Reorder added with quantity {item.Reorder.Quantity} and minQuantity {item.Reorder.MinQuantity}" });
                }

                // Reorder rule removed 
                if ((item.Reorder is null) && !(oldValue.Reorder is null))
                {
                    AddLog(new Log { Name = $"Item {item.Name} updated", Action = LogAction.UPDATE, Date = DateTime.Now, Target = LogTarget.ITEM, Description = $"Reorder removed with quantity {oldValue.Reorder.Quantity} and minQuantity {oldValue.Reorder.MinQuantity}" });
                }

                // Type Changed
                if (item.Type != oldValue.Type)
                {
                    AddLog(new Log { Name = $"Item {item.Name} updated", Action = LogAction.UPDATE, Date = DateTime.Now, Target = LogTarget.ITEM, Description = $"Type updated from {oldValue.Type} to {item.Type}" });
                }

                // Location Changed
                if (item.Location != oldValue.Location)
                {
                    AddLog(new Log { Name = $"Item {item.Name} updated", Action = LogAction.UPDATE, Date = DateTime.Now, Target = LogTarget.ITEM, Description = $"Location updated from {oldValue.Location} to {item.Location}" });

                }

                // Name Changed
                if (item.Name != oldValue.Name)
                {
                    AddLog(new Log { Name = $"Item {item.Name} updated", Action = LogAction.UPDATE, Date = DateTime.Now, Target = LogTarget.ITEM, Description = $"Name updated from {oldValue.Name} to {item.Name}" });

                }

                // Quantity Changed
                if (item.Quantity != oldValue.Quantity)
                {
                    AddLog(new Log { Name = $"Item {item.Name} updated", Action = LogAction.UPDATE, Date = DateTime.Now, Target = LogTarget.ITEM, Description = $"Quantity updated from {oldValue.Quantity} to {item.Quantity}" });
                   
                }


                await itemRef.UpdateAsync(updateItem);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            return true;
        }

        public static async Task<Boolean> UpdateOrder( string id, string orderStatus, List<ItemOrderedDetail> Items )
        {
            FirestoreDb db = GetDbRef();

            DocumentReference orderRef = db.Collection("order").Document(id);

            Dictionary<string, object> updateOrder = new Dictionary<string, object>
            {
                {"status", orderStatus}
            };

            try
            {
                await orderRef.UpdateAsync(updateOrder);
                AddLog(new Log { Name = $"Order {orderRef.Id} updated", Action = LogAction.UPDATE, Date = DateTime.Now, Target = LogTarget.ORDER, Description = $"Order status is updated to {orderStatus}" });
                if (orderStatus == OrderStatus.FINISHED.ToString())
                {
                    for (int i = 0; i < Items.Count; i++)
                    {
                        DocumentReference itemRef = db.Collection("item").Document(Items[i].Item.Id);
                        int newQuantity = Global.Items.Where(x => x.Id == Items[i].Item.Id).FirstOrDefault().Quantity + Items[i].Quantity;

                        Dictionary<string, object> updateItem = new Dictionary<string, object>
                        {
                            {"quantity",  newQuantity },

                        };

                        await itemRef.UpdateAsync(updateItem);

                    }


                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }


            return true;
        }

        public static async Task<Boolean> UpdateSupplier( Supplier supplier )
        {

            FirestoreDb db = GetDbRef();

            DocumentReference supplierRef = db.Collection("supplier").Document(supplier.SupplierId);

            Dictionary<string, object> updateSupplier = new Dictionary<string, object>
            {
                {"name", supplier.Name},
                {"address", supplier.Address},
                {"email", supplier.Email},
                {"phone", supplier.Phone},
            };

            try
            {
                await supplierRef.UpdateAsync(updateSupplier);
                AddLog(new Log { Name = $"Supplier {supplierRef.Id} updated", Action = LogAction.UPDATE, Date = DateTime.Now, Target = LogTarget.SUPPLIER, Description = $"Supplier Info is updated!" });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }


            return true;
        }

        #endregion

        #region Delete
        public static async Task<Boolean> DeleteItem( string ItemId )
        {
            FirestoreDb db = GetDbRef();
            DocumentReference itemRef = db.Collection("item").Document(ItemId);
            try
            {
                await itemRef.DeleteAsync();
                AddLog(new Log { Name = $"Item: {ItemId} is deleted!", Action = LogAction.DELETE, Date = DateTime.Now, Target = LogTarget.ITEM, Description = $"Item is deleted!" });

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            return true;
        }

        public static async Task<Boolean> DeleteOrder( string OrderId )
        {
            FirestoreDb db = GetDbRef();
            DocumentReference orderRef = db.Collection("order").Document(OrderId);
            try
            {
                await orderRef.DeleteAsync();
                AddLog(new Log { Name = $"Order: {OrderId} is deleted!", Action = LogAction.DELETE, Date = DateTime.Now, Target = LogTarget.ORDER, Description = $"Order is deleted!" });

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            return true;
        }

        public static async Task<Boolean> DeleteSupplier( string SupplierId )
        {
            FirestoreDb db = GetDbRef();
            DocumentReference supplierRef = db.Collection("supplier").Document(SupplierId);
            try
            {
                await supplierRef.DeleteAsync();
                AddLog(new Log { Name = $"Supplier: {SupplierId} is deleted!", Action = LogAction.DELETE, Date = DateTime.Now, Target = LogTarget.SUPPLIER, Description = $"Supplier is deleted!" });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            return true;
        }
        #endregion

        #region Send Email
        public static void SendEmail( string SupplierId, int quantity, string itemName )
        {
            Supplier supplier = Suppliers.Single(x => x.SupplierId == SupplierId);
            if (supplier == null)
            {
                MessageBox.Show("Unable to send Reorder email. Supplier didn't exist!");
            }
            try
            {
                string Body = $"<h3>Hello {supplier.Name},</h3>" + $"<div>I need {quantity} for {itemName}! Please deliver it as soon as possible</div>" + $"<div>Best regards!</div><div>Huy</div>";

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("", ""),
                    EnableSsl = true,
                };
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("huyvothesisemail@gmail.com"),
                    Subject = "Reorder",
                    Body = Body,

                    IsBodyHtml = true,
                };
                mailMessage.To.Add(supplier.Email);
                smtpClient.Send(mailMessage);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
        #endregion

        #region DispatchTimer
        public static void InitializeDispatchTimer( EventHandler eventHandler )
        {
            if (dispatcherTimer.IsEnabled)
            {
                dispatcherTimer.Stop();
                var eventField = dispatcherTimer.GetType().GetField("Tick", BindingFlags.NonPublic | BindingFlags.Instance);
                var eventDelegate = (Delegate)eventField.GetValue(dispatcherTimer);
                var invocatationList = eventDelegate.GetInvocationList();
                foreach (var handler in invocatationList)
                {
                    dispatcherTimer.Tick -= ((EventHandler)handler);
                }
            }

            dispatcherTimer.Tick += new EventHandler(eventHandler);
            dispatcherTimer.Interval = new TimeSpan(0, 1, 0);
            dispatcherTimer.Start();
        }

        #endregion

        #region Helper
        public static bool CheckConnectionToInternet()
        {
            INetworkListManager networkListManager = new NetworkListManager();

            return networkListManager.IsConnectedToInternet;
        }
        #endregion

    }
}
